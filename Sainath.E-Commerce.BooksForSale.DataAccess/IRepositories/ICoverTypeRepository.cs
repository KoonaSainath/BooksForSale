using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void UpdateCoverType(CoverType coverType);
    }
}
