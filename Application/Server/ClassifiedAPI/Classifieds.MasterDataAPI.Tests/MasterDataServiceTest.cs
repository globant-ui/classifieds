using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.Repository;
using Moq;


namespace Classifieds.MastersData.BusinessServices.Test
{
    [TestClass]
    public class MasterDataServiceTest
    {
        #region Class Variables
        private Mock<IMasterDataRepository> _moqAppManager;
        private IMasterDataService _service;
        private readonly List<Category> classifiedcategory = new List<Category>();
        private readonly List<string> categoryList = new List<string>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IMasterDataRepository>();
            _service = new MasterDataService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsMasterData()
        {
            var lstcategory = GetCategoryObject();
            classifiedcategory.Add(lstcategory);
            categoryList.Add("Automotive");
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllCategory_ThrowsException()
        {
            var result = _service.GetAllCategory();
        }


        /// <summary>
        /// tests the positive test criteria
        /// </summary>
        [TestMethod]
        public void GetAllCategoryTest()
        {
            // Arrange
            SetUpClassifiedsMasterData();
            _moqAppManager.Setup(x => x.GetAllCategory()).Returns(classifiedcategory);

            //Act
            var result = _service.GetAllCategory();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

        [TestMethod]
        public void PostCategoryTest()
        {
            //Arrange
            Category lstObject = GetCategoryObject();
            _moqAppManager.Setup(x => x.AddCategory(It.IsAny<Category>())).Returns(lstObject);

            //Act
            var result = _service.CreateCategory(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Category));
        }


        [TestMethod]
        public void DeleteCategoryTest()
        {
            //Arrange
            Category lstObject = GetCategoryObject();
            _moqAppManager.Setup(x => x.DeleteCategory(It.IsAny<string>()));

            //Act
            _service.DeleteCategory(lstObject._id);

            //Assert
            Assert.IsTrue(true);
            _moqAppManager.Verify(v => v.DeleteCategory(lstObject._id), Times.Once());
        }

        [TestMethod]
        public void DeleteCategoryTest_InvalidId()
        {
            _service.DeleteCategory(null);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void PutCategoryTest()
        {
            //Arrange
            Category lstObject = GetCategoryObject();
            _moqAppManager.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>())).Returns(lstObject);
            var updatedList = new Category() { ListingCategory = lstObject.ListingCategory };
            //Act
            var result = _service.UpdateCategory(lstObject._id, updatedList);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Category));
        }

        [TestMethod]
        public void PutCategoryTest_InvalidId()
        {
            var updatedData = new Category() { ListingCategory = "testupdated" };
            var result = _service.UpdateCategory(null, updatedData);
            Assert.IsNull(result);
        }
        #endregion

        /// <summary>
        /// test positive scenario for GetListingsBySubCategory  
        /// </summary>
        [TestMethod]
        public void GetCategorySuggetionTest()
        {
            // Arrange
            SetUpClassifiedsMasterData();
            _moqAppManager.Setup(x => x.GetCategorySuggetion(It.IsAny<string>())).Returns(categoryList);

            //Act
            var result = _service.GetCategorySuggetion("Auto");

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
            var result = _service.GetCategorySuggetion(null);
        }
    }
}
