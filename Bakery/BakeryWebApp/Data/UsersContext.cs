﻿using BakeryWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Data
{
    public class UsersContext : IdentityDbContext<User>
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
