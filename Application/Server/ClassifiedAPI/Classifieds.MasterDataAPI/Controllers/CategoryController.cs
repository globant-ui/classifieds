using Classifieds.MastersData.BusinessEntities;
using Classifieds.MastersData.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Common;
using Classifieds.Common.Repositories;

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
    public class CategoryController : ApiController
    {
        #region Private Variable

        private readonly IMasterDataService _masterDataService;
        private readonly ILogger _logger;
        private readonly ICommonRepository _commonRepository;
        private string _userEmail = string.Empty;
        #endregion

        #region MastersDataController
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public CategoryController(IMasterDataService masterdataService, ILogger logger, ICommonRepository commonRepository)
        {
            _masterDataService = masterdataService;
            _logger = logger;
            _commonRepository = commonRepository;
        }

        #endregion

        #region GetAllCategory
        /// <summary>
        /// Returns the All Category 
        /// </summary>
        /// <returns>Category Colletion</returns>
        [HttpGet]
        public List<Category> GetAllCategory()
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = getUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                
                return _masterDataService.GetAllCategory().ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
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
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = getUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                result = _masterDataService.GetCategorySuggetion(categoryText).ToList();

            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
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
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = getUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                
                var classified = _masterDataService.CreateCategory(categoryObj);
                result = Request.CreateResponse<Category>(HttpStatusCode.Created, classified);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                _logger.Log(ex, _userEmail);
                throw ex;
            }

            return result;
        }

        #endregion

        #region UpdateCategory
        /// <summary>
        /// Update Category item for given Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="value">Category Object</param>
        /// <returns></returns>
        public HttpResponseMessage Put(string id, Category value)
        {
            HttpResponseMessage result = null;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = getUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                
                var classified = _masterDataService.UpdateCategory(id, value);
                result = Request.CreateResponse<Category>(HttpStatusCode.Accepted, classified);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                _logger.Log(ex, _userEmail);
                throw ex;
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
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = getUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                
                _masterDataService.DeleteCategory(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                _logger.Log(ex, _userEmail);
                throw ex;
            }

            return result;
        }

        #endregion

        #region private methods
        private string getUserEmail()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            bool found = message.Headers.TryGetValues("UserEmail", out headerValues);
            return headerValues.FirstOrDefault();
        }
        #endregion
    }
}
