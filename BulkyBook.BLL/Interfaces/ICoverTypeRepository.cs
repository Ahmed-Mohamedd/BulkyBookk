using BulkyBook.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.BLL.Interfaces
{
    public interface ICoverTypeRepository:IGenericRepository<CoverType>
    {
        void Update(CoverType coverType);

        void Save();

        
    }
}
