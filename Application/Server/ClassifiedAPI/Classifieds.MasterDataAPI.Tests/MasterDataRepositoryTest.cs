#region using
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Classifieds.MastersData.Repository;
using Classifieds.MastersData.BusinessEntities;
using System.Collections.Generic;
#endregion

namespace Classifieds.MastersData.Repository.Test
{
    [TestClass]
    public class MasterDataRepositoryTest
    {
        #region Class Variables
        private IMasterDataRepository _masterDataRepo;
        private IDBRepository _dbRepository;
        private readonly List<Category> classifiedCategory = new List<Category>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _masterDataRepo = new MasterDataRepository(_dbRepository);

        }
        #endregion

        #region Setup
        private void SetUpClassifiedsCategory()
        {
            var lstMasterData = GetCategoryObject();
            classifiedCategory.Add(lstMasterData);
        }

        private Category GetCategoryObject()
        {
            Category dataObject = new Category
            {
                ListingCategory = "Housing",
                SubCategory = new String[] { "Test1", "Test2", "Test3" },
                Image = "Automobile.png"
            };
            return dataObject;
        }
        #endregion

        #region Unit Test Cases
        [TestMethod]
        public void Repo_GetAllCategoryTest()
        {
            // Arrange
            SetUpClassifiedsCategory();

            //Act
            var result = _masterDataRepo.GetAllCategory();

            //Assert
            Assert.IsNotNull(result[0]);
        }

        [TestMethod]
        public void Repo_AddCategoryTest()
        {
            //Arrange
            Category dataObject = GetCategoryObject();

            //Act
            var result = _masterDataRepo.AddCategory(dataObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Category));
        }

        [TestMethod]
        public void Repo_DeleteMasterDataTest()
        {
            //Arrange
            Category dataObject = new Category
            {
                ListingCategory = "test",
                SubCategory = new String[] { "Test1", "Test2", "Test3" },
                Image = "Automobile.png"
            };

            //Act
            var result = _masterDataRepo.AddCategory(dataObject);
            Assert.IsNotNull(result._id);
            _masterDataRepo.DeleteCategory(result._id);

            //Assert
            Assert.IsTrue(true);

        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteMasterDataTest_InvalidId()
        {
            _masterDataRepo.DeleteCategory("qwer");
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_DeleteListTest_NUllId()
        {
            _masterDataRepo.DeleteCategory(null);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Repo_UpdateMasterDataTest()
        {
            //Arrange
            Category lstObject = GetCategoryObject();
            //Act
            var result = _masterDataRepo.AddCategory(lstObject);
            Assert.IsNotNull(result._id);
            result.ListingCategory = "UpdatedHousing";

            var Updatedresult = _masterDataRepo.UpdateCategory(result._id, result);
            Assert.IsNotNull(Updatedresult);

            Assert.AreEqual(result.ListingCategory, Updatedresult.ListingCategory);
            Assert.IsInstanceOfType(result, typeof(Category));
        }
        #endregion
    }
}
