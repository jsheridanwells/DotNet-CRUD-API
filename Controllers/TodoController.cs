using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DotNetCRUD_API.Models;
using System.Linq;

namespace DotNetCRUD_API.Controllers
{
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item 1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll() => _context.TodoItems.ToList();

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null) return BadRequest();
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);            
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            // if (item == null || item.Id != id) return BadRequest();
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound();
            todo.Done = item.Done;
            todo.Name = item.Name;
            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null) return NotFound();
            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();

        }
    }
}