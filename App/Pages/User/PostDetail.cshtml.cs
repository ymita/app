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
    public class PostDetailModel : PageModel
    {
        private readonly IAppRepository _appRepository;
        public Post Post { get; set; }
        public string UserName { get; set; }
        public List<Tag> Tags { get; set; }

        public PostDetailModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }
        public async Task<IActionResult> OnGet(string userName, int id)
        {
            this.UserName = userName;
            try
            {
                var post = await this._appRepository.getPost(id, userName);
                if (post.IsDraft)
                {
                    return NotFound();
                }
                
                // Post の取得
                this.Post = post;

                // Post に紐づく Tags の取得
                this.Tags = await this._appRepository.getTags(id);

            }
            catch (Exception hrex)
            {
                return NotFound();
            }
            return Page();
        }
    }
}