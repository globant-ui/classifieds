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
        private IDBRepository _dbRepository;
        private ClassifiedsUser _user;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _userRepo = new UserRepository<ClassifiedsUser>(_dbRepository);
            
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

        #endregion
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
    }
}
