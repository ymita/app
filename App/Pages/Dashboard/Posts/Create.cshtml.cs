using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Data;
using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Identity;

namespace App.Pages.Dashboard.Posts
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppRepository _appRepository;

        public CreateModel(UserManager<IdentityUser> userManager,
            IAppRepository appRepository)
        {
            this._userManager = userManager;
            this._appRepository = appRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var userId = _userManager.GetUserId(User);
            Post.OwnerId = userId;
            this.Post.UpdatedDate = this.Post.PublishedDate;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await this._appRepository.savePostAsync(Post);

            return RedirectToPage("./Index");
        }
    }
}
