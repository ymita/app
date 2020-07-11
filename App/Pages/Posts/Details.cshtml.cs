using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task OnGet(int id)
        {
            this.Post = await this._appRepository.getPost(id);
        }
    }
}