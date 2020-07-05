using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Areas.Posts.Pages
{
    public class IndexModel : PageModel
    {
        public IAppRepository _appRepository { get; set; }

        public List<Post> Posts { get; set; }

        public string UserName { get; set; }

        public IndexModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }

        public void OnGet(string userName = "test@test.com")
        {
            if (string.IsNullOrEmpty(userName))
            {
                return;
            }

            this.UserName = userName;
            this.Posts = this._appRepository.getPostsByUser(userName);
        }
    }
}
