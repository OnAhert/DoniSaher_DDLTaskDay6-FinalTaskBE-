using MediatR;
using System;
using System.Collections.Generic;

namespace Core.Features.Commands.UpdateTodo
{
    public class UpdateTodoCommand : IRequest<UpdateTodoResponse>
    {
        public Guid TodoId { get; set; }
        public string Day { get; set; }
        public DateTime TodayDate { get; set; }
        public string Note { get; set; }
        public List<UpdateTodoDetailCommand> TodoDetails { get; set; }
    }

    public class UpdateTodoDetailCommand
    {
        public Guid TodoDetailId { get; set; }
        public string Activity { get; set; }
        public string Category { get; set; }
        public string DetailNote { get; set; }
    }
}
