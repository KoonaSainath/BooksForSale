using Sainath.E_Commerce.BooksForSale.DataAccess.BaseClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.DataAccess.DataAccessClasses.Admin
{
    public class CoverTypeData : BaseData
    {
        public CoverTypeData(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public IEnumerable<CoverType> GetAllCoverTypes()
        {
            return unitOfWork.CoverTypeRepository.GetAllRecords();
        }
        public void InsertCoverType(CoverType coverType)
        {
            unitOfWork.CoverTypeRepository.InsertRecord(coverType);
            unitOfWork.Save();
        }
        public CoverType GetCoverTypeById(int coverTypeId)
        {
            return unitOfWork.CoverTypeRepository.GetRecord(coverTypeId);
        }
        public void RemoveCoverType(CoverType coverType)
        {
            unitOfWork.CoverTypeRepository.RemoveRecord(coverType);
            unitOfWork.Save();
        }
        public void RemoveCoverTypes(IEnumerable<CoverType> coverTypes)
        {
            unitOfWork.CoverTypeRepository.RemoveRecords(coverTypes);
            unitOfWork.Save();
        }
        public CoverType UpdateCoverType(CoverType coverType)
        {
            unitOfWork.CoverTypeRepository.UpdateCoverType(coverType);
            unitOfWork.Save();
            return coverType;
        }
    }
}
