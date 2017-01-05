using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using Classifieds.Listings.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Classifieds.ListingsAPI.Tests
{
    [TestClass]
    public class ListingBusinessServiceTest
    {
        #region Class Variables
        private Mock<IListingRepository> _moqAppManager;
        private IListingService _service;
        private readonly List<Listing> classifiedList = new List<Listing>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _moqAppManager = new Mock<IListingRepository>();
            _service = new ListingService(_moqAppManager.Object);
        }
        #endregion

        #region Setup
        private void SetUpClassifiedsListing()
        {
            var lstListing = GetListObject();
            classifiedList.Add(lstListing);            
        }

        private Listing GetListObject()
        {
            Listing listObject = new Listing
            {
                _id = "9",
                ListingType = "test",
                ListingCategory = "test",
                SubCategory = "test",
                Title = "test",
                Address = "AAA",
                ContactNo = "1111",
                ContactName = "AAA AAA",
                Configuration = "NA",
                Details = "for rupees 20,000,000,000",
                Brand = "test",
                Price = 123,
                YearOfPurchase = 123,
                ExpiryDate = "test",
                Status = "test",
                Submittedby = "test",
                SubmittedDate = "test",
                IdealFor = "test",
                Furnished = "test",
                FuelType = "test",
                KmDriven = 123,
                YearofMake = 123,
                Dimensions = "test",
                TypeofUse = "test",
                Photos = new string[] { "/Photos/Merc2016.jpg", "/Photos/Merc2016.jpg" }
            };
            return listObject;
        }
        #endregion

        #region Unit Test Cases
        /// <summary>
        /// test positive scenario for Get Listing By Id  
        /// </summary>
        [TestMethod]
        public void GetListingByIdTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingById(It.IsAny<string>())).Returns(classifiedList);

            //Act
            var result = _service.GetListingById("123");

            //Assert
            Assert.AreEqual(result.Count, 1);
        }

        /// <summary>
        /// test for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        public void GetListingById_EmptyResult_Test()
        {
            // Arrange
            _moqAppManager.Setup(x => x.GetListingById(It.IsAny<string>())).Returns(new List<Listing>() { new Listing() });

            //Act
            var result = _service.GetListingById("123");

            //Assert
            Assert.IsNotNull(result[0], null);
            Assert.IsInstanceOfType(result, typeof(IList<Listing>));
        }

        /// <summary>
        /// test for null input giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingById_ThrowsException()
        {
            var result = _service.GetListingById(null);
        }

        /// <summary>
        /// test positive scenario for GetListingsBySubCategory  
        /// </summary>
        [TestMethod]
        public void GetListingsBySubCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingsBySubCategory(It.IsAny<string>())).Returns(classifiedList);

            //Act
            var result = _service.GetListingsBySubCategory("test");

            //Assert
            Assert.AreEqual(result.Count, 1);
        }

        /// <summary>
        /// test for null input giving exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetListingsBySubCategory_ThrowsException()
        {
            var result = _service.GetListingsBySubCategory(null);
        }

        /// <summary>
        /// test for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        public void GetListingsBySubCategory_EmptyResult_Test()
        {
            // Arrange
            _moqAppManager.Setup(x => x.GetListingsBySubCategory(It.IsAny<string>())).Returns(new List<Listing>() { new Listing() });

            //Act
            var result = _service.GetListingsBySubCategory("123");

            //Assert
            Assert.IsNotNull(result[0], null);
            Assert.IsInstanceOfType(result, typeof(IList<Listing>));
        }
        
        /// <summary>
        /// tests the positive test criteria
        /// </summary>
        [TestMethod]
        public void GetListingByCategoryTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingsByCategory("Housing")).Returns(classifiedList);

            //Act
            var result = _service.GetListingsByCategory("Housing");

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
        }

        /// <summary>
        /// tests for incorrect input giving empty result
        /// </summary>
        [TestMethod]
        public void GetListingByCategory_EmptyResultTest()
        {
            // Arrange
            SetUpClassifiedsListing();
            _moqAppManager.Setup(x => x.GetListingsByCategory("Housing")).Returns(new List<Listing>() { new Listing() });

            //Act
            var result = _service.GetListingsByCategory("Housing");

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(Listing));
        }

        /// <summary>
        /// tests for null output if input is null
        /// </summary>
        [TestMethod]
        public void GetListingByCategory_ReturnsNull()
        {
            var result = _service.GetListingsByCategory(null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// test positive scenario for PostList
        /// </summary>
        [TestMethod]
        public void PostListTest()
        {
            //Arrange
            Listing lstObject = GetListObject();
            _moqAppManager.Setup(x => x.Add(It.IsAny<Listing>())).Returns(lstObject);

            //Act
            var result = _service.CreateListing(lstObject);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for inserting empty listing object return null result
        /// </summary>
        [TestMethod]
        public void PostListTest_EmptyList()
        {
            var result = _service.CreateListing(null);
            Assert.IsNull(result, null);
        }

        /// <summary>
        /// test positive scenario for DeleteList
        /// </summary>
        [TestMethod]
        public void DeleteListTest()
        {
            //Arrange
            Listing lstObject = GetListObject();
            _moqAppManager.Setup(x => x.Delete(It.IsAny<string>()));

            //Act
            _service.DeleteListing(lstObject._id);

            //Assert
            Assert.IsTrue(true);
            _moqAppManager.Verify(v => v.Delete(lstObject._id), Times.Once());
        }

        /// <summary>
        /// test for null listing id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void DeleteListTest_InvalidId_ThrowException()
        {
            _moqAppManager.Setup(x => x.Delete(It.IsAny<string>()));
            _service.DeleteListing(null);            
        }

        /// <summary>
        /// test positive scenario for update listing
        /// </summary>
        [TestMethod]
        public void PutListTest()
        {
            //Arrange
            Listing lstObject = GetListObject();
            _moqAppManager.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<Listing>())).Returns(lstObject);
            var updatedList = new Listing() { Title = lstObject.Title, ListingType = lstObject.ListingType };
            //Act
            var result = _service.UpdateListing(lstObject._id, updatedList);

            //Assert
            Assert.IsNotNull(result, null);
            Assert.IsInstanceOfType(result, typeof(Listing));
        }

        /// <summary>
        /// test for updating listing with incorrect id returns null result
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PutListTest_InvalidId_ThrowException()
        {
            var updatedList = new Listing() { Title = "testupdated", ListingType = "testupdated" };
            _service.UpdateListing(null, updatedList);          
        }

        /// <summary>
        /// test for positive scenario for get top listing
        /// </summary>
        [TestMethod]
        public void GetTopListing_5RecordsTest()
        {
            // Arrange
            //SetUpClassifiedsListing();
            List<Listing> list = new List<Listing>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(GetListObject());
            }
            _moqAppManager.Setup(x => x.GetTopListings(It.IsAny<int>())).Returns(list);
            
            //Act
            var result = _service.GetTopListings(5);

            //Assert
            Assert.AreEqual(result.Count, 5);
        }       

        #endregion
    }
}
