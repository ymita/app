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

        public UserService(IAppRepository appRepository)
        {
            this._appRepository = appRepository;
        }

        public async Task<IList<Post>> GetAllPostsAsync(bool includesDraft = false)
        {
            var posts = await this._appRepository.getAllPostsAsync(includesDraft);
            return posts;
        }
    }
}
