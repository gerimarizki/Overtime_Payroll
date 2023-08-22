namespace Overtime_Payroll.Contracts;
    public interface IGeneralRepository<TEntity>
    {
        ICollection<TEntity> GetAll();
        TEntity? GetByGuid(Guid guid);
        TEntity? Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        bool IsExist(Guid guid);
        void Clear();

}
