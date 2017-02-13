using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.BusinessServices;
using System.Collections.Generic;
using Classifieds.MastersDataAPI.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Net;
using System.Web.Http;
using System.Web.Http.Routing;
using Classifieds.Common;
using Classifieds.Common.Repositories;

namespace Classifieds.MastersDataAPI.Tests
{
    [TestClass]
    public class MasterdataControllerTest
    {
        #region Unit Test Cases

        #region Class Variables

        private Mock<IMasterDataService> _mockService;
        private Mock<ILogger> _logger;
        private Mock<ICommonRepository> _mockAuthRepo;
        private readonly List<string> _categoryList = new List<string>();
        private readonly List<string> _subCategoryList = new List<string>();
        private CategoryController _controller;
        private readonly List<CategoryViewModel> _categoryViewModelList = new List<CategoryViewModel>();
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<IMasterDataService>();
            _logger = new Mock<ILogger>();
            _mockAuthRepo = new Mock<ICommonRepository>();
            _controller = new CategoryController(_mockService.Object, _logger.Object, _mockAuthRepo.Object);
        }
        #endregion

        #region Setup Methods

        private void SetUpClassifiedsListing()
        {
            _categoryList.Add("Automotive");
            _subCategoryList.Add("Car");
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// for Get Data Object of Category
        /// </summary>
        private Category GetCategoryDataObject()
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
                tempFilters[i].FilterValues = new [] { "A" + i, "B" + i };
            }
            return tempFilters;
        }

        private CategoryViewModel GetCategoryVmDataObject()
        {
            CategoryViewModel dataObject = new CategoryViewModel()
            {
                _id = "9",
                ListingCategory = "Automotive",
                SubCategory = new [] {"Cars", "Motorcycles", "Scooters", "Bicycles" },
                Image = "Automotive.png"
            };
            return dataObject;
        }
        #endregion

        #region GetAllCategoryTestCases


        /// <summary>
        /// test positive scenario for Get All category
        /// </summary>
        [TestMethod]
        public void GetAllCategoryTest()
        {
            _categoryViewModelList.Add(GetCategoryVmDataObject()); //SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetAllCategory()).Returns(_categoryViewModelList);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var objList = _controller.GetAllCategory();

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0].SubCategory[0], "Cars");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetCategory_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetAllCategory();
        }

        /// <summary>
        /// test negative scenario for empty category
        /// </summary>
        [TestMethod]
        public void GetAllCategory_EmptyCategoryTest()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _mockService.Setup(x => x.GetAllCategory()).Returns(new List<CategoryViewModel>());
            var result = _controller.GetAllCategory();

            //Assert
            Assert.AreEqual(result.Count, 0);
        }

        #endregion GetAllCategoryTestCases

        #region GetCategorySuggetionTest

        /// <summary>
        ///test positive scenario for Get Category list for maching input
        /// </summary>
        [TestMethod]
        public void GetCategorySuggetionTest()
        {
            SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetCategorySuggetion(It.IsAny<string>())).Returns(_categoryList);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
           var objList = _controller.GetCategorySuggetion("Auto");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0], "Automotive");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetCategorySuggetion_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetCategorySuggetion(null);
        }

        #endregion GetCategorySuggetionTest

        #region GetSubCategorySuggetionTest

        /// <summary>
        ///test positive scenario for Get sub Category list for maching input
        /// </summary>
        [TestMethod]
        public void GetSubCategorySuggetionTest()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            SetUpClassifiedsListing();
            _mockService.Setup(x => x.GetSubCategorySuggestion(It.IsAny<string>()))
              .Returns(_subCategoryList);
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var objList = _controller.GetSubCategorySuggestion("car");

            //Assert
            Assert.AreEqual(objList.Count, 1);
            Assert.AreEqual(objList[0], "Car");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetSubCategorySuggetion_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetSubCategorySuggestion(null);
        }

        #endregion GetCategorySuggetionTest

        #region PostMasterDataTestCases


        /// <summary>
        /// test positive scenario for Post category
        /// </summary>
        [TestMethod]
        public void Controller_PostCategoryTest()
        {
            // Arrange
            _mockService.Setup(x => x.CreateCategory(It.IsAny<Category>())).Returns(GetCategoryDataObject());
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };
            _controller.Configuration = new HttpConfiguration();
            _controller.Configuration.Routes.MapHttpRoute(
                name: "Category",
                routeTemplate: "api/{controller}/{method}/{id}",
                defaults: new { id = RouteParameter.Optional });

            _controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "Category" } });

            // Act
            var listObj = GetCategoryDataObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for inserting null listing object throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_PostCategory_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Post(null);
        }

        /// <summary>
        /// test positive scenario for PostCategory and verify response header location
        /// </summary>
        [TestMethod]
        public void Controller_PostcategoryTest_SetsLocationHeader_MockURLHelperVersion()
        {
            // This version uses a mock UrlHelper.
            // Arrange
            _mockService.Setup(x => x.CreateCategory(It.IsAny<Category>())).Returns(GetCategoryDataObject());
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost/Classifieds.MastersDataAPI/api/Category";

            // Create the mock and set up the Link method, which is used to create the Location header.
            // The mock version returns a fixed string.
            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            _controller.Url = mockUrlHelper.Object;

            // Act
            Category listObj = GetCategoryDataObject();
            var response = _controller.Post(listObj);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        
        #endregion PostMasterDataTestCases

        #region DeleteCategoryTestCases

        /// <summary>
        /// test positive scenario for deleting category
        /// </summary>
        [TestMethod]
        public void Controller_DeleteCategoryTest()
        {
            // Arrange
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            var dataObject = GetCategoryDataObject();
            _mockService.Setup(x => x.DeleteCategory(It.IsAny<string>()));
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            // Act                
            var response = _controller.Delete(dataObject._id);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(true, response.IsSuccessStatusCode);
        }

        /// <summary>
        /// test for deleting category object throws exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Controller_DeleteCatgory_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.Delete(null);
        }

        #endregion DeleteCategoryTestCases

        #region UpdateCategoryTestCases

        /// <summary>
        /// test positive scenario for updating Category
        /// </summary>
        [TestMethod]
        public void Controller_UpdateCategoryTest()
        {
            // Arrange
            _mockService.Setup(x => x.UpdateCategory(It.IsAny<string>(), It.IsAny<Category>())).Returns(GetCategoryDataObject());
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));
            _controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/api/Category")
            };
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            // Act     
            var dataObject = GetCategoryDataObject();
            var updatedProduct = new Category() { ListingCategory = dataObject.ListingCategory };
            var contentResult = _controller.Put(dataObject._id, updatedProduct);

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        /// <summary>
        ///  test for update listing with null Category id throws exception
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Controller_UpdateCategory_ThrowsException()
        {
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            var updatedProduct = new Category() { ListingCategory = "Automotive", Image = "" };
            _controller.Put(null, updatedProduct);
        }

        #endregion UpdateCategoryTestCases

        #region Filter Test Cases

        /// <summary>
        ///test positive scenario for get all filters by subcategory
        /// </summary>
        [TestMethod]
        public void Controller_GetAllFiltersBySubCategory()
        {
            SubCategory[] sc = GetSubCategoryDataObject();
            _mockService.Setup(x => x.GetAllFiltersBySubCategory(It.IsAny<string>())).Returns(sc[0]);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var objList = _controller.GetAllFiltersBySubCategory("SubCategory0");

            //Assert
            Assert.AreEqual(objList.Filters[1].FilterName, "Filter1");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetAllFiltersBySubCategory_ThrowsException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _mockService.Setup(x => x.GetAllFiltersBySubCategory(null)).Throws(ex);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetAllFiltersBySubCategory(null);
        }
        
        /// <summary>
        ///test positive scenario for get filters by filterName and subcategory
        /// </summary>
        [TestMethod]
        public void Controller_GetFiltersByFilterName()
        {
            Filters[] filter = GetFiltersDataObject();
            _mockService.Setup(x => x.GetFiltersByFilterName(It.IsAny<string>(),It.IsAny<string>())).Returns(filter[0]);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var objList = _controller.GetFiltersByFilterName("SubCategory0","Filter1");

            //Assert
            Assert.AreEqual(objList.FilterValues.Length, 2);
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetFiltersByFilterName_ThrowsException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _mockService.Setup(x => x.GetFiltersByFilterName(null,null)).Throws(ex);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetFiltersByFilterName(null, null);
        }

        /// <summary>
        ///test positive scenario for GetFilter Names Only for given subcategory
        /// </summary>
        [TestMethod]
        public void Controller_GetFilterNamesOnly()
        {
            Filters[] filters = GetFiltersDataObject();
            List<string> filterNames= new List<string>();
            foreach (var flt in filters)
            {
                filterNames.Add(flt.FilterName);
            }
            _mockService.Setup(x => x.GetFilterNamesOnly(It.IsAny<string>())).Returns(filterNames);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _logger.Setup(x => x.Log(It.IsAny<Exception>(), It.IsAny<string>()));

            //Act
            var objList = _controller.GetFilterNamesOnly("SubCategory1");

            //Assert
            Assert.AreEqual(objList[1], "Filter1");
        }

        /// <summary>
        /// test for null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Controller_GetFilterNamesOnly_ThrowsException()
        {
            var ex = new ArgumentNullException("ArgumentNullException", new ArgumentNullException());
            _mockService.Setup(x => x.GetFilterNamesOnly(null)).Throws(ex);
            _mockAuthRepo.Setup(x => x.IsAuthenticated(It.IsAny<HttpRequestMessage>())).Returns("200");
            _controller.GetFilterNamesOnly(null);
        }

        #endregion End Filter Test Cases

        #endregion
    }
}
