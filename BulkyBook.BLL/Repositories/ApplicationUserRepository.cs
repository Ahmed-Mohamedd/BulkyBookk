using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Context;
using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser> , IApplicationUserRepository
	{
        private readonly ApplicationDbContext _dbContext;
        public ApplicationUserRepository( ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update( ApplicationUser applicationUser)
        {
            _dbContext.ApplicationUsers.Update(applicationUser);
        }
      


    }
}
