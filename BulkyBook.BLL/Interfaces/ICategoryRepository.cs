using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public  interface ICategoryRepository:IGenericRepository<Category>
    {
        //IReadOnlyList<Category> GetAllCategories();

        //Category GetById(int? id);

        //int Add(Category category);
        //int Update(Category category);

        //int Delete(Category category);

        void Update(Category category);

        

        void Save();

    }
}
