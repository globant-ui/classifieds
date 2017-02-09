using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.Repository;

namespace Classifieds.UserServiceAPI.Tests
{
    [TestClass]
    public class UserRepositoryTest
    {
        #region Class Variables
        private IUserRepository<ClassifiedsUser> _userRepo;
        private ISubscriptionRepository<Subscription> _subscriptionRepo;
        private IDBRepository _dbRepository;
        private ClassifiedsUser _user;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _userRepo = new UserRepository<ClassifiedsUser>(_dbRepository);
            _subscriptionRepo = new SubscriptionRepository<Subscription>(_dbRepository);

        }
        #endregion

        #region Setup
        private ClassifiedsUser GetUserObject()
        {
            ClassifiedsUser user = new ClassifiedsUser
            {
                UserEmail = "ashish.kulkarni@globant.com",
                UserName = "Ashish Kulkarni"
            };
            return user;
        }

        private Subscription GetSubscription()
        {
            Subscription subscription = new Subscription
            {
                Email = "v.wadsamudrakar@globant.com",
                SubmittedDate = "08-02-2017 12:45:34.243"
            };
            return subscription;
        }

        #endregion

        #region UsersTestCase

        [TestMethod]
        public void RegisterUserTest_ValidInput()
        {
            //Arrange
            _user = GetUserObject();
            //Act
            var result = _userRepo.RegisterUser(_user);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RegisterUserTest_InValidInput_ThrowsException()
        {
            //Act
            _userRepo.RegisterUser(null);
        }

        #endregion

        #region AddSubscriptionTestCases

        /// <summary>
        /// test positive scenario for add data Object into the database
        /// </summary>
        [TestMethod]
        public void Repo_AddSubscriptionTest()
        {
            //Arrange
            var dataObject = GetSubscription();

            //Act
            var result = _subscriptionRepo.AddSubscription(dataObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Subscription));
        }

        /// <summary>
        /// test for adding empty Email Subscription object returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_AddSubscriptionTest_EmptyList_ThrowException()
        {
            var result = _subscriptionRepo.AddSubscription(null);
            Assert.IsNull(result, null);
        }

        #endregion AddSubscriptionTestCases        

        #region DeleteSubscriptionTest

        /// <summary>
        /// test positive scenario for Delete Subscription by Id
        /// </summary>
        [TestMethod]
        public void Repo_DeleteSubscriptionTest()
        {
            //Arranges
            var dataObject = GetSubscription();

            //Act
            var result = _subscriptionRepo.AddSubscription(dataObject);
            Assert.IsNotNull(result._id);
            _subscriptionRepo.DeleteSubscription(result._id);

            //Assert
            Assert.IsNull(null);
        }

        /// <summary>
        /// test for incorrect id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteSubscriptionTest_InvalidId()
        {
            _subscriptionRepo.DeleteSubscription("qwer");
            Assert.IsTrue(true);
        }


        /// <summary>
        /// test for null id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Repo_DeleteSubscriptionTest_NUllId()
        {
            _subscriptionRepo.DeleteSubscription(null);
            Assert.IsNull(true);
        }

        #endregion DeleteSubscriptionTest
    }
}
