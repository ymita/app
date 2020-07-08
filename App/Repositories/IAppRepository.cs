using App.Models;
using System;
using System.Collections.Generic;

namespace App.Repositories
{
    public interface IAppRepository
    {
        List<Post> getPostsByUser(string userName);

        Post getPost(int id);
    }
}