using System;
using System.Threading.Tasks;
using Xunit;
using App.Pages;
using App.Repositories;
using Moq;
using Microsoft.Extensions.Logging;
using App.Data;
using Microsoft.EntityFrameworkCore;

namespace AppTests.UnitTests.Pages
{
    public class IndexPageTests
    {
        //[Fact]
        //public async Task OnGetAsync_PopulatesThePageModel()
        //{
        //    var loggerMock = new Mock<ILogger<IndexModel>>();

        //    var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
        //    var mockAppIdentityDbContext = new Mock<AppIdentityDbContext>(optionsBuilder.Options);
        //    var identityRepository = new Mock<IdentityRepository>(mockAppIdentityDbContext);
        //    var appRepository = new Mock<AppRepository>();
        //    //Arrange
        //    var pageModel = new IndexModel(
        //            loggerMock.Object,
        //            identityRepository.Object,
        //            appRepository.Object);
        //    //Act
        //    await pageModel.OnGet();

        //    //Assert
        //    Assert.NotNull(pageModel.Posts);
        //    Assert.True(pageModel.Posts.Count >= 0);
        //    Assert.NotNull(pageModel.Owners);
        //    Assert.True(pageModel.Owners.Count >= 0);
        //}
    }
}
