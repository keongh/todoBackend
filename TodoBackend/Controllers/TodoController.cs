using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Models;
using TodoBackend.Services;

namespace TodoBackend.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly TodoService _todoService;

        public TodoController(TodoContext context, TodoService todoService)
        {
            _context = context;
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateNewTodo(Todo newTodo)
        {
            newTodo = _todoService.CreateTodo(newTodo.Task);
            _context.todos.Add(newTodo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodo), new {id = newTodo.Id}, newTodo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(long id)
        {
            var todo = await _context.todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos()
        {
            return await _context.todos.ToArrayAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(long id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(long id)
        {
            var todo = await _context.todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            _context.todos.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TodoExists(long id)
        {
            return _context.todos.Any(e => e.Id == id);
        }
    }
}