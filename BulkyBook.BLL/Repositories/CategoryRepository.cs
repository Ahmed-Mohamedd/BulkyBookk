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
    public class CategoryRepository : GenericRepository<Category> ,  ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository( ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Category category)
        {
            _dbContext.categories.Update(category);
        }
        //public int Add(Category category)
        //{
        //    _dbContext.categories.Add(category);
        //    return _dbContext.SaveChanges();
        //}

        //public int Delete(Category category)
        //{
        //    _dbContext.categories.Remove(category);
        //   return _dbContext.SaveChanges();
        //}

        //public IReadOnlyList<Category> GetAllCategories()
        //    =>  _dbContext.categories.ToList();


        //public Category GetById(int? id)
        //    => _dbContext.categories.Find(id);


        //public int Update(Category category)
        //{
        //    _dbContext.categories.Update(category);
        //    return _dbContext.SaveChanges();
        //}


    }
}
