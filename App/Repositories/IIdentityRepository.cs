using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public interface IIdentityRepository
    {

        Task<string> getUserNameByIdAsync(string userId);

        Task saveProfilePictureAsync(string userId, Task<byte[]> picture);

        Task<byte[]> getProfilePicutreAsync(string userId);
    }
}
