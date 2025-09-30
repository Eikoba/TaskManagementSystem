using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Infrastructure.Data;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class EfTaskRepository: ITaskRepository
    {
        private readonly TaskDbContext _context;

        public EfTaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<PartTaskResultDTO<TaskItem>> GetPartAsync(int pageSize, TaskFilterDto? filter)
        {
            var query = _context.Tasks.AsQueryable();
            var pageNumber = 1;
            if (filter != null)
            {
                if (filter.Status.HasValue)
                    query = query.Where(t => t.Status == filter.Status.Value);

                if (filter.Priority.HasValue)
                    query = query.Where(t => t.Priority == filter.Priority.Value);

                pageNumber = filter.PageNumber;
            }

            var totalCount = await query.CountAsync();
            
            var items = await query.AsNoTracking()
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            return new PartTaskResultDTO<TaskItem>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
