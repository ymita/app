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
    public class DetailsModel : PageModel
    {
        private readonly IAppRepository _appRepository;

        public DetailsModel(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }

        public Post Post { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = await this._appRepository.getPostAsync(id.Value);

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
