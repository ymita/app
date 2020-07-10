using App.Data;
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
    }
}
