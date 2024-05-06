using AMNSystemsERP.DL.DB;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AMNSystemsERP.BL.Repositories.CommonRepositories
{
    public class ERPRepository<TEntity> where TEntity : class
    {
        internal ERPContext _context;
        internal DbSet<TEntity> dbSet;

        public ERPRepository(ERPContext context)
        {
            this.dbSet = context.Set<TEntity>();
            this._context = context;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null
        , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        , string includeProperties = ""
        , int count = 0)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await Task.FromResult(orderBy(query));
            }
            else
            {
                return await Task.FromResult(query);
            }
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).FirstOrDefaultAsync();
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).CountAsync();
        }

        public virtual void InsertSingle(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public long Max(Expression<Func<TEntity, long>> predicate)
        {
            return dbSet.Max(predicate);
        }
        public virtual void InsertList(List<TEntity> entities)
        {
            dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;

            // CreatedDate and CreatedBy log properties neglet
            _context.Entry(entityToUpdate)
                       .Properties
                       .Where(c => c.Metadata?.Name == "CreatedBy" || c.Metadata?.Name == "CreatedDate")
                       ?.ToList()
                       ?.ForEach(x => x.IsModified = false);
            //
        }

        public virtual void UpdateList(List<TEntity> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    dbSet.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
            }
        }

        public virtual void DeleteById(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (entityToDelete != null)
                DeleteByEntity(entityToDelete);
        }

        public virtual void DeleteByEntity(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            _context.Entry(entityToDelete).State = EntityState.Deleted;
        }

        public virtual void DeleteRangeByIds<T>(List<T> Ids)
        {
            if (Ids != null
                && Ids.Count > 0)
            {
                Ids
                    .Where(x => x != null)
                    .ToList()
                    .ForEach((id) =>
                    {
                        TEntity entityToDelete = dbSet.Find(id);
                        DeleteByEntity(entityToDelete);
                    });
            }
        }

        public virtual void DeleteRangeEntities(List<TEntity> EntitiesToDelete)
        {
            if (EntitiesToDelete != null && EntitiesToDelete.Count > 0)
            {
                EntitiesToDelete
                    .Where(x => x != null)
                    .ToList()
                    .ForEach((entity) =>
                    {
                        DeleteByEntity(entity);
                    });
            }
        }
    }
}
