﻿using App.Models;
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

        public Post getPost(int id)
        {
            var post = this._appDbContext.Posts.Where(p => p.Id == id).FirstOrDefault();
            return post;
        }

        public List<Post> getAllPosts()
        {
            var posts = this._appDbContext.Posts.ToList();
            return posts;
        }
    }
}
