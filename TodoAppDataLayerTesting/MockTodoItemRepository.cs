using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace TodoAppDataLayerTesting
{
    public class MockTodoItemRepository : ITodoItemRepository
    {
        private List<TodoItem> _todoItems;
        public MockTodoItemRepository(DbContextOptions<TodoContext> options)
        {
            _todoItems = new List<TodoItem>();
            using(var context = new TodoContext(options)) 
            {
                context.Database.EnsureCreated();
            }
        }
        public Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem)
        {
            _todoItems.Add(todoItem);
            return Task.FromResult(todoItem);   
        }

        public Task<bool> DeleteTodoItemAsync(int id)
        {
            var todoItem = _todoItems.FirstOrDefault(ti => ti.Id == id);
            if (todoItem != null)
            {
                _todoItems.Remove(todoItem);
                return Task.FromResult(true);   
            }

            return Task.FromResult(false);
        }

        public Task<List<TodoItem>> GetAllTodoItemsAsync()
        {
            return Task.FromResult(this._todoItems);
        }

        public Task<TodoItem> GetTodoItemByIdAsync(int id)
        {
            var todoItem = _todoItems.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(todoItem);   
        }

        public Task<TodoItem> UpdateTodoItemAsync(TodoItem todoItem)
        {
            var existingTodoItem = _todoItems.FirstOrDefault(ti => ti.Id == todoItem.Id);
            if (existingTodoItem != null)
            {
                _todoItems.Remove(existingTodoItem);
                _todoItems.Add(todoItem);
            }

            return Task.FromResult(todoItem);
        }

        public void Reset()
        {
            _todoItems.Clear();
        }
    }
}
