﻿using System;
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
        public List<string> Owners { get; set; } = new List<string>();
        public IndexModel(ILogger<IndexModel> logger,
            IIdentityRepository identityRepository,
            IAppRepository appRepository,
            IUserService userService)
        {
            _logger = logger;
            _identityRepository = identityRepository;
            _appRepository = appRepository;
            this._userService = userService;
        }

        public async Task OnGet()
        {
            Posts = await this._userService.GetAllPostsAsync();
            for(int i = 0; i < Posts.Count; i++)
            {
                var ownerId = Posts[i].OwnerId;
                var userName = await _identityRepository.getUserNameByIdAsync(ownerId);
                Owners.Add(userName);
            }
            this.Tags = await this._appRepository.getAllTagsAsync();
        }
    }
}
