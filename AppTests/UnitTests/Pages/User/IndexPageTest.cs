using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using App.Pages.User;
using Moq;
using App.Repositories;
using App.Models;

namespace AppTests.UnitTests.Pages.User
{
    public class IndexPageTest
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel()
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

            string userName = "user1";
            mockAppRepository.Setup(x => x.getPostsByUserAsync(userName)).Returns(Task.FromResult(posts));

            //Act
            var pageModel = new IndexModel(mockAppRepository.Object);
            await pageModel.OnGetAsync(userName);

            //Assert
            Assert.NotNull(pageModel.UserName);
            Assert.NotNull(pageModel.Posts);
        }
    }
}
