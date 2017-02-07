using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.UserService.Repository;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.BusinessServices;
using Moq;

namespace Classifieds.UserServiceAPI.Tests
{
    [TestClass]
    public class UserServiceTest
    {
        #region Class Variables
        private Mock<IUserRepository<ClassifiedsUser>> _moqAppManager;
        private IUserService _service;
        string _returnString = string.Empty;
        private ClassifiedsUser _user;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IUserRepository<ClassifiedsUser>>();
            _service = new UserService.BusinessServices.UserService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsUsers()
        {
            _user = GetUserObject();
            _returnString = "200 OK";
        }

        private ClassifiedsUser GetUserObject()
        {
            ClassifiedsUser user = new ClassifiedsUser
            {
                _id = "7",
                UserEmail = "ashish.kulkarni@globant.com",
                UserName = "Ashish Kulkarni"
            };
            return user;
        }

        #endregion
        [TestMethod]
        public void RegisterUserTest()
        {
            SetUpClassifiedsUsers();
            //string returnString = "200 OK";
            _moqAppManager.Setup(x => x.RegisterUser(this._user)).Returns(_returnString);

            //Act
            var result = _service.RegisterUser(_user);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RegisterUserTest_ThrowsException()
        {
            Exception ex = new Exception();
            _moqAppManager.Setup(x => x.RegisterUser(It.IsAny<ClassifiedsUser>())).Throws(ex);
            _service.RegisterUser(null);
        }
    }
}
