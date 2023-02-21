using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;

namespace TodoApi.Repository;

public class TodoItemRepo : ITodoItemsRepo 
{
    private readonly AppDbContext _dbContext;

    public TodoItemRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TodoItemDTO> CreateTodoItem(TodoItem item)
    {
        await _dbContext.TodoItems.AddAsync(item);
        var saveChangesAsyncResponse = await _dbContext.SaveChangesAsync();
        
        if (saveChangesAsyncResponse != 1) throw new InvalidOperationException("TodoItem creation failed");
        var createdTodoItem = await _dbContext.TodoItems.LastAsync();
        return ItemToDto(createdTodoItem);
    }

    public async Task<IEnumerable<TodoItemDTO>> GetAll()
    {
        List<TodoItemDTO> result = new List<TodoItemDTO>();
        
        var foundTodoItems = await _dbContext.TodoItems.ToListAsync();
        if (!foundTodoItems.IsNullOrEmpty()) foundTodoItems.ForEach(item => result.Add(ItemToDto(item)));
        else throw new ApplicationException("Db is empty");
        
        return result;
    }

    public async Task<TodoItemDTO> GetById(long id)
    {
        var todoItem = await FindTodoItem(id);
        return todoItem != null
            ? ItemToDto(todoItem)
            : throw new ApplicationException($"TodoItem with id {id} not found");
    }

    public async Task<TodoItemDTO> UpdateTodoItem(long id, TodoItem item)
    {
        var todoItem = await FindTodoItem(id);
        
        var updateState = todoItem != null
            ? _dbContext.Entry(item).State = EntityState.Modified
            : throw new ApplicationException($"TodoItem with id {id} not found");
        await _dbContext.SaveChangesAsync();
        
        return ItemToDto(item);
    }

    public async Task DeleteTodoItem(long id)
    {
        var todoItem = await FindTodoItem(id);
        if (todoItem != null) _dbContext.Remove(todoItem);
        else throw new ApplicationException($"TodoItem with id {id} not found");
    }
    
    
    
    // HELPER FUNCTIONS
    
    private async Task<TodoItem?> FindTodoItem(long id)
    {
        return await _dbContext.TodoItems.FindAsync(id);
    }
    
    private static TodoItemDTO ItemToDto(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
}