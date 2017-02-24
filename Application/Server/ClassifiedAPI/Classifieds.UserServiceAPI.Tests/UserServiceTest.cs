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
        [ExpectedException(typeof(Exception))]
        public void RegisterUserTest_ThrowsException()
        {
            Exception ex = new Exception();
            _moqAppManager.Setup(x => x.RegisterUser(It.IsAny<ClassifiedsUser>())).Throws(ex);
            _service.RegisterUser(null);
        }

        #endregion RegisterUserTest

        #region AddSubscriptionTest

        /// <summary>
        /// test positive scenario for Add Subscription
        /// </summary>
        [TestMethod]
        public void AddSubscriptionTest()
        {
            //Arrange
            var lstObject = GetSubscription();
            _moqSubAppManager.Setup(x => x.AddSubscription(It.IsAny<Subscription>())).Returns(lstObject);

            //Act
            var result = _service.AddSubscription(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Subscription));
        }

        /// <summary>
        /// test for inserting empty Subscription object return null result
        /// </summary>
        [TestMethod]
        public void AddSubscriptionTest_EmptyList()
        {
            var result = _service.AddSubscription(null);
            Assert.IsNull(result, null);
        }

        #endregion AddSubscriptionTest

        #region DeleteSubscriptionTest

        /// <summary>
        /// test positive scenario for DeleteEmailSubscription
        /// </summary>
        [TestMethod]
        public void DeleteSubscriptionTest()
        {
            //Arrange
            var lstObject = GetSubscription();
            _moqSubAppManager.Setup(x => x.DeleteSubscription(It.IsAny<string>()));

            //Act
            _service.DeleteSubscription(lstObject._id);

            //Assert
            Assert.IsTrue(true);
            _moqSubAppManager.Verify(v => v.DeleteSubscription(lstObject._id), Times.Once());
        }

        /// <summary>
        /// test for null Exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubscriptionTest_InvalidId_ThrowException()
        {
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqSubAppManager.Setup(x => x.DeleteSubscription(null)).Throws(ex);
            _service.DeleteSubscription(null);
        }

        #endregion DeleteSubscriptionTest
    }
}
