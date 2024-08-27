using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Persistence.Repositories;

namespace Core.Features.Commands.DeleteTodo
{
    public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand, DeleteTodoResponse>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IDistributedCache _cache;
        private const string CacheKeyPrefix = "Todo_";

        public DeleteTodoHandler(ITodoRepository todoRepository, IDistributedCache cache)
        {
            _todoRepository = todoRepository;
            _cache = cache;
        }

        public async Task<DeleteTodoResponse> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = await _todoRepository.GetByIdAsync(command.TodoId);

            if (todo == null)
            {
                return new DeleteTodoResponse
                {
                    Success = false,
                    Message = "Todo not found."
                };
            }
            await _todoRepository.RemoveTodoDetailsByTodoIdAsync(command.TodoId);

            _todoRepository.Remove(todo);
            await _todoRepository.SaveChangesAsync();

            string cacheKey = $"{CacheKeyPrefix}{command.TodoId}";
            try
            {
                await _cache.RemoveAsync(cacheKey, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis error: {ex.Message}");
            }

            return new DeleteTodoResponse
            {
                Success = true,
                Message = "Todo and its details deleted successfully."
            };
        }
    }
}
