using BCVP.FrameWork.IRepository;
using BCVP.Repository.Base;
using BCVP.Model.Models;
using BCVP.IRepository.UnitOfWork;

namespace BCVP.Repository
{
    /// <summary>
    /// UserRoleRepository
    /// </summary>	
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
