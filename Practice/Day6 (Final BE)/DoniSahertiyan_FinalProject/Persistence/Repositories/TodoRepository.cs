using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;
using Persistence.Models;

namespace Persistence.Repositories;

public class TodoRepository : GenericRepository<Todo>, ITodoRepository
{
    private readonly TableContext _context;

    public TodoRepository(TableContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Todo> GetByIdAsync(Guid id)
    {
        return await _context.Todoes.FindAsync(id);
    }
    public async Task AddAsync(Todo todo)
    {
        await _context.Todoes.AddAsync(todo);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<Todo>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Set<Todo>()
            .Include(todo => todo.TodoDetails)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task BulkAddTodoDetailsAsync(List<TodoDetail> todoDetails)
    {
        await _context.TodoDetails.AddRangeAsync(todoDetails);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveTodoDetailsByTodoIdAsync(Guid todoId)
    {
        var todoDetails = await _context.TodoDetails
            .Where(td => td.TodoId == todoId)
            .ToListAsync();

        _context.TodoDetails.RemoveRange(todoDetails);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Todo todo)
    {
        _context.Todoes.Update(todo);
        await _context.SaveChangesAsync();
    }
}