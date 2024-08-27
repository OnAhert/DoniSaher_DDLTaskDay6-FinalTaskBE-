using MediatR;

namespace Core.Features.Commands.CreateTodo
{
    public class CreateTodoCommand : IRequest<CreateTodoResponse>
    {
        public string Day { get; set; }
        public DateTime TodayDate { get; set; }
        public string Note { get; set; }
        public List<CreateTodoDetailCommand> TodoDetails { get; set; }
    }

    public class CreateTodoDetailCommand
    {
        public string Activity { get; set; }
        public string Category { get; set; }
        public string DetailNote { get; set; }
    }
}
