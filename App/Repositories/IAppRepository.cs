using App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repositories
{
    public interface IAppRepository
    {
        Task<List<Post>> getPostsByUserAsync(string userName);

        Task<Post> getPost(int id, string userName = null);

        Task<List<Post>> getAllPosts(bool includesDraft = false);
    }
}