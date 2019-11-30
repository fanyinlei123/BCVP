using BCVP.IRepository;
using BCVP.Repository.Base;
using BCVP.Model.Models;
using BCVP.IRepository.UnitOfWork;

namespace BCVP.Repository
{
    /// <summary>
    /// RoleRepository
    /// </summary>	
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
