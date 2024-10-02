using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly LicentaContext _context;

        public BaseRepository(LicentaContext context)
        {
            _context = context;
        }

        public async Task<Result<T>> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return Result<T>.Success(entity);
        }

        public async Task<Result<T>> DeleteAsync(Guid id)
        {
            var result = await FindByIdAsync(id);
            if (result.IsSuccess)
            {
                _context.Set<T>().Remove(result.Value);
                await _context.SaveChangesAsync();
                return Result<T>.Success(result.Value);
            }

            return Result<T>.Failure($"Entity with id {id} not found");
        }

        public virtual async Task<Result<T>> FindByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return Result<T>.Failure($"Entity with id {id} not found.");
            }

            return Result<T>.Success(entity);
        }

        public async Task<Result<IReadOnlyList<T>>> GetAllAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return Result<IReadOnlyList<T>>.Success(entities);
        }

        public async Task<Result<IReadOnlyList<T>>> GetPagedResponseAsync(int page, int size)
        {
            var result = await _context.Set<T>().Skip(page).Take(size).AsNoTracking().ToListAsync();
            return Result<IReadOnlyList<T>>.Success(result);
        }

        public async Task<Result<T>> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result<T>.Success(entity);
        }
    }
}
