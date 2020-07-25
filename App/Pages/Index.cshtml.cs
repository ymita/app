using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
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
        public List<Post> Posts { get; set; }
        public List<string> Owners { get; set; } = new List<string>();
        public IndexModel(ILogger<IndexModel> logger,
            IIdentityRepository identityRepository,
            IAppRepository appRepository)
        {
            _logger = logger;
            _identityRepository = identityRepository;
            _appRepository = appRepository;
        }

        public async Task OnGet()
        {
            Posts = await _appRepository.getAllPosts();
            for(int i = 0; i < Posts.Count; i++)
            {
                var ownerId = Posts[i].OwnerId;
                var userName = await _identityRepository.getUserNameByIdAsync(ownerId);
                Owners.Add(userName);
            }
        }
    }
}
