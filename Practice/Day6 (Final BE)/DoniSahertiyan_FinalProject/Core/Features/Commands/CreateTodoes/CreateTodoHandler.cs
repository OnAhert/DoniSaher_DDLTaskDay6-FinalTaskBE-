using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Persistence.Models;
using Persistence.Repositories;
using System.Text.Json;

namespace Core.Features.Commands.CreateTodo
{
    public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IDistributedCache _cache;
        private const string CacheKeyPrefix = "Todo_";
        private const int CacheTTLMinutes = 10;

        public CreateTodoHandler(ITodoRepository todoRepository, IDistributedCache cache)
        {
            _todoRepository = todoRepository;
            _cache = cache;
        }

        public async Task<CreateTodoResponse> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = new Todo
            {
                TodoId = Guid.NewGuid(),
                Day = command.Day,
                TodayDate = command.TodayDate,
                Note = command.Note,
                DetailCount = command.TodoDetails.Count
            };

            // Add Todo to repository
            await _todoRepository.AddAsync(todo);

            // Prepare TodoDetails for bulk insertion
            var todoDetails = command.TodoDetails
                .Where(detail => detail.Category == "Task" || detail.Category == "DailyActivity")
                .Select(detail => new TodoDetail
                {
                    TodoDetailId = Guid.NewGuid(),
                    TodoId = todo.TodoId,
                    Activity = detail.Activity,
                    Category = detail.Category,
                    DetailNote = detail.DetailNote
                }).ToList();

            if (todoDetails.Count != command.TodoDetails.Count)
            {
                throw new ArgumentException("Some categories were invalid. Allowed values are 'Task' and 'DailyActivity'.");
            }

            // Bulk add TodoDetails
            await _todoRepository.BulkAddTodoDetailsAsync(todoDetails);

            var response = new CreateTodoResponse
            {
                TodoId = todo.TodoId,
                Day = todo.Day,
                TodayDate = todo.TodayDate,
                Note = todo.Note,
                DetailCount = todo.DetailCount,
                TodoDetails = todoDetails.Select(detail => new TodoDetailResponse
                {
                    TodoDetailId = detail.TodoDetailId,
                    Activity = detail.Activity,
                    Category = detail.Category,
                    DetailNote = detail.DetailNote
                }).ToList()
            };

            string cacheKey = $"{CacheKeyPrefix}{response.TodoId}";
            try
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheTTLMinutes)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response), cacheOptions, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis error: {ex.Message}");
            }

            return response;
        }
    }
}
