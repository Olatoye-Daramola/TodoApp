using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsRepo _repository;

        public TodoItemsController(ITodoItemsRepo repository)
        {
            _repository = repository;
        }
        
        // POST: api/v1/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(TodoItem todoItem)
        {
            return Created("", await _repository.CreateTodoItem(todoItem));
        }

        // GET: api/v1/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            return Ok(await _repository.GetAll());
        }

        // GET: api/v1/TodoItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(long id)
        {
            return Ok(await _repository.GetById(id));
        }

        // PUT: api/v1/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            return Ok(await _repository.UpdateTodoItem(id, todoItem));
        }

        // DELETE: api/v1/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            await _repository.DeleteTodoItem(id);
            return NoContent();
        }
    }
}