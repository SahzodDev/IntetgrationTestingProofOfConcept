using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ToDoApp.Models;

namespace TodoAppDataLayerTesting
{
    public class EdgeCasesTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public EdgeCasesTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetTodoItemWithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            int invalidId = 1000;

            // Act
            var response = await client.GetAsync($"/api/todoitems/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostTodoItemAsync_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            var invalidTodoItem = new TodoItem
            {
                Name = null,
                IsComplete = false
            };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(invalidTodoItem), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/todoitems", jsonContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutTodoItem_With_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            var invalidTodoItem = new TodoItem
            {
                Id = 1000,
                Name = "Updated Todo Item",
                IsComplete = true
            };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(invalidTodoItem), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"/api/todoitems/{invalidTodoItem.Id}", jsonContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
