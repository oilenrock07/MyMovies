using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public void Commit()
        {
            _databaseFactory.GetContext().SaveChanges();
        }
    }
}
