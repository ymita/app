using App.Pages.User;
using App.Repositories;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AppTests.UnitTests.Pages.User
{
    public class IndexPageTests
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel()
        {
            //    var loggerMock = new Mock<ILogger<IndexModel>>();

            //var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
            //    var mockAppIdentityDbContext = new Mock<AppIdentityDbContext>(optionsBuilder.Options);
            //    var identityRepository = new Mock<IdentityRepository>(mockAppIdentityDbContext);
            var appRepository = new Mock<IAppRepository>();
            //Arrange
            var pageModel = new IndexModel(
                    appRepository.Object);
            //    //Act
            await pageModel.OnGetAsync("abcd");

            //    //Assert
            Assert.NotNull(pageModel.Posts);
            Assert.True(pageModel.Posts.Count >= 0);
        }
    }
}
