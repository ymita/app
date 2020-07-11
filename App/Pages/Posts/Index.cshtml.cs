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
    public class IndexModel : PageModel
    {
        public List<Post> Posts { get; set; }
        private readonly IAppRepository _appRepository;
        public IndexModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }
        public async Task OnGet()
        {
            this.Posts = await this._appRepository.getAllPosts();
        }
    }
}