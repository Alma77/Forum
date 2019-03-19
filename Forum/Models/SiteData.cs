using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;

namespace Forum.Models
{
    public class SiteData
    {
        private readonly ApplicationDbContext _context;

        public int GetNumUsers()
        {
            int numUsers = _context.Users.Count();

            return numUsers;
        }
    }

    public class User
    {
        public string Name { get; set; }
    }

}
