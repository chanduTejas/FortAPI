using AutoFixture;
using FluentAssertions;
using FortCode.Model;
using FortCode.Model.Request;
using FortCode.Repository.Interfaces;
using FortCode.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestFortApi
{
    public class FortApiUnitTest
    {
        #region AddUserAsync
        [Fact]
        public async Task AddUserAsync_FailedResult()
        {
            // Fixture are used to create a fixed environment in which tests are run so that results are repeatable.
            var fixture = new Fixture();
            //Mock object that will bypass the checking function
            var repoMock = new Mock<IFortRepository>();
            // This code is used to generate a random user details which all the properties which are there in the Class AddUserRequest.
            var addUserRequest = fixture.Create<AddUserRequest>();
            //repoMock.Setup is used to get the reponse from the AddUserAsync without calling to a DB (FortRepository), in this ReturnsAsync is the return value from this method.
            repoMock.Setup(c => c.AddUserAsync(addUserRequest)).ReturnsAsync(0);
            var serviceObject = new FortService(repoMock.Object);
            //This is used for calling AddUserAsync method in FortService class but here we'll get the response what we've intiliaze in repoMock.Setup
            var actualResult = await serviceObject.AddUserAsync(addUserRequest);
            //It will going to compare the values
            Assert.Equal(0, actualResult);
        }
        [Fact]
        public async Task AddUserAsync_ExpectedResult()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            var addUserRequest = fixture.Create<AddUserRequest>();
            repoMock.Setup(c => c.AddUserAsync(addUserRequest)).ReturnsAsync(1);
            var serviceObject = new FortService(repoMock.Object);
            var actualResult = await serviceObject.AddUserAsync(addUserRequest);
            Assert.Equal(1, actualResult);
        }
        [Fact]
        public async Task AddUserAsync_WhenEmailIsNull_ThrowsException()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            var addUserRequest = fixture.Build<AddUserRequest>()
                                        .Without(c => c.Email)
                                        .Create();
            var serviceObject = new FortService(repoMock.Object);
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => serviceObject.AddUserAsync(addUserRequest));
            Assert.Equal(nameof(addUserRequest.Email), exception.ParamName);
            repoMock.VerifyNoOtherCalls();
        }
        #endregion

        #region GetAllCountry
        [Fact]
        public async Task GetAllCountryUserAsync_FailedResult()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            var listCountry = fixture.CreateMany<Country>().ToList();
            repoMock.Setup(c => c.GetAllCountryByUserAsync(1)).ReturnsAsync(listCountry);
            var serviceObject = new FortService(repoMock.Object);
            var actualResult = await serviceObject.GetAllCountryByUserAsync(2);
            listCountry.Should().NotBeEquivalentTo("");
        }
        [Fact]
        public async Task GetAllCountryUserAsync_ExpectedResult()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            var listCountry = fixture.CreateMany<Country>().ToList();
            repoMock.Setup(c => c.GetAllCountryByUserAsync(1)).ReturnsAsync(listCountry);
            var serviceObject = new FortService(repoMock.Object);
            var actualResult = await serviceObject.GetAllCountryByUserAsync(1);
            listCountry.Should().BeEquivalentTo(actualResult);
        }
        #endregion

        #region DeleteCountry
        [Fact]
        public async Task DeleteCountryAsync_ExpectedResult()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            repoMock.Setup(r => r.DeleteCountryAsync(0)).ReturnsAsync(1);
            var serviceObject = new FortService(repoMock.Object);
            var actualResult = await serviceObject.DeleteCountryAsync(0);
            Assert.Equal(1, actualResult);
        }
        [Fact]
        public async Task DeleteCountryAsync_FailureResult()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            repoMock.Setup(r => r.DeleteCountryAsync(0)).ReturnsAsync(1);
            var serviceObject = new FortService(repoMock.Object);
            var actualResult = await serviceObject.DeleteCountryAsync(0);
            Assert.Equal(0, actualResult);
        }
        #endregion

        #region AddCountryAsync
        [Fact]
        public async Task AddCountryAsync_FailedResult()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            var addCountryRequest = fixture.CreateMany<AddCountryRequest>();
            var userId = fixture.Create<int>();
            repoMock.Setup(c => c.AddCountryAsync(addCountryRequest.ToList(), userId)).ReturnsAsync(0);
            var serviceObject = new FortService(repoMock.Object);
            var actualResult = await serviceObject.AddCountryAsync(addCountryRequest.ToList(), 1);
            Assert.Equal(0, actualResult);
        }
        [Fact]
        public async Task AddCountryAsync_ExpectedResult()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            var addCountryRequest = fixture.CreateMany<AddCountryRequest>();
            var userId = fixture.Create<int>();
            repoMock.Setup(c => c.AddCountryAsync(addCountryRequest.ToList(), userId)).ReturnsAsync(1);
            var serviceObject = new FortService(repoMock.Object);
            var actualResult = await serviceObject.AddCountryAsync(addCountryRequest.ToList(), 1);
            Assert.Equal(1, actualResult);
        }
        [Fact]
        public async Task AddCountryAsync_WhenCountryIsNull_ThrowsException()
        {
            var fixture = new Fixture();
            var repoMock = new Mock<IFortRepository>();
            var addCountryRequest = new List<AddCountryRequest>();
            var userId = fixture.Create<int>();
            var serviceObject = new FortService(repoMock.Object);
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => serviceObject.AddCountryAsync(addCountryRequest, userId));
            Assert.Equal("Empty Data", exception.ParamName);
            repoMock.VerifyNoOtherCalls();
        }
        #endregion  
    }
}
