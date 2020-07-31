using App.Models;
using App.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace App.Services.User
{
    public class UserService : IUserService
    {
        private readonly IAppRepository _appRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(IAppRepository appRepository,
            UserManager<IdentityUser> userManager)
        {
            this._appRepository = appRepository;
            this._userManager = userManager;
        }

        public async Task<IList<Post>> GetAllPostsAsync(bool includesDraft = false)
        {
            var posts = await this._appRepository.getAllPostsAsync(includesDraft);
            return posts;
        }

        public async Task<IList<string>> GetOwnersAsync(IList<Post> posts)
        {
            List<string> owners = new List<string>();
            for (int i = 0; i < posts.Count; i++)
            {
                var ownerId = posts[i].OwnerId;
                //var userName = await _identityRepository.getUserNameByIdAsync(ownerId);
                var user = await this._userManager.FindByIdAsync(ownerId);
                owners.Add(user.UserName);
            }
            return owners;
        }
    }
}
