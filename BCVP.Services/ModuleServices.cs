using BCVP.Services.BASE;
using BCVP.Model.Models;
using BCVP.IRepository;
using BCVP.IServices;

namespace BCVP.Services
{	
	/// <summary>
	/// ModuleServices
	/// </summary>	
	public class ModuleServices : BaseServices<Module>, IModuleServices
    {
	
        IModuleRepository _dal;
        public ModuleServices(IModuleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
