using Core.Features.Commands.UpdateTodo;
using MediatR;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Commands.UpdateTodo
{
    public class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, UpdateTodoResponse>
    {
        private readonly ITodoRepository _todoRepository;

        public UpdateTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<UpdateTodoResponse> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = await _todoRepository.GetByIdAsync(command.TodoId);
            if (todo == null)
            {
                return new UpdateTodoResponse
                {
                    TodoId = command.TodoId,
                    Day = null,
                    TodayDate = default(DateTime),
                    Note = null,
                    DetailCount = 0,
                    TodoDetails = new List<TodoDetailResponse>()
                };
            }

            todo.Day = command.Day;
            todo.TodayDate = command.TodayDate;
            todo.Note = command.Note;

            foreach (var detailCommand in command.TodoDetails)
            {
                if (detailCommand.Category != "Task" && detailCommand.Category != "DailyActivity")
                {
                    throw new ArgumentException("Invalid category. Allowed values are 'Task' and 'DailyActivity'.");
                }

                var existingDetail = todo.TodoDetails
                    .FirstOrDefault(d => d.TodoDetailId == detailCommand.TodoDetailId);

                if (existingDetail != null)
                {
                    existingDetail.Activity = detailCommand.Activity;
                    existingDetail.Category = detailCommand.Category;
                    existingDetail.DetailNote = detailCommand.DetailNote;
                }
                else
                {
                    todo.TodoDetails.Add(new TodoDetail
                    {
                        TodoDetailId = detailCommand.TodoDetailId,
                        TodoId = todo.TodoId,
                        Activity = detailCommand.Activity,
                        Category = detailCommand.Category,
                        DetailNote = detailCommand.DetailNote
                    });
                }
            }

            todo.DetailCount = todo.TodoDetails.Count;

            await _todoRepository.UpdateAsync(todo);

            var response = new UpdateTodoResponse
            {
                TodoId = todo.TodoId,
                Day = todo.Day,
                TodayDate = todo.TodayDate,
                Note = todo.Note,
                DetailCount = todo.DetailCount,
                TodoDetails = todo.TodoDetails.Select(detail => new TodoDetailResponse
                {
                    TodoDetailId = detail.TodoDetailId,
                    Activity = detail.Activity,
                    Category = detail.Category,
                    DetailNote = detail.DetailNote
                }).ToList()
            };

            return response;
        }
    }
}
