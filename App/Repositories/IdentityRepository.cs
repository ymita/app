﻿using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace App.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        public AppIdentityDbContext _identityDbContext { get; set; }

        public IdentityRepository(AppIdentityDbContext identityDbContext)
        {
            this._identityDbContext = identityDbContext;
        }

        public async Task<string> getUserNameByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User name is not provided");
            }
            var user = await this._identityDbContext.Users.FindAsync(userId);
            return user.UserName;
        }

        public async Task saveProfilePictureAsync(string userId, Task<byte[]> picture)
        {
            var result = picture.Result;
            var profilePicture = this._identityDbContext.ProfilePictures.Where(x => x.UserId == userId).FirstOrDefault();
            if(profilePicture != null)
            {
                this._identityDbContext.ProfilePictures.Remove(profilePicture);
            }

            this._identityDbContext.ProfilePictures.Add(
                new ProfilePicture { UserId = userId, Picture = result }
            );
            
            await this._identityDbContext.SaveChangesAsync();

            return;
        }

        public async Task<byte[]> getProfilePicutreAsync(string userId)
        {
            var picture = await this._identityDbContext.ProfilePictures.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (picture == null)
            {
                return null;
            }
            return picture.Picture;
        }
    }
}
