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
    public class MastersDataRepositoryTest
    {
        #region Unit Test Cases

        #region Class Variables
        private IMasterDataRepository<Category> _masterDataRepo;
        private IDBRepository _dbRepository;
        private readonly List<Category> classifiedCategory = new List<Category>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _dbRepository = new DBRepository();
            _masterDataRepo = new MasterDataRepository<Category>(_dbRepository);

        }
        #endregion

        #region Setup
        private void SetUpClassifiedsCategory()
        {
            var lstCategory = GetCategoryObject();
            classifiedCategory.Add(lstCategory);
        }

        private Category GetCategoryObject()
        {
            Category dataObject = new Category
            {
                ListingCategory = "Automotive",
                //SubCategory = new String[] { "Car",
                //                            "Motor Cycle",
                //                            "Scooter",
                //                            "Bicycle",
                //                            "Accessories" },
                Image = "Automotive.png"
            };
            return dataObject;
        }
        #endregion

        #region GetAllCategoryTestCases

        /// <summary>
        /// test positive scenario for Get All Category  
        /// </summary>
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
        /// tests for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        public void GetAllCategory_Repo_Invalid_OR_Null()
        {
            var result = new List<Category>();
            Assert.AreEqual(0, result.Count);

            Assert.AreEqual(null, null);
        }

        #endregion GetAllCategoryTestCases

        #region GetCategorySuggetionTest
        
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
            _masterDataRepo.GetCategorySuggetion(null);
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

        #endregion GetCategorySuggetionTest

        #region PostCategoryTestCases
        
        /// <summary>
        /// test positive scenario for add data Object into the database
        /// </summary>
        [TestMethod]
        public void Repo_AddCategoryTest()
        {
            //Arrange
            var dataObject = GetCategoryObject();

            //Act
            var result = _masterDataRepo.AddCategory(dataObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Category));
        }

        /// <summary>
        /// test for adding empty Category object returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repo_AddCategoryTest_EmptyList_ThrowException()
        {
            var result = _masterDataRepo.AddCategory(null);
            Assert.IsNull(result, null);
        }

        #endregion AddCategoryTestCases        

        #region UpdateCategoryTestcases
        
        /// <summary>
        /// test positive scenario for updating lst object
        /// </summary>
        [TestMethod]
        public void Repo_UpdateCategoryTest()
        {
            //Arrange
            var lstObject = GetCategoryObject();
            //Act
            var result = _masterDataRepo.AddCategory(lstObject);
            Assert.IsNotNull(result._id);
            result.ListingCategory = "UpdatedAutomotive";

            var updatedResult = _masterDataRepo.UpdateCategory(result._id, result);
            Assert.IsNotNull(updatedResult);

            Assert.AreEqual(result.ListingCategory, updatedResult.ListingCategory);
            Assert.IsInstanceOfType(result, typeof(Category));
        }

        /// <summary>
        /// test for incorrect Category id returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Repo_UpdateCategoryTest_NullId_ThrowException()
        {
            var result = _masterDataRepo.UpdateCategory(null, null);
            Assert.IsNull(result);
        }

        #endregion UpdateCategoryTestcases

        #region DeleteCategoryTestCases
        
        /// <summary>
        /// test positive scenario for Delete Category by Id
        /// </summary>
        [TestMethod]
        public void Repo_DeleteCategoryTest()
        {
            //Arrange
            var dataObject = GetCategoryObject();

            //Act
            var result = _masterDataRepo.AddCategory(dataObject);
            Assert.IsNotNull(result._id);
            _masterDataRepo.DeleteCategory(result._id);
            var categoryobj = _masterDataRepo.GetCategoryById(result._id);

            //Assert
            Assert.IsNull(categoryobj);
        }


        /// <summary>
        /// test for incorrect id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Repo_DeleteCategoryTest_InvalidId()
        {
            _masterDataRepo.DeleteCategory("qwer");
            Assert.IsTrue(true);
        }


        /// <summary>
        /// test for null id returns exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Repo_DeleteCategoryTest_NUllId()
        {
            _masterDataRepo.DeleteCategory(null);
            Assert.IsNull(true);
        }

        #endregion DeleteCategoryTestCases

        #endregion
    }
}
