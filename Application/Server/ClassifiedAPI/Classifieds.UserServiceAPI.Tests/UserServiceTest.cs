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
        private Mock<ISubscriptionRepository<Subscription>> _moqSubAppManager;
        private IUserService _service;
        string _returnString = string.Empty;
        private ClassifiedsUser _user;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IUserRepository<ClassifiedsUser>>();
            _moqSubAppManager = new Mock<ISubscriptionRepository<Subscription>>();
            _service = new UserService.BusinessServices.UserService(_moqAppManager.Object, _moqSubAppManager.Object);
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

        private Subscription GetSubscription()
        {
            Subscription subscription = new Subscription
            {
                UserEmail = "v.wadsamudrakar@globant.com",
                SubmittedDate = new DateTime(2018, 02, 03)
            };
            return subscription;
        }

        #endregion

        #region RegisterUserTest
        [TestMethod]
        public void RegisterUserTest()
        {
            SetUpClassifiedsUsers();
            _moqAppManager.Setup(x => x.RegisterUser(this._user)).Returns(_returnString);

            //Act
            var result = _service.RegisterUser(_user);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RegisterUserTest_ThrowsException()
        {
            _service.RegisterUser(null);
        }

        #endregion RegisterUserTest

    }
}
