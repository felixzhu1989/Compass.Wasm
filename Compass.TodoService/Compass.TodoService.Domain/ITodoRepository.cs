using Compass.TodoService.Domain.Entities;

namespace Compass.TodoService.Domain;

public interface ITodoRepository
{
    Task<IQueryable<Todo>> GetTodosAsync();
    Task<Todo?> GetTodoByIdAsync(Guid id);

    Task<IQueryable<Memo>> GetMemosAsync();
    Task<Memo?> GetMemoByIdAsync(Guid id);
}