﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Transactions;

namespace GameService.Entity.DBEntity
{
    public static class IQueryableExtensions
    {

        #region Public Methods

        public static int CountWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null)
        {
            using var scope = CreateTransaction();
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            int toReturn = query.Count();
            scope.Complete();
            return toReturn;
        }

        public static async Task<int> CountWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            using var scope = CreateTransactionAsync();
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            int toReturn = await query.CountAsync(cancellationToken);
            scope.Complete();
            return toReturn;
        }

        public static bool AnyWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null)
        {
            using var scope = CreateTransaction();
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            bool toReturn = query.Any();
            scope.Complete();
            return toReturn;
        }

        public static async Task<bool> AnyWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            using var scope = CreateTransactionAsync();
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            bool toReturn = await query.AnyAsync(cancellationToken);
            scope.Complete();
            return toReturn;
        }

        public static T? FirstOrDefaultWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null)
        {
            using var scope = CreateTransaction();
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            T? result = query.FirstOrDefault();
            scope.Complete();
            return result;
        }

        public static async Task<T?> FirstOrDefaultWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            using var scope = CreateTransactionAsync();
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            T? result = await query.FirstOrDefaultAsync(cancellationToken);
            scope.Complete();
            return result;
        }

        public static List<T> ToListWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null)
        {
            List<T> result = default!;
            using (var scope = CreateTransaction())
            {
                if (expression is not null)
                {
                    query = query.Where(expression);
                }
                result = query.ToList();
                scope.Complete();
            }
            return result;
        }

        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            List<T> result = default!;
            using (var scope = CreateTransactionAsync())
            {
                if (expression is not null)
                {
                    query = query.Where(expression);
                }
                result = await query.ToListAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static T[] ToArrayWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null)
        {
            T[] result = default!;
            using (var scope = CreateTransaction())
            {
                if (expression is not null)
                {
                    query = query.Where(expression);
                }
                result = query.ToArray();
                scope.Complete();
            }
            return result;
        }

        public static async Task<T[]> ToArrayWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            T[] result = default!;
            using (var scope = CreateTransactionAsync())
            {
                if (expression is not null)
                {
                    query = query.Where(expression);
                }
                result = await query.ToArrayAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        #endregion

        #region Private Methods

        private static TransactionScope CreateTransactionAsync()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
        }

        private static TransactionScope CreateTransaction()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                });
        }

        #endregion

    }
}
