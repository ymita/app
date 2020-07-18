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
        public List<TagInView> TagsInView { get; set; } = new List<TagInView>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userName = User.Identity.Name;
            var posts = await _appRepository.getPostsByUserAsync(userName);
            Post = posts.Find(x => x.Id == id);

            // AllTags に対して Tags の内容を反映し、TagsInView を生成する。
            //var allTags = context.Tags.ToList();
            //this.AllTags = await this._context.Tags.ToListAsync();
            //foreach(var item in this.AllTags)
            //{
            //    System.Diagnostics.Debug.WriteLine(item.Id);
            //    if (posts.Exists(x=>x.Id == item.Id))
            //    {
            //        System.Diagnostics.Debug.WriteLine(item.Id);
            //    }
            //}
            var userId = _userManager.GetUserId(User);
            string sql = "SELECT Id, PostId, TagId from dbo.Posts_Tags_XREF WHERE PostId in (SELECT Id from dbo.Posts WHERE OwnerId = '" + userId + "')";
            var visiblePostsTagsCrossReferences = this._context.PostsTagsCrossReferences.FromSqlRaw(sql).ToList();
            visiblePostsTagsCrossReferences = visiblePostsTagsCrossReferences.GroupBy(x => x.TagId).Select(g => g.First()).ToList();

            sql = "SELECT Id, PostId, TagId from dbo.Posts_Tags_XREF WHERE PostId in (SELECT Id from dbo.Posts WHERE OwnerId = '" + userId + "') AND PostId = " + Post.Id;
            var selectedPostsTags = this._context.PostsTagsCrossReferences.FromSqlRaw(sql).ToList();

            var visibleTags = new List<Tag>();
            foreach(var item in visiblePostsTagsCrossReferences)
            {
                visibleTags.Add(
                    this._context.Tags.Where(x => x.Id == item.TagId).FirstOrDefault()
                );
            }

            List<int> tagIDs = new List<int>();
            foreach(var tag in selectedPostsTags)
            {
                tagIDs.Add(tag.TagId);
            }

            this.TagsInView = visibleTags.ConvertAll(x => new TagInView() {
                                    Id = x.Id,
                                    TagName = x.TagName,
                                    IsSelected = tagIDs.Contains(x.Id)
                                }).ToList();

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
            
            // Post に紐づくTags
            //var tags = this._context.Tags.Where(x => x.PostId == Post.Id).ToList();

            /* TagsInView は新たに Post に紐づく Tags */
            var tagsToAdd = new List<Tag>();
            var tagsToDelete = new List<Tag>();
            // tags になくて TagsInView で IsSelected = true のものは新規追加
            // tags にあって TagsInView で IsSelected = false のものは削除
            var selectedTags = TagsInView.Where(x => x.IsSelected == true).ToList();
            //for (int i = 0; i < selectedTags.Count; i++)
            //{
            //    if (!tags.Exists(x=> x.TagName == selectedTags[i].TagName))
            //    {
            //        //this.AllTags から追加、削除する Id を割り出して、Add する。
            //        tagsToAdd.Add(
            //            new Tag
            //            {
            //                TagName = selectedTags[i].TagName,
            //                //PostId = this.Post.Id
            //            }
            //        );
            //    }
            //}

            // データベース上で既に Post に紐付いているタグはそのまま
            string sql = "SELECT Id, PostId, TagId from dbo.Posts_Tags_XREF WHERE PostId in (SELECT Id from dbo.Posts WHERE OwnerId = '" + userId + "') AND PostId = " + Post.Id;
            var postTags = this._context.PostsTagsCrossReferences.FromSqlRaw(sql).ToList();
            // データベース上で、まだ Post に紐付いていないタグは追加対象とする => コミットする。
            foreach (var item in selectedTags)
            {
                Tag tag = new Tag() { Id = item.Id, TagName = item.TagName };
                if (!postTags.Any(x => x.TagId == item.Id))
                {
                    var ptxref = new PostTagCrossReference() { PostId = Post.Id, TagId = item.Id };
                    this._context.PostsTagsCrossReferences.Add(ptxref);
                }
            }

            // データベース上で PostsTagsRelation テーブルで Post に紐付いているタグは削除する => コミットする。
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
            // どの Post とも紐付いていないタグは Tags テーブルから削除する => コミットする。

                //this.AllTags から追加、削除する Id を割り出して、Delete する。
                /*this.AllTags.Where(x => x.PostId == Post.Id).OrderBy(x => x.TagName).ToList();*/
            for (int i = 0; i < unselectedTags.Count; i++)
            {
                this.AllTags = await this._context.Tags.ToListAsync();
                //var tagToDelete = this.AllTags.Where(x => x.PostId == Post.Id && x.TagName == unselectedTags[i].TagName).FirstOrDefault();
                //if(tagToDelete != null)
                //{
                //    tagsToDelete.Add(tagToDelete);
                //}
            }
            //if (!tags.Contains(unselectedTags[i]))
            //{
            //    tagsToDelete.Add(unselectedTags[i]);
            //}


            //this._context.Tags.AddRange(tagsToAdd);
            this._context.Tags.RemoveRange(tagsToDelete);

            // どちらにもあるものは Stay

            //for (int i = 0; i < TagsInView.Count; i++)
            //{
            //    if(TagsInView[i].IsSelected)
            //    {
            //        var t = new Tag();
            //        t.PostId = Post.Id;
            //        t.TagName = TagsInView[i].TagName;
            //        this._context.Tags.Add(t);
            //    }
            //}

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

            return Page();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }

    public class TagInView : Tag
    {
        public bool IsSelected { get; set; }
    }
}
