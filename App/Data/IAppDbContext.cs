﻿using App.Areas.User.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Data
{
    public interface IAppDbContext
    {
        DbSet<Post> Posts { get; set; }
    }
}
