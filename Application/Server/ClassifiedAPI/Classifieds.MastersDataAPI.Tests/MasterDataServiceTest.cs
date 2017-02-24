using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.Repository;
using Moq;


namespace Classifieds.MastersData.BusinessServices.Test
{
    [TestClass]
    public class MastersDataServiceTest
    {
        #region Unit Test Cases

        #region Class Variables
        private Mock<IMasterDataRepository<Category>> _moqAppManager;
        private IMasterDataService _service;
        private readonly List<Category> _classifiedcategory = new List<Category>();
        private readonly List<string> _categoryList = new List<string>();
        
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IMasterDataRepository<Category>>();
            _service = new MasterDataService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsCategory()
        {
            var lstcategory = GetCategoryObject();
            _classifiedcategory.Add(lstcategory);
            _categoryList.Add("Automotive");
           
        }

       
        #endregion

        #region Private Methods
        /// <summary>
        /// for Get Data Object of Category
        /// </summary>
        private Category GetCategoryObject()
        {
            Category dataObject = new Category
            {
                _id = "9",
                ListingCategory = "Automotive",
                SubCategory = GetSubCategoryDataObject(),
                Image = "Automotive.png"
            };
            return dataObject;
        }

        private SubCategory[] GetSubCategoryDataObject()
        {
            SubCategory[] subCat = new SubCategory[2];

            for (int i = 0; i < 2; i++)
            {
                subCat[i] = new SubCategory();
                subCat[i].Name = "SubCategory" + i;
                subCat[i].Filters = GetFiltersDataObject();
            }
            return subCat;
        }

        private Filters[] GetFiltersDataObject()
        {
            Filters[] tempFilters = new Filters[2];
            for (int i = 0; i < 2; i++)
            {
                tempFilters[i] = new Filters();
                tempFilters[i].FilterName = "Filter" + i;
                tempFilters[i].FilterValues = new[] { "A" + i, "B" + i };
            }
            return tempFilters;
        }
        
        #endregion

        #region GetAllCategoryTestCases

        /// <summary>
        /// tests for null output if input is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllCategory_ThrowException()
        {
            SetUpClassifiedsCategory();
            var result = _service.GetAllCategory();
            Assert.IsNull(result);
        }

        /// <summary>
        /// tests the positive test criteria for Get All Category
        /// </summary>
        [TestMethod]
        public void GetAllCategoryTest()
        {
            // Arrange
            SetUpClassifiedsCategory();
            _moqAppManager.Setup(x => x.GetAllCategory()).Returns(_classifiedcategory);

            ////Act
            var result = _service.GetAllCategory();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// tests for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        public void GetAllCategory_EmptyResultTest()
        {
            // Arrange
            SetUpClassifiedsCategory();
            _moqAppManager.Setup(x => x.GetAllCategory()).Returns(new List<Category>() { new Category() });

            //Act
            var result = _service.GetAllCategory();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(CategoryViewModel));
        }

        #endregion GetAllCategoryTestCases

        #region PostCategoryTestCases


        /// <summary>
        /// test positive scenario for Post Category
        /// </summary>
        [TestMethod]
        public void PostCategoryTest()
        {
            //Arrange
            var lstObject = GetCategoryObject();
            _moqAppManager.Setup(x => x.AddCategory(It.IsAny<Category>())).Returns(lstObject);

            //Act
            var result = _service.CreateCategory(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Category));
        }

        /// <summary>
        /// test for inserting empty Category object return null result
        /// </summary>
        [TestMethod]
        public void PostCategoryTest_EmptyList()
        {
            var result = _service.CreateCategory(null);
            Assert.IsNull(result, null);
        }

        #endregion PostCategoryTestCases

        #region DeleteCategoryTestCases

        /// <summary>
        /// test positive scenario for DeleteList
        /// </summary>
        [TestMethod]
        public void DeleteCategoryTest()
        {
            //Arrange
            var lstObject = GetCategoryObject();
            _moqAppManager.Setup(x => x.DeleteCategory(It.IsAny<string>()));

            //Act
            _service.DeleteCategory(lstObject._id);

            //Assert
            Assert.IsTrue(true);
            _moqAppManager.Verify(v => v.DeleteCategory(lstObject._id), Times.Once());
        }

