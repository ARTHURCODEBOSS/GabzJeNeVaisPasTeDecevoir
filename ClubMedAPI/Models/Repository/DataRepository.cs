using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClubMedAPI.Models.EntityFramework;

namespace ClubMedAPI.Models.Repository
{
    public class DataRepository<TEntity> : IDataRepository<TEntity> where TEntity : class
    {
        private readonly ClubMedContext _context;

        public DataRepository(ClubMedContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<ActionResult<TEntity>> GetByIdAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return new NotFoundResult();
            return entity;
        }

        public async Task<ActionResult<TEntity>> GetByStringAsync(string id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return new NotFoundResult();
            return entity;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entityToUpdate, TEntity entity)
        {
            _context.Entry(entityToUpdate).State = EntityState.Detached;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
