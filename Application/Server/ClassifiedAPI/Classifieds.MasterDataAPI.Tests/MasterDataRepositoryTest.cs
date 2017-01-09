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
                ListingCategory = "Automotive",
                SubCategory = new String[] { "Car",
                                            "Motor Cycle",
                                            "Scooter",
                                            "Bicycle",
                                            "Accessories" },
                Image = "Automotive.png"
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

        /// <summary>
        /// test positive scenario for get category list for matching input
        /// </summary>
        [TestMethod]
        public void GetCategorySuggetionTest()
        {
            //Act
            var result = _masterDataRepo.GetCategorySuggetion("Aut");

            //Assert
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// test for null input Category Text returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCategorySuggetionTest_NullSubCategoryText()
        {
            var result = _masterDataRepo.GetCategorySuggetion(null);
        }

        /// <summary>
        /// test for invalid input CategoryText returns null result
        /// </summary>
        [TestMethod]
        public void GetCategorySuggetionTest_InvalidSubCategory()
        {
            var result = _masterDataRepo.GetCategorySuggetion("qwer");
            Assert.IsNull(result);
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
                ListingCategory = "Automotive",
                SubCategory = new String[] { "Car",
                                            "Motor Cycle",
                                            "Scooter",
                                            "Bicycle",
                                            "Accessories" },
                Image = "Automotive.png"
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
