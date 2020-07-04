using App.Areas.User.Models;
using App.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class AppRepository : IAppRepository
    {
        public AppDbContext _appDbContext { get; set; }
        public IIdentityRepository _identityRepository { get; set; }


        public AppRepository(AppDbContext appDbContext, IIdentityRepository identityRepository)
        {
            this._appDbContext = appDbContext;
            this._identityRepository = identityRepository;
        }

        public List<Post> getPostsByUser(string userName)
        {
            List<Post> posts = null;

            var user = this._identityRepository.getUserByName(userName);
            
            if (user != null)
            {
                posts = this._appDbContext.Posts.Where(p => p.OwnerId == user.Id).ToList();
            }
            
            return posts;
        }
    }
}
