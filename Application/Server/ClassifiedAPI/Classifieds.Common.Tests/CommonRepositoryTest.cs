using Classifieds.Common.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.Common.Repositories;
using Classifieds.NLog.MongoDB;
using System.Net;
using System.Net.Http;

namespace Classifieds.Common.Tests
{
    [TestClass]
    public class CommonRepositoryTest
    {
        #region Class Variables
        private ICommonRepository _commonRepo;
        private ICommonDBRepository _dbRepository;
        private UserToken _userToken;
        private ILogger _logger;

        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new CommonDBRepository();
            _logger = new Logger(new NLog.MongoDB.Logger());
            _commonRepo = new CommonRepository(_dbRepository, _logger);

        }
        #endregion

        #region Setup
        private HttpRequestMessage GetRequest()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("AccessToken", "06a96321055948989a96876a7dad9317");
            request.Headers.Add("UserEmail", "santosh.kale@globant.com");
            return request;
        }

        #endregion
        [TestMethod]
        public void IsAuthenticatedTest_ValidInput()
        {
            //Arrange
            HttpRequestMessage request = GetRequest();

            //Act
            var result = _commonRepo.IsAuthenticated(request);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("200", result);
        }

        [TestMethod]
        public void IsAuthenticatedTest_InValidInput()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();

            //Act
            var result = _commonRepo.IsAuthenticated(request);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new HttpResponseMessage(HttpStatusCode.Conflict).ToString() + " Pls try after some time.", result);
        }

        [TestMethod]
        public void IsAuthenticatedTest_ThrowsException()
        {
            //Arrange
            HttpRequestMessage request = new HttpRequestMessage();

            //Act
            var result = _commonRepo.IsAuthenticated(request);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
