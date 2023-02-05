using Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Admin;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.BusinessDomain.BusinessDomainClasses.Admin
{
    public class CoverTypeDomain
    {
        private readonly CoverTypeData coverTypeData;
        public CoverTypeDomain(IUnitOfWork unitOfWork)
        {
            coverTypeData = new CoverTypeData(unitOfWork);
        }
        public IEnumerable<CoverType> GetAllCoverTypes()
        {
            IEnumerable<CoverType> coverTypes = coverTypeData.GetAllCoverTypes(); ;
            return coverTypes;
        }
        public CoverType InsertCoverType(CoverType coverType)
        {
            coverTypeData.InsertCoverType(coverType);
            return coverType;
        }
        public CoverType GetCoverType(int coverTypeId)
        {
            CoverType coverType = coverTypeData.GetCoverTypeById(coverTypeId);
            return coverType;
        }
        public void RemoveCoverType(CoverType coverType)
        {
            coverTypeData.RemoveCoverType(coverType);
        }
        public void RemoveCoverTypes(IEnumerable<CoverType> coverTypes)
        {
            coverTypeData.RemoveCoverTypes(coverTypes);
        }
        public CoverType UpdateCoverType(CoverType coverType)
        {
            CoverType updatedCoverType = coverTypeData.UpdateCoverType(coverType);
            return updatedCoverType;
        }
    }
}
