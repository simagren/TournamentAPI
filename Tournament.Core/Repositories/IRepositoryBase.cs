﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Repositories;

public interface IRepositoryBase<T>
{
    IQueryable<T> GetAll(bool trackChanges);
    IQueryable<T> GetWithCondition(Expression<Func<T, bool>> condition, bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
