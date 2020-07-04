using App.Areas.User.Models;
using System;
using System.Collections.Generic;

namespace App.Repositories
{
    public interface IAppRepository
    {
        List<Post> getPostsByUser(string userName);

        Post getPost(Guid postId);
    }
}