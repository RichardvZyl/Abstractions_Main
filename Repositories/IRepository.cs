namespace Abstractions.Repositories;

public interface IRepository<T> : ICommandRepository<T>, IQueryRepository<T> where T : class { }
