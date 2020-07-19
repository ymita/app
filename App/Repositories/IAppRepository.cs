using App.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Repositories
{
    public interface IAppRepository
    {
        Task<List<Post>> getPostsByUserAsync(string userName);

        Task<Post> getPost(int id, string userName = null);

        Task<List<Post>> getAllPosts(bool includesDraft = false);

        Task<List<PostTagCrossReference>> getPostsTagsReferencesAsync(string userId);

        Task<List<PostTagCrossReference>> getPostTagsReferencesByPostIdAsync(string userId, int postId);
    }
}