using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Common;
using System.Web.Http.Cors;

namespace Classifieds.MasterDataAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform CRUD operation on MasterData Called Category
    /// class name: CategoryController
    /// Purpose : This class is used for to implement get/post/put/delete methods on category
    /// Created By : Varun Wadsamudrakar
    /// Created Date: 30/12/2016
    /// Modified by :
    /// Modified date: 
    /// </summary>

    [EnableCors("http://localhost:3000", "*", "*")]
    public class CategoryController : ApiController
    {
        #region Private Variable

        private readonly IMasterDataService _masterDataService;
        private readonly ILogger _logger;

        #endregion

        #region MastersDataController
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public CategoryController(IMasterDataService masterdataService, ILogger logger)
        {
            _masterDataService = masterdataService;
            _logger = logger;
        }

        #endregion

        #region GetAllCategory
        /// <summary>
        /// Returns the All Category 
        /// </summary>
        /// <param name="category">All category</param>
        /// <returns></returns>
        [HttpGet]
        public List<Category> GetAllCategory()
        {
            try
            {
                return _masterDataService.GetAllCategory().ToList();

            }
            catch (Exception ex)
            {
                throw _logger.Log(ex, "Globant/User");
            }
        }
        #endregion
        #region GetCategorySuggestion
        /// <summary>
        /// Returns All Categgries matching the imput text.
        /// </summary>
        /// <param name="categoryText">Category Text</param>
        /// <returns>Category List</returns>
        public List<string> GetCategorySuggetion(string categoryText)
        {
            List<string> result = null;
            try
            {
                result= _masterDataService.GetCategorySuggetion(categoryText).ToList();

            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Globant/User");
            }
            return result;
        }

        #endregion

        #region PostCategory

        /// <summary>
        /// Insert new Category item into the database
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Post(Category categoryObj)
        {
            HttpResponseMessage result = null;
            try
            {
                var classified = _masterDataService.CreateCategory(categoryObj);
                result = Request.CreateResponse<Category>(HttpStatusCode.Created, classified);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                throw _logger.Log(ex, "Globant/User");
            }

            return result;
        }

        #endregion

        #region UpdateCategory
        /// <summary>
        /// Update Category item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="Category">Category Object</param>
        /// <returns></returns>
        public HttpResponseMessage Put(string id, Category value)
        {
            HttpResponseMessage result = null;
            try
            {
                var classified = _masterDataService.UpdateCategory(id, value);
                result = Request.CreateResponse<Category>(HttpStatusCode.Accepted, classified);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                throw _logger.Log(ex, "Globant/User");
            }
            return result;
        }

        #endregion

        #region DeleteCategory
        /// <summary>
        /// Delete category item for given Id
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(string id)
        {
            HttpResponseMessage result = null;

            try
            {
                _masterDataService.DeleteCategory(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                throw _logger.Log(ex, "Globant/User");
            }

            return result;
        }

        #endregion
    }
}
