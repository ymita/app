using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.User.Models;
using App.Data;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Areas.User.Pages
{
    public class EntryModel : PageModel
    {
        private IAppRepository _appRepository { get; set; }
        public Post Post { get; set; }
        public EntryModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }

        public void OnGet(string userName = null, Guid? postId = null)
        {
            if(string.IsNullOrEmpty(userName) || !postId.HasValue) {
                return;
            }
            var post = this._appRepository.getPost(postId.Value);
            this.Post = post;
        }
    }
}
