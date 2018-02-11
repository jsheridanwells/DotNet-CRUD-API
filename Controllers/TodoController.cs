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
    }
}