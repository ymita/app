using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        public AppIdentityDbContext _identityDbContext { get; set; }

        public IdentityRepository(AppIdentityDbContext identityDbContext)
        {
            this._identityDbContext = identityDbContext;
        }

        public IdentityUser getUserByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            var user = this._identityDbContext.Users.Where(x => x.UserName == userName).FirstOrDefault();
            return user;
        }

        public string getUserNameById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User name is not provided");
            }
            return this._identityDbContext.Users.Find(userId).UserName;
        }

        public async Task saveProfilePicture(string userId, Task<byte[]> picture)
        {
            var result = picture.Result;
            var profilePicture = this._identityDbContext.ProfilePictures.Where(x => x.UserId == userId).FirstOrDefault();
            if(profilePicture != null)
            {
                this._identityDbContext.ProfilePictures.Remove(profilePicture);
            }
            //if (profilePicture == null)
            //{
            this._identityDbContext.ProfilePictures.Add(
                new ProfilePicture { UserId = userId, Picture = result }
            );
            //}
            await this._identityDbContext.SaveChangesAsync();

            return;
        }

        public byte[] getProfilePicutre(string userId)
        {
            var picture = this._identityDbContext.ProfilePictures.Where(x => x.UserId == userId).FirstOrDefault();
            if (picture == null)
            {
                return null;
            }
            return picture.Picture;
        }
    }
}
