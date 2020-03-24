using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [RoutePrefix("api/TodoItems")]
    public class TodoItemsController : ApiController
    {
        private readonly TodoContext _context;

        public TodoItemsController()
        {
            _context = new TodoContext();
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<IHttpActionResult> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return Ok(ItemToDTO(todoItem));
        }

        [HttpGet]
        public IHttpActionResult GetTodoItems()
        {
            var result = _context.TodoItems
                    .Select(x => x)
                    .ToList();

            var resultDTO = result.Select(x => ItemToDTO(x)).ToList();

            return Ok(resultDTO);
        }

        [HttpGet]
        public IHttpActionResult GetTodoItem(long id)
        {
            var todoItem = _context.TodoItems.Find(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            var resultDTO = ItemToDTO(todoItem);

            return Ok(resultDTO);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) => new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };

        // PUT: api/TodoItems/5
        [HttpPut]
        public async Task<IHttpActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
