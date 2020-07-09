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
    public class DetailModel : PageModel
    {
        private readonly IAppRepository _appRepository;
        public Post Post { get; set; }
        public DetailModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }

        public void OnGet(int id)
        {
            var post = this._appRepository.getPost(id);
            this.Post = post;
        }
    }
}
