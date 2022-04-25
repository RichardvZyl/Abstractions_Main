using Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Abstractions.EntityFrameworkCore;

public class EFRepository<T> : Repository<T> where T : class
{
    public EFRepository(DbContext context) 
        : base(new EFCommandRepository<T>(context)
        , new EFQueryRepository<T>(context)) { }
}
