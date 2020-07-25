using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using App.Repositories;
using System.ComponentModel;
using System.Text;

namespace App.Pages.Dashboard.Posts
{
    public class EditModel : PageModel
    {
        private readonly App.Data.AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppRepository _appRepository;

        public EditModel(App.Data.AppDbContext context,
            UserManager<IdentityUser> userManager,
            IAppRepository appRepository)
        {
            _context = context;
            _userManager = userManager;
            _appRepository = appRepository;
        }

        [BindProperty]
        public Post Post { get; set; }
        [DisplayName("タグ")]
        public List<Tag> Tags { get; set; }
        [BindProperty]
        public List<TagInView> TagsInView { get; set; } = new List<TagInView>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 該当する id の Post を取得する
            var userName = User.Identity.Name;
            var posts = await _appRepository.getPostsByUserAsync(userName);
            Post = posts.Find(x => x.Id == id);

            // View に表示するタグ一覧(選択/未選択状態を含めて)を構築する。
            this.TagsInView = await this.getTagsInView();

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            Post.OwnerId = userId;

            var isDraft = Request.Form["draft"].Count == 1 ? true : false;
            Post.IsDraft = isDraft;

            _context.Attach(Post).State = EntityState.Modified;


            var selectedTagsInView = TagsInView.Where(x => x.IsSelected == true).ToList();
            
            string sql = "SELECT Id, PostId, TagId from dbo.Posts_Tags_XREF WHERE PostId in (SELECT Id from dbo.Posts WHERE OwnerId = '" + userId + "') AND PostId = " + Post.Id;
            var postTags = this._context.PostsTagsCrossReferences.FromSqlRaw(sql).ToList();
            
            // ユーザーが既に利用しているが、まだこの Post には紐付いていないタグは追加対象とする
            foreach (var item in selectedTagsInView)
            {
                Tag tag = new Tag() { Id = item.Id, TagName = item.TagName };
                if (!postTags.Any(x => x.TagId == item.Id))
                {
                    var ptxref = new PostTagCrossReference() { PostId = Post.Id, TagId = item.Id };
                    this._context.PostsTagsCrossReferences.Add(ptxref);
                }
            }
            
            // データベース上で PostsTagsRelation テーブルで Post に紐付いているタグは削除する
            var unselectedTags = TagsInView.Where(x => x.IsSelected == false).ToList();
            foreach (var item in unselectedTags)
            {
                Tag tag = new Tag() { Id = item.Id, TagName = item.TagName };
                if(postTags.Any(x=>x.TagId == tag.Id))
                {
                    var ptxref = this._context.PostsTagsCrossReferences.Where(x=>x.TagId == tag.Id).FirstOrDefault();
                    this._context.PostsTagsCrossReferences.Remove(ptxref);
                }
            }
            // TO DO: どの Post とも紐付いていないタグは Tags テーブルから削除する => コミットする。
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM dbo.Tags ");
            sb.Append("WHERE Id IN( ");
            sb.Append("    SELECT Id from dbo.Tags");
            sb.Append("    WHERE Id NOT IN(");
            sb.Append("        SELECT TagId from dbo.Posts_Tags_XREF");
            sb.Append("        WHERE PostId IN (");
            sb.Append("            SELECT Id from dbo.Posts");
            sb.Append("            ");
            sb.Append("            WHERE OwnerId = '" + userId +"'");
            sb.Append("        )");
            sb.Append("    )");
            sb.Append(")");
            this._context.Database.ExecuteSqlRaw(sb.ToString());

            // TO DO: //Request.Form.ToList()
            var newlyCreatedTags = Request.Form.Where(x => x.Key.Contains("newTag")).ToList();
            if(newlyCreatedTags.Count > 0)
            {
                int newlyCreatedTagsCount = int.Parse(newlyCreatedTags.Last().Key.Split("_")[1]);

                for (int i = 0; i <= newlyCreatedTagsCount; i++)
                {
                    var newlyCreatedTagPair = newlyCreatedTags.Where(x => x.Key.StartsWith("newTag_" + i)).ToList();
                    if (newlyCreatedTagPair.Count == 2)
                    {
                        if (
                            newlyCreatedTagPair[0].Key == "newTag_" + i + "_IsSelected"
                            &&
                            newlyCreatedTagPair[1].Key == "newTag_" + i + "_TagName"
                            )
                        {
                            // newlyCreatedTagPair[1].Value を新しいタグとして登録する。
                            System.Diagnostics.Debug.WriteLine(newlyCreatedTagPair[1].Value);
                            var tagName = newlyCreatedTagPair[1].Value.ToString();

                            Tag tag = new Tag() { TagName = tagName };
                            var res = this._context.Tags.Add(tag);

                            await _context.SaveChangesAsync();
                            var ptxref = new PostTagCrossReference() { PostId = Post.Id, TagId = res.Entity.Id };
                            this._context.PostsTagsCrossReferences.Add(ptxref);
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // TagsInView の更新
            this.TagsInView = await this.getTagsInView();
            return RedirectToPage("./Edit", new { id = this.Post.Id });
            //return Page();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }


        private async Task<List<TagInView>> getTagsInView()
        {
            var userId = _userManager.GetUserId(User);

            // 現在ユーザーの全ての Post と、対応する Tag の関係表を取得する
            var visiblePostsTagsCrossReferences =
                await _appRepository.getPostsTagsReferencesAsync(userId);

            // 現在の Post と、対応する Tag の関係表を取得する
            var currentPostTags =
                await _appRepository.getPostTagsReferencesByPostIdAsync(userId, Post.Id);

            var visibleTags = new List<Tag>();
            foreach (var item in visiblePostsTagsCrossReferences)
            {
                visibleTags.Add(
                    this._context.Tags.Where(x => x.Id == item.TagId).FirstOrDefault()
                );
            }

            // 現在の Post に対応する Tag の ID をリストとして構築する。
            List<int> currentPostTagIDs = new List<int>();
            foreach (var tag in currentPostTags)
            {
                currentPostTagIDs.Add(tag.TagId);
            }

            // View に表示するタグ一覧(選択/未選択状態を含めて)を構築する。
            var tagsInView = visibleTags.ConvertAll(x => new TagInView()
            {
                Id = x.Id,
                TagName = x.TagName,
                IsSelected = currentPostTagIDs.Contains(x.Id)
            }).ToList();

            return tagsInView;
        }
    }

    public class TagInView : Tag
    {
        public bool IsSelected { get; set; }
    }
}
