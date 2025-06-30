using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected TournamentApiContext Context { get; }
    protected DbSet<T> DbSet { get; }

    public RepositoryBase(TournamentApiContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public void Create(T entity)
    {
        DbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public IQueryable<T> GetAll()
    {
        return DbSet;
    }

    public IQueryable<T> GetWithCondition(Expression<Func<T, bool>> condition, bool trackChanges = false)
    {
        return DbSet.Where(condition);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }
}
