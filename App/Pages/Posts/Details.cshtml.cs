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

        public async Task OnGet(string userName, int id)
        {
            try {
                this.Post = await this._appRepository.getPost(id, userName);
            }
            catch(HttpResponseException hrex) {
                System.Diagnostics.Debug.WriteLine(hrex.Message);
            }
        }
    }
}