using App.Models;
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
        public IAppDbContext _appDbContext { get; set; }
        public IIdentityRepository _identityRepository { get; set; }


        public AppRepository(IAppDbContext appDbContext, IIdentityRepository identityRepository)
        {
            this._appDbContext = appDbContext;
            this._identityRepository = identityRepository;
        }

        //public async Task<List<Post>> GetPosts(string userName)
        //{
        //    return await this._appDbContext.Posts.ToListAsync();
        //}

        public async Task<List<Post>> getPostsByUserAsync(string userName)
        {
            List<Post> posts = null;

            var user = this._identityRepository.getUserByName(userName);
            
            if (user != null)
            {
                posts = await this._appDbContext.Posts.Where(p => p.OwnerId == user.Id).ToListAsync();
            }
            
            return posts;
        }

        public async Task<Post> getPost(int id)
        {
            var post = await this._appDbContext.Posts.Where(p => p.Id == id).FirstOrDefaultAsync();
            return post;
        }

        public async Task<List<Post>> getAllPosts()
        {
            var posts = await this._appDbContext.Posts.ToListAsync();
            return posts;
        }
    }
}
