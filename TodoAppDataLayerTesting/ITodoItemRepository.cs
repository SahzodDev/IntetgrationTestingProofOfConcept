using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace TodoAppDataLayerTesting
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetAllTodoItemsAsync();
        Task<TodoItem> GetTodoItemByIdAsync(int id);
        Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem);
        Task<TodoItem> UpdateTodoItemAsync(TodoItem todoItem);
        Task<bool> DeleteTodoItemAsync(int id);
        void Reset();
    }
}
