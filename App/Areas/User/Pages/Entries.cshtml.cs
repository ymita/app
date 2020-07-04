using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.User.Models;
using App.Data;
using App.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Areas.User.Pages
{
    public class EntriesModel : PageModel
    {
        public IAppRepository _appRepository { get; set; }

        public List<Post> Posts { get; set; }

        public string UserName { get; set; }

        public EntriesModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }

        public void OnGet(string userName)
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
