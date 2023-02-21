using TodoApi.Models;

namespace TodoApi.Repository;

public interface ITodoItemsRepo
{
    Task<TodoItemDTO> CreateTodoItem(TodoItem item);
    
    Task<IEnumerable<TodoItemDTO>> GetAll();
    Task<TodoItemDTO> GetById(long id);
    
    Task<TodoItemDTO> UpdateTodoItem(long id, TodoItem item);
    
    Task DeleteTodoItem(long id);
}