using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages.Search
{
    public class TagModel : PageModel
    {
        private readonly IAppRepository _appRepository;
        public List<Post> Posts { get; set; }
        public TagModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }
        public async Task OnGet(string tag)
        {
            var posts = await this._appRepository.getPostsByTag(tag);
            this.Posts = posts;
        }
    }
}