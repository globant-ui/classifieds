using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private ClassifiedsUser _user;
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _userRepo = new UserRepository(_dbRepository);
            _user = GetUserObject();
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

        #endregion
        [TestMethod]
        public void RegisterUserTest()
        {
            //Act
            var result = _userRepo.RegisterUser(_user);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
