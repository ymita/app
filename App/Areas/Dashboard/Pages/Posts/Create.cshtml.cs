using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Dashboard.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly App.Data.AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public Post Post { get; set; }

        public CreateModel(
            App.Data.AppDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            this.Post.OwnerId = userId;
            this.Post.PublishedDate = DateTime.Now;
            _context.Posts.Add(Post);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
