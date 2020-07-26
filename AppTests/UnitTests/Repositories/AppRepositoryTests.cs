using App.Models;
using App.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppTests.UnitTests.Repositories
{
    public class AppRepositoryTests
    {
        [Fact]
        public async Task getAllPosts()
        {
            //Arrange
            var mockAppRepository = new Mock<IAppRepository>();

            List<Post> posts = new List<Post>()
            {
                new Post{
                    Id = 1, Title = "My first post", Description = "Body", OwnerId = "zzz", PublishedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDraft = false
                },
                new Post{
                    Id = 2, Title = "My second post", Description = "Body", OwnerId = "zzz", PublishedDate = DateTime.Now, UpdatedDate = DateTime.Now, IsDraft = false
                },
            };

            mockAppRepository.Setup(x => x.getAllPosts(false)).Returns(Task.FromResult(posts));

            //Act
            var fetchedPosts = await mockAppRepository.Object.getAllPosts(false);

            //Assert
            Assert.NotNull(fetchedPosts);
            Assert.True(fetchedPosts.Count >= 0);
        }
    }
}
