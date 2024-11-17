namespace SCH.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
