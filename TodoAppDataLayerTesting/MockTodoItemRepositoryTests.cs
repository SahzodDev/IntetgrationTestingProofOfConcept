﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using ToDoApp.Data;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using FluentAssertions;
using System.Security.Cryptography.X509Certificates;

namespace TodoAppDataLayerTesting
{
    public class MockTodoItemRepositoryTests
    {
        private DbContextOptions<TodoContext> _options;
        public MockTodoItemRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TodoContext>()
                .UseSqlite(CreateInMemoryDatabase()).Options;
            using (var context = new TodoContext(_options)) 
            {
                context.Database.EnsureCreated();
            }
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        private static DbContextOptions<TodoContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<TodoContext>()
                .UseSqlite(CreateInMemoryDatabase())
                .Options; 
        }


        [Fact]
        public async Task GetTodoItemByIdAsync_ReturnsTodoItem()
        {
            // Arrange
            DbContextOptions<TodoContext> options = CreateOptions();
            using (var context = new TodoContext(options))
            {
                var repository = new MockTodoItemRepository(options);
                var todoItem = new TodoItem { Name = "Test Todo Item" };
                await repository.CreateTodoItemAsync(todoItem);

                // Act
                var result = await repository.GetTodoItemByIdAsync(todoItem.Id);

                // Assert 
                Assert.Equal(todoItem.Id, result.Id);
                todoItem.Id.Should().Be(result.Id);
            }
            
        }

        [Fact]
        public async Task CreateAsync_Should_Create_TodoItem_In_Database()
        {
            // Arrange
            DbContextOptions<TodoContext> options = CreateOptions();
            using (var context = new TodoContext(options))
            {
                var repository = new MockTodoItemRepository(options);
                var todoItem = new TodoItem { Name = "Test Todo Item" };
                await repository.CreateTodoItemAsync(todoItem);

                // Act
                var result = await repository.GetTodoItemByIdAsync(todoItem.Id);

                // Assert 
                result.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Update_Should_Update_TodoItem_In_Database()
        {
            // Arrange
            DbContextOptions<TodoContext> options = CreateOptions();
            using (var context = new TodoContext(options))
            {
                var repository = new MockTodoItemRepository(options);
                var todoItem = new TodoItem { Name = "Test Todo Item" };
                await repository.CreateTodoItemAsync(todoItem);

                // Act
                todoItem.Name = "Updated Test Todo Item";
                await repository.UpdateTodoItemAsync(todoItem);
                var result = await repository.GetTodoItemByIdAsync(todoItem.Id);

                // Assert
                todoItem.Name.Should().Be(result.Name);

            }
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_TodoItem_From_Database()
        {
            // Arrange
            int todoItemId = 1;
            var options = CreateOptions();
            using (var context = new TodoContext(options))
            {
                var repository = new MockTodoItemRepository(options);
                await repository.CreateTodoItemAsync(new TodoItem { Id = todoItemId, Name = "Test Todo Item", IsComplete = false});
            }

            // Act
            using (var context = new TodoContext(options))
            {
                var repository = new MockTodoItemRepository(options);
                var todoItem = await repository.GetTodoItemByIdAsync(todoItemId);
                await repository.DeleteTodoItemAsync(todoItemId);
            }

            // Assert
            using (var context = new TodoContext(options))
            {
                var deletedTodoItem = await  context.TodoItems.FindAsync(todoItemId);
                deletedTodoItem.Should().BeNull();
            }
        }
    }
}
