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

namespace App.Pages.Dashboard.Posts
{
    public class DeleteModel : PageModel
    {
        private readonly App.Data.AppDbContext _context;
        private readonly IAppRepository _appRepository;

        public DeleteModel(App.Data.AppDbContext context,
            IAppRepository appRepository)
        {
            _context = context;
            _appRepository = appRepository;
        }

        [BindProperty]
        public Post Post { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userName = User.Identity.Name;
            var res = await _appRepository.getPostsByUserAsync(userName);
            Post = res.Find(x => x.Id == id);

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = await _context.Posts.FindAsync(id);

            if (Post != null)
            {
                _context.Posts.Remove(Post);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
