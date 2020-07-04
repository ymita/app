using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Areas.User.Pages
{
    public class EntryModel : PageModel
    {
        public void OnGet(string userName = null, int? id = null)
        {
        }
    }
}
