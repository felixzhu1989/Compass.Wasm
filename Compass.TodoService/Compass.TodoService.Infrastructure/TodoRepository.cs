using Compass.TodoService.Domain;
using Compass.TodoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Compass.TodoService.Infrastructure;

public class TodoRepository:ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context)
    {
        _context = context;
    }
    public Task<IQueryable<Todo>> GetTodosAsync()
    {
        return Task.FromResult(_context.Todos.AsQueryable());
    }

    public Task<Todo?> GetTodoByIdAsync(Guid id)
    {
        return _context.Todos.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<IQueryable<Memo>> GetMemosAsync()
    {
        return Task.FromResult(_context.Memos.AsQueryable());
    }

    public Task<Memo?> GetMemoByIdAsync(Guid id)
    {
        return _context.Memos.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }
}