using App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services.User
{
    public interface IUserService
    {
        Task<IList<Post>> GetAllPostsAsync(bool includesDraft = false);
        Task<IList<string>> GetOwnersAsync(IList<Post> posts);
    }
}