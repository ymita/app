using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Areas.Dashboard.Pages
{
    public class PostsModel : PageModel
    {
        private readonly IAppRepository _appRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public List<Post> Posts { get; set; }

        public PostsModel(
            IAppRepository appRepository,
            UserManager<IdentityUser> userManager)
        {
            this._appRepository = appRepository;
            this._userManager = userManager;
        }
        
        public void OnGet()
        {
            var userName = _userManager.GetUserName(User);
            this.Posts = this._appRepository.getPostsByUser(userName);
        }
    }
}
