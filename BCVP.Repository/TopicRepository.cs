using BCVP.IRepository;
using BCVP.IRepository.UnitOfWork;
using BCVP.Model.Models;
using BCVP.Repository.Base;

namespace BCVP.Repository
{
    public class TopicRepository : BaseRepository<Topic>, ITopicRepository
    {
        public TopicRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
