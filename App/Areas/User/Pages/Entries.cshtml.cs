using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.User.Models;
using App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Areas.User.Pages
{
    public class EntriesModel : PageModel
    {
        public AppDbContext _dbContext { get; set; }
        public AppIdentityDbContext _identityDbContext { get; set; }

        public List<Post> Posts { get; set; }

        public EntriesModel(AppDbContext dbContext,
            AppIdentityDbContext identityDbContext)
        {
            this._dbContext = dbContext;
            this._identityDbContext = identityDbContext;
            //var users = identityDbContext.Users.ToList();
            ////var user = dbContext.Users.First();
            ////var userId = user.Id;
            //var posts = dbContext.Posts.ToList(); 
        }

        public void OnGet(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return;
            }

            var user = this._identityDbContext.Users.Where(x => x.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return;
            }
            this.Posts = this._dbContext.Posts.Where(p => p.OwnerId == user.Id).ToList();
        }
    }
}
