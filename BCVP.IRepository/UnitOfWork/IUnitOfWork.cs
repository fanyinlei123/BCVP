using SqlSugar;

namespace BCVP.IRepository.UnitOfWork
{
    public interface IUnitOfWork
    {
        ISqlSugarClient GetDbClient();

        void BeginTran();

        void CommitTran();
        void RollbackTran();
    }
}
