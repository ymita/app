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
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
        public List<string> PostOwners { get; set; } = new List<string>();
        private readonly ILogger<IndexModel> _logger;
        private readonly IAppRepository _appRepository;
        private readonly IIdentityRepository _identityRepository;

        public IndexModel(ILogger<IndexModel> logger,
            IAppRepository appRepository,
            IIdentityRepository identityRepository)
        {
            _logger = logger;
            _appRepository = appRepository;
            _identityRepository = identityRepository;
        }

        public void OnGet()
        {
            Posts = _appRepository.getAllPosts();
            for(int i = 0; i < Posts.Count; i++)
            {
                var userName = _identityRepository.getUserNameById(Posts[i].OwnerId);
                PostOwners.Add(userName);
            }
        }
    }
}
