using Persistence.Models;

namespace Persistence.Repositories;

public interface ITodoRepository : IGenericRepository<Todo>
{
    Task AddAsync(Todo todo);
    Task SaveChangesAsync();
    Task<Todo> GetByIdAsync(Guid id);
    Task<List<Todo>> GetPagedAsync(int pageNumber, int pageSize);
    Task BulkAddTodoDetailsAsync(List<TodoDetail> todoDetails);
    Task RemoveTodoDetailsByTodoIdAsync(Guid todoId);
    Task UpdateAsync(Todo todo);
}