using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Classifieds.UserService.BusinessEntities;
using Classifieds.UserService.Repository;

namespace Classifieds.UserServiceAPI.Tests
{
    [TestClass]
    public class UserRepositoryTest
    {
        #region Class Variables
        private IUserRepository _userRepo;
        private IDBRepository _dbRepository;
        private ClassifiedsUser user;
        private UserToken token;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _userRepo = new UserRepository(_dbRepository);
            user = GetUserObject();
            token = GetUserToken();
        }
        #endregion

        #region Setup
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

        private UserToken GetUserToken()
        {
            UserToken token = new UserToken
            {
                //_id = 5.ToString(),
                AccessToken = Guid.NewGuid().ToString(),
                UserEmail = "ashish.kulkarni@globant.com"
            };
            return token;
        }
        #endregion
        [TestMethod]
        public void RegisterUserTest()
        {
            //Act
            var result = _userRepo.RegisterUser(user);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SaveTokenTest()
        {
            //Act
            var result = _userRepo.SaveToken(token);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserToken));
        }
    }
}
