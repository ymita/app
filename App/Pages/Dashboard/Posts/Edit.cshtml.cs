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
        public List<Tag> Tags { get; set; }
        private List<Tag> AllTags { get; set; }
        [BindProperty]
        public List<TagInView> TagsInView { get; set; } = new List<TagInView>();/* TagsInView は新たに Post に紐づく Tags */

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

            //var userId = _userManager.GetUserId(User);

            //// 現在ユーザーの全ての Post と、対応する Tag の関係表を取得する
            //var visiblePostsTagsCrossReferences = 
            //    await _appRepository.getPostsTagsReferencesAsync(userId);

            //// 現在の Post と、対応する Tag の関係表を取得する
            //var currentPostTags = 
            //    await _appRepository.getPostTagsReferencesByPostIdAsync(userId, Post.Id);

            // 現在ユーザーの全ての Post と、対応する Tag の関係表から、
            // 現在ユーザーが使ったことのある Tag リストを構築する。
            //var visibleTags = new List<Tag>();
            //foreach(var item in visiblePostsTagsCrossReferences)
            //{
            //    visibleTags.Add(
            //        this._context.Tags.Where(x => x.Id == item.TagId).FirstOrDefault()
            //    );
            //}

            //// 現在の Post に対応する Tag の ID をリストとして構築する。
            //List<int> currentPostTagIDs = new List<int>();
            //foreach(var tag in currentPostTags)
            //{
            //    currentPostTagIDs.Add(tag.TagId);
            //}

            // View に表示するタグ一覧(選択/未選択状態を含めて)を構築する。
            this.TagsInView = await this.getTagsInView();
            //// View に表示するタグ一覧(選択/未選択状態を含めて)を構築する。
            //this.TagsInView = visibleTags.ConvertAll(x => new TagInView() {
            //                        Id = x.Id,
            //                        TagName = x.TagName,
            //                        IsSelected = currentPostTagIDs.Contains(x.Id)
            //                    }).ToList();

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostNewAsync(int? id)
        {
            if (!id.HasValue && id != -1)
            {
                return RedirectToPage("./Index");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Post オブジェクトを保存

            //これから編集する Post オブジェクトを用意
            Post = new Post();
            var userId = _userManager.GetUserId(User);
            Post.OwnerId = userId;
            Post.PublishedDate = DateTime.Now;
            Post.IsDraft = true;

            //これから編集する Tag コレクションを用意
            Tags = new List<Tag>();

            try
            {
                await _context.Posts.AddAsync(Post);
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

            // ページ遷移
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
            
            // データベース上で、まだ Post に紐付いていないタグは追加対象とする
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

            return Page();
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
