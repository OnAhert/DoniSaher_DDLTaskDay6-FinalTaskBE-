using Persistence.Models;

namespace Core.Features.Queries.GetTodoes
{
    public class GetTodoesResponse
    {
        public List<TodoWithDetails> Todoes { get; set; }
    }

    public class TodoWithDetails
    {
        public Guid TodoId { get; set; }
        public string Day { get; set; }
        public DateTime TodayDate { get; set; }
        public string Note { get; set; }
        public int DetailCount { get; set; }
        public List<TodoDetail> TodoDetails { get; set; }
    }
}
