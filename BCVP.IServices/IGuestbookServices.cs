using BCVP.IServices.BASE;
using BCVP.Model.Models;
using System.Threading.Tasks;

namespace BCVP.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<bool> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();
    }
}
