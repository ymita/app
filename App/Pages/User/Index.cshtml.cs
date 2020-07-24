using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly IAppRepository _appResository;

        public IndexModel(IAppRepository appRepository)
        {
            this._appResository = appRepository;
        }

        public string UserName { get; set; }
        public IList<Post> Posts { get; set; }

        public async Task OnGetAsync(string userName)
        {
            if(string.IsNullOrEmpty(userName))
            {
                return;
            }

            this.UserName = userName;
            var posts = await this._appResository.getPostsByUserAsync(userName);
            Posts = posts;
        }
    }
}