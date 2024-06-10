namespace ActivityFinder.Server.Models
{
    public interface IEntityToVmMapper<TEntity,TVm>
    {
        public TVm MapListToVm(IQueryable<TEntity> queryForPage, int totalCount);
    }
}
