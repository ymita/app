using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Dashboard.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly App.Data.AppDbContext _context;
        private readonly IAppRepository _appRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public IndexModel(App.Data.AppDbContext context,
            IAppRepository appRepository,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _appRepository = appRepository;
            _userManager = userManager;
        }

        public IList<Post> Posts { get;set; }

        public async Task OnGetAsync()
        {
            Posts = Posts = await this.GetPostsByCurrentUser();
        }


        public async Task OnPostAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var post = _context.Posts.Where(p => p.OwnerId == userId).ToList().Find(p => p.Id == id);

            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            Posts = await this.GetPostsByCurrentUser();
        }

        private async Task<List<Post>> GetPostsByCurrentUser()
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Posts.Where(p => p.OwnerId == userId).ToListAsync();
        }
    }
}
