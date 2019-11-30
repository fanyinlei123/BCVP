using BCVP.Repository.Base;
using BCVP.Model.Models;
using BCVP.IRepository;
using BCVP.IRepository.UnitOfWork;

namespace BCVP.Repository
{
    /// <summary>
    /// ModuleRepository
    /// </summary>	
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
