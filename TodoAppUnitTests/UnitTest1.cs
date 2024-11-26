using System.Runtime.CompilerServices;
using ToDoApp.Models;

namespace TodoAppUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void IsComplete_DefaultValue_ShouldBeTrue()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.IsComplete = true;

            // Act & Assert
            Assert.True(todoItem.IsComplete);
        }

        [Fact]
        public void Name_Value_IsNotNull()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.Name = "Complete ASP.NET Project Work";

            // Act & Assert
            Assert.NotEmpty(todoItem.Name);
        }

        [Fact]
        public void Name_Value_IsNull()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.Name = String.Empty;

            // Act & Assert
            Assert.Empty(todoItem.Name);
        }

        
    }
}