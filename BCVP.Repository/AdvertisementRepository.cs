using BCVP.IRepository;
using BCVP.IRepository.UnitOfWork;
using BCVP.Model.Models;
using BCVP.Repository.Base;

namespace BCVP.Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
