using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public interface IIdentityRepository
    {
        IdentityUser getUserByName(string userName);

        string getUserNameById(string userId);
    }
}
