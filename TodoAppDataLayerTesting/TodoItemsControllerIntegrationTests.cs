using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ToDoApp.Data;
using ToDoApp.Models;

namespace TodoAppDataLayerTesting
{
    public class TodoItemsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TodoItemsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetTodoItems_ReturnsListOfTodoItems()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
            var client = _factory.CreateClient();
            dbContext.TodoItems.RemoveRange(dbContext.TodoItems);
            await dbContext.SaveChangesAsync();
            dbContext.TodoItems.AddRange(new ToDoApp.Models.TodoItem { Name = "Test Todo 1", IsComplete = false });
            dbContext.TodoItems.AddRange(new ToDoApp.Models.TodoItem { Name = "Test Todo 2", IsComplete = true });
            await dbContext.SaveChangesAsync();

            // Act
            var response = await client.GetAsync("/api/todoitems");
            var responseContent = await response.Content.ReadAsStringAsync();
            var todoItems = JsonConvert.DeserializeObject<List<TodoItem>>(responseContent);

            // Assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            todoItems.Should().NotBeNullOrEmpty();
            todoItems.Count.Should().Be(2);
            todoItems.Should().Contain(ti => ti.Name == "Test Todo 1" && ti.IsComplete == false);
            todoItems.Should().Contain(ti => ti.Name == "Test Todo 2" && ti.IsComplete == true);
        }

        [Fact]
        public async Task GetTodoItemWithId_ReturnsSuccessStatusCode()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
            var client = _factory.CreateClient();
            var todoItem = new TodoItem { Name = "Test Todo", IsComplete=false };
            dbContext.TodoItems.AddRange(todoItem);
            await dbContext.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"/api/todoitems/{todoItem.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }
    }
}
