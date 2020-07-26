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

        Task<Post> getPostAsync(int id, string userName = null);

        Task<List<Post>> getAllPostsAsync(bool includesDraft = false);
        Task<List<Tag>> getTagsAsync(int postId);
        Task<List<Tag>> getAllTagsAsync();
        Task<List<PostTagCrossReference>> getPostsTagsReferencesAsync(string userId);

        Task<List<PostTagCrossReference>> getPostTagsReferencesByPostIdAsync(string userId, int postId);

        Task<List<Post>> getPostsByTagAsync(string tag);

        Task savePostAsync(Post post);
    }
}