using MediatR;

namespace Core.Features.Queries.GetTodoes
{
    public class GetTodoesQuery : IRequest<GetTodoesResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
