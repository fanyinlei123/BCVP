using BCVP.IRepository.Base;
using BCVP.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCVP.IRepository
{	
	/// <summary>
	/// IRoleModulePermissionRepository
	/// </summary>	
	public interface IRoleModulePermissionRepository : IBaseRepository<RoleModulePermission>//类名
    {
        Task<List<RoleModulePermission>> WithChildrenModel();
        Task<List<TestMuchTableResult>> QueryMuchTable();
    }
}
