using Core.Features.Queries.GetTodoes;
using MediatR;
using Persistence.DatabaseContext;
using Persistence.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using Core.Features.Commands.CreateTodo;
using Core.Features.Commands.DeleteTodo;
using Core.Features.Commands.UpdateTodo;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : BaseController
    {
        private readonly IMediator _mediator;

        public TableController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("v1/todo/all")]
        [Authorize(Roles = "Admin,User")]
        public async Task<GetTodoesResponse> GetTodoes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var query = new GetTodoesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var response = await _mediator.Send(query);
            return response;
        }
        [HttpPost("v1/todo")]
        [Authorize(Roles = "Admin,User")]
        public async Task<CreateTodoResponse> CreateTodo([FromBody] CreateTodoCommand command)
        {
            var response = await _mediator.Send(command);
            return response;
        }
        [HttpDelete("v1/todo/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<DeleteTodoResponse> DeleteTodo(Guid id)
        {
            var command = new DeleteTodoCommand
            {
                TodoId = id
            };
            var response = await _mediator.Send(command);
            return response;
        }
        [HttpPut("v1/todo")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateTodoAsync([FromBody] UpdateTodoCommand command)
        {
            var response = await _mediator.Send(command);
            if (response == null || response.TodoId == Guid.Empty)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}