        /// <summary>
        /// test for null Exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteCategoryTest_InvalidId_ThrowException()
        {
            ArgumentNullException ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.DeleteCategory(null)).Throws(ex);
            _service.DeleteCategory(null);
        }

        #endregion DeleteCategoryTestCases

        #region PutCategoryTestCases

        /// <summary>
        /// test positive scenario for Update Category
        /// </summary>
        [TestMethod]
        public void PutCategoryTest()
        {
            //Arrange
            var lstObject = GetCategoryObject();
            _moqAppManager.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>())).Returns(lstObject);
            var updatedList = new Category() { ListingCategory = lstObject.ListingCategory };
            //Act
            var result = _service.UpdateCategory(lstObject._id, updatedList);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Category));
        }


        /// <summary>
        /// test negative scenario for Update invalid Category
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PutCategoryTest_InvalidId_ThrowException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            var updatedData = new Category() { ListingCategory = "testupdated" };
            _moqAppManager.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>())).Throws(ex);
            _service.UpdateCategory(null, updatedData);
        }

        #endregion PutCategoryTestCases

        #region  GetCategorySuggetionTest

        /// <summary>
        /// test positive scenario for GetListingsBySubCategory  
        /// </summary>
        [TestMethod]
        public void GetCategorySuggetionTest()
        {
            // Arrange
            SetUpClassifiedsCategory();
            _moqAppManager.Setup(x => x.GetCategorySuggestion(It.IsAny<string>())).Returns(_categoryList);

            //Act
            var result = _service.GetCategorySuggestion("Auto");

            //Assert
            Assert.AreEqual(result.Count, 1);
        }

        /// <summary>
        /// test for null input giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCategorySuggetion_ThrowsException()
        {
            _service.GetCategorySuggestion(null);
        }

        #endregion GetCategorySuggetionTest

        #region  GetSubCategorySuggetionTest

        /// <summary>
        /// test positive scenario for GetSubCategorySuggetionTest  
        /// </summary>
        [TestMethod]
        public void GetSubCategorySuggestionTest()
        {
            // Arrange
            SetUpClassifiedsCategory();
            _moqAppManager.Setup(x => x.GetSubCategorySuggestion(It.IsAny<string>())).Returns(_categoryList);

            //Act
            var result = _service.GetSubCategorySuggestion("car");

            //Assert
            Assert.AreEqual(result.Count, 1);
        }

        /// <summary>
        /// test for null input giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSubCategorySuggestion_ThrowsException()
        {
            _service.GetSubCategorySuggestion(null);
        }

        #endregion GetSubCategorySuggetionTest

        #region Filter Test Cases

        /// <summary>
        ///test positive scenario for get all filters by subcategory
        /// </summary>
        [TestMethod]
        public void GetAllFiltersBySubCategory()
        {
            //Arrange
            SetUpClassifiedsCategory();
            _moqAppManager.Setup(x => x.GetAllFiltersBySubCategory(It.IsAny<string>())).Returns(_classifiedcategory);

            //Act
            var objList = _service.GetAllFiltersBySubCategory("SubCategory0");

            //Assert
            Assert.AreEqual(objList.Filters[1].FilterName, "Filter1");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllFiltersBySubCategory_ThrowsException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.GetAllFiltersBySubCategory(null)).Throws(ex);
            _service.GetAllFiltersBySubCategory(null);
        }

        /// <summary>
        ///test positive scenario for get filters by filterName and subcategory
        /// </summary>
        [TestMethod]
        public void GetFiltersByFilterName()
        {
            SetUpClassifiedsCategory();
            _moqAppManager.Setup(x => x.GetFiltersByFilterName(It.IsAny<string>(), It.IsAny<string>())).Returns(_classifiedcategory);

            //Act
            var objList = _service.GetFiltersByFilterName("SubCategory0", "Filter1");

            //Assert
            Assert.AreEqual(objList.FilterValues.Length, 2);
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFiltersByFilterName_ThrowsException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.GetFiltersByFilterName(null, null)).Throws(ex);
            _service.GetFiltersByFilterName(null, null);
        }

        /// <summary>
        ///test positive scenario for GetFilter Names Only for given subcategory
        /// </summary>
        [TestMethod]
        public void GetFilterNamesOnly()
        {
            //Arrange
            SetUpClassifiedsCategory();
            _moqAppManager.Setup(x => x.GetFilterNamesOnly(It.IsAny<string>())).Returns(_classifiedcategory);
            

            //Act
            var objList = _service.GetFilterNamesOnly("SubCategory1");

            //Assert
            Assert.AreEqual(objList[1], "Filter1");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetFilterNamesOnly_ThrowsException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _moqAppManager.Setup(x => x.GetFilterNamesOnly(null)).Throws(ex);
            _service.GetFilterNamesOnly(null);
        }
        #endregion

        #endregion Unit Test Cases
    }
}
