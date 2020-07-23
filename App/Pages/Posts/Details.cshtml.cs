using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages.Posts
{
    public class DetailsModel : PageModel
    {
        private readonly IAppRepository _appRepository;
        public Post Post { get; set; }
        public DetailsModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }

        public async Task<IActionResult> OnGet(string userName, int id)
        {
            try {
                var post = await this._appRepository.getPost(id, userName);
                if(post.IsDraft)
                {
                    return NotFound();
                }
                this.Post = post;
            }
            catch(HttpResponseException hrex) {
                System.Diagnostics.Debug.WriteLine(hrex.Message);
            }
            return Page();
        }
    }
}