using BCVP.IRepository;
using BCVP.IRepository.UnitOfWork;
using BCVP.Model.Models;
using BCVP.Repository.Base;

namespace BCVP.Repository
{
    public class BlogArticleRepository : BaseRepository<BlogArticle>, IBlogArticleRepository
    {
        public BlogArticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
