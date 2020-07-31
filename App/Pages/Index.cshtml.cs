using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
using App.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace App.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IIdentityRepository _identityRepository;
        private readonly IAppRepository _appRepository;
        private readonly IUserService _userService;

        public IList<Post> Posts { get; set; }
        public List<Tag> Tags { get; set; }
        public IList<string> Owners { get; set; } = new List<string>();
        public IndexModel(ILogger<IndexModel> logger,
            IUserService userService,
            IAppRepository appRepository)
        {
            _logger = logger;
            this._userService = userService;
            this._appRepository = appRepository;
        }

        public async Task OnGet()
        {
            this.Posts = await this._userService.GetAllPostsAsync();
            this.Owners = await this._userService.GetOwnersAsync(this.Posts);
            this.Tags = await this._appRepository.getAllTagsAsync();
        }
    }
}
