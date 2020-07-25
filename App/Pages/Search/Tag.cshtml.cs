using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages.Search
{
    public class TagModel : PageModel
    {
        private readonly IAppRepository _appRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityRepository _identityRepository;

        public List<Post> Posts { get; set; }
        public List<string> PostOwners { get; set; } = new List<string>();
        public string Tag { get; set; }
        public TagModel(IAppRepository appRepository,
            UserManager<IdentityUser> userManager,
            IIdentityRepository identityRepository)
        {
            this._appRepository = appRepository;
            this._userManager = userManager;
            this._identityRepository = identityRepository;
        }
        public async Task OnGet(string tag)
        {
            this.Tag = tag;

            var posts = await this._appRepository.getPostsByTag(tag);
            this.Posts = posts;

            for (int i = 0; i < this.Posts.Count; i++) { 
                var post = this.Posts[i];
                var userName = this._identityRepository.getUserNameById(post.OwnerId);
                this.PostOwners.Add(userName);
            }
        }
    }
}