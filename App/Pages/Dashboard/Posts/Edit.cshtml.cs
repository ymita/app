using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using App.Repositories;

namespace App.Pages.Dashboard.Posts
{
    public class EditModel : PageModel
    {
        private readonly App.Data.AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppRepository _appRepository;

        public EditModel(App.Data.AppDbContext context,
            UserManager<IdentityUser> userManager,
            IAppRepository appRepository)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<IActionResult> OnPostNewAsync(int? id)
        {
            if (!id.HasValue && id != -1)
            {
                return RedirectToPage("./Index");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Post オブジェクトを保存

            //これから編集する Post オブジェクトを用意
            Post = new Post();
            var userId = _userManager.GetUserId(User);
            Post.OwnerId = userId;
            Post.PublishedDate = DateTime.Now;
            Post.IsDraft = true;

            try
            {
                await _context.Posts.AddAsync(Post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // ページ遷移
            return Page();
        }
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            Post.OwnerId = userId;

            var isDraft = Request.Form["draft"].Count == 1 ? true : false;
            Post.IsDraft = isDraft;

            _context.Attach(Post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
