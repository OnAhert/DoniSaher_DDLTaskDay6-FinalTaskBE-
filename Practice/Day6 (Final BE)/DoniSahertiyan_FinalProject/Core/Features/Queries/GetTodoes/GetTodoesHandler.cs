using MediatR;
using Persistence.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Queries.GetTodoes
{
    public class GetTodoesHandler : IRequestHandler<GetTodoesQuery, GetTodoesResponse>
    {
        private readonly ITodoRepository _todoRepository;

        public GetTodoesHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<GetTodoesResponse> Handle(GetTodoesQuery query, CancellationToken cancellationToken)
        {
            var todos = await _todoRepository.GetPagedAsync(query.PageNumber, query.PageSize);

            var todosWithDetails = new List<TodoWithDetails>();

            foreach (var todo in todos)
            {
                todosWithDetails.Add(new TodoWithDetails
                {
                    TodoId = todo.TodoId,
                    Day = todo.Day,
                    TodayDate = todo.TodayDate,
                    Note = todo.Note,
                    DetailCount = todo.DetailCount,
                    TodoDetails = todo.TodoDetails
                });
            }

            return new GetTodoesResponse
            {
                Todoes = todosWithDetails
            };
        }
    }
}
