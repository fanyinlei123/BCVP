using BCVP.IServices.BASE;
using BCVP.Model.Models;
using System.Threading.Tasks;

namespace BCVP.IServices
{	
	/// <summary>
	/// RoleServices
	/// </summary>	
    public interface IRoleServices :IBaseServices<Role>
	{
        Task<Role> SaveRole(string roleName);
        Task<string> GetRoleNameByRid(int rid);

    }
}
