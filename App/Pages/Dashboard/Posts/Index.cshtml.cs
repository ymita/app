using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.Repositories;

namespace App.Pages.Dashboard.Posts
{
    public class IndexModel : PageModel
    {
        private readonly IAppRepository _appResository;

        public IndexModel(IAppRepository appRepository)
        {
            this._appResository = appRepository;
        }

        public IList<Post> Posts { get;set; }

        public async Task OnGetAsync()
        {
            var userName = User.Identity.Name;
            var posts = await this._appResository.getPostsByUserAsync(userName);
            Posts = posts;
        }


        public async Task OnPostAsync()
        {
            var userName = User.Identity.Name;
            var post = await this._appResository.getPostsByUserAsync(userName);
        }
    }
}
