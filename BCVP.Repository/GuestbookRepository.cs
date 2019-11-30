using BCVP.IRepository;
using BCVP.IRepository.UnitOfWork;
using BCVP.Model.Models;
using BCVP.Repository.Base;

namespace BCVP.Repository
{
    public class GuestbookRepository : BaseRepository<Guestbook>, IGuestbookRepository
    {
        public GuestbookRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }



    }
}
