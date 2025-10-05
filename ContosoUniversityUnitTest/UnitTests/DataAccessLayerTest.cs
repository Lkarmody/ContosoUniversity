using ContosoUniversity.Data;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoUniversityUnitTest.Utilities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ContosoUniversity.Models;

namespace ContosoUniversityUnitTest.Test.UnitTests
{
    public class DataAccessLayerTest
    {
        [Fact]
        public async Task GetStudentsAsync_MessagesAreReturned()
        {
            using (var db = new SchoolContext(ContosoUniversityUnitTest.Utilities.Utilities.TestDbContextOptions()))
            {
                // Arrange
                var expectedMessages = SchoolContext.GetSeedingMessages();
                await db.AddRangeAsync(expectedMessages);
                await db.SaveChangesAsync();

                // Act
                var result = await db.GetStudentsAsync();

                // Assert
                var actualMessages = Assert.IsAssignableFrom<List<Student>>(result);
                //Assert.Equal(
                 ///   expectedMessages.OrderBy(m => m.Id).Select(m => m.Text),
                    //actualMessages.OrderBy(m => m.Id).Select(m => m.Text));
            }
        }

        [Fact]
        public async Task AddStudentAsync_MessageIsAdded()
        {
            using (var db = new SchoolContext(ContosoUniversityUnitTest.Utilities.Utilities.TestDbContextOptions()))
            {
                // Arrange
                var recId = 10;
                var expectedMessage = new Student() { ID = recId };

                // Act
                await db.AddStudentAsync(expectedMessage);

                // Assert
                var actualMessage = await db.FindAsync<Student>(recId);
                Assert.Equal(expectedMessage, actualMessage);
            }
        }

    }
}
