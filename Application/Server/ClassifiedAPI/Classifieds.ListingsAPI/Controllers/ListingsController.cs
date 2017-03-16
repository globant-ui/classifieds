using Classifieds.Common;
using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Common.Repositories;
using Classifieds.Listings.BusinessServices.ServiceAgent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Classifieds.ListingsAPI.Helpers;
using System.Configuration;
using System.Reflection;

namespace Classifieds.ListingsAPI.Controllers
{
    /// <summary>
    /// This Service is used to perform CRUD operation on Listings
    /// class name: ListingsController
    /// Purpose : This class is used for to implement get/post/put/delete methods on listings
    /// Created By : Suyash, Santosh
    /// Created Date: 08/12/2016
    /// Modified by : Ashish
    /// Modified date: 12/01/2017
    /// </summary>
    public class ListingsController : ApiController
    {
        #region Private Variable
        private readonly IListingService _listingService;
        private readonly ILogger _logger;
        private readonly ICommonRepository _commonRepository;
        private string _userEmail = string.Empty;
        private enum Status
        {
            Active,
            Closed,
            Expired
        };
        private string _accessToken = string.Empty;
        private readonly IWebApiServiceAgent _webApiServiceAgent;
        #endregion

        #region Constructor
        /// <summary>
        /// The class constructor. 
        /// </summary>
        public ListingsController(IListingService listingService, ILogger logger, ICommonRepository commonRepository, IWebApiServiceAgent webApiServiceAgent)
        {
            _listingService = listingService;
            _logger = logger;
            _commonRepository = commonRepository;
            _webApiServiceAgent = webApiServiceAgent;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the listing for given id
        /// </summary>
        /// <param name="id">listing id</param>
        /// <returns></returns>
        public ProductInfo GetListingById(string id)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                _accessToken = GetAccessToken();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                ProductInfo productInfo = new ProductInfo();
                var listing = _listingService.GetListingById(id);
                if (listing != null)
                {
                    #region Select common fields of listing entity and avoid redundant ones. Auto mapping/copy of one object to another(Dynamic object cloning) i.e. set all properies of 'ListingCommonFields' from 'Listing' entity. 
                    productInfo.Listing = new ListingCommonFields();
                    Type t = productInfo.Listing.GetType();
                    PropertyInfo[] propertyArray = t.GetProperties();
                    Object dynamicObject = t.InvokeMember("", BindingFlags.CreateInstance, null, productInfo.Listing, null);
                    foreach (PropertyInfo pi in propertyArray)
                    {
                        if (pi.CanWrite)
                        {
                            pi.SetValue(dynamicObject, listing.GetType().GetProperty(pi.Name).GetValue(listing), null);
                        }
                    }
                    productInfo.Listing = (ListingCommonFields)dynamicObject;
                    #endregion

                    #region Get Users details who submitted this listing card
                    if (!string.IsNullOrEmpty(productInfo.Listing.SubmittedBy))
                    {
                        var userDetails = _webApiServiceAgent.GetUserDetails(_accessToken, _userEmail, productInfo.Listing.SubmittedBy);
                        if (userDetails != null)
                        {
                            productInfo.UserName = userDetails.UserName;
                            productInfo.Email = userDetails.UserEmail;
                            productInfo.ContactNo = userDetails.Mobile;
                            productInfo.Designation = userDetails.Designation;
                            productInfo.Photo = userDetails.Image;
                        }
                    }
                    #endregion

                    #region To retrieve subcategory specific field name and their values within this listing card
                    if (!string.IsNullOrEmpty(productInfo.Listing.SubCategory))
                    {
                        var filters = _webApiServiceAgent.GetFilters(_accessToken, _userEmail, productInfo.Listing.SubCategory);
                        if (filters != null)
                        {
                            filters = filters.Where(flt => flt != "Sale/Rent").ToArray();
                            Fields[] fields = new Fields[filters.Length];
                            for (int item = 0; item < filters.Length; item++)
                            {
                                fields[item] = new Fields();
                                fields[item].FieldName = filters[item].ToString();
                                var properties = listing.GetType().GetProperty(filters[item]);
                                if (properties != null)
                                {
                                    fields[item].FieldValue = Convert.ToString(properties.GetValue(listing));
                                }                                
                            }                            
                            productInfo.Fields = fields;
                        }
                    }
                    #endregion
                }
                return productInfo;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the listings for given sub category
        /// </summary>
        /// <param name="subCategory">listing sub category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of filtered listings</returns>
        public List<Listing> GetListingsBySubCategory(string subCategory, int startIndex = 1, int pageCount = 10, bool isLast = false)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                if (startIndex < 0 || pageCount <= 0)
                {
                    string param = startIndex < 0 ? "Start Index" : "Page Count";
                    throw new Exception(param + "passed cannot be negative!");
                }

                return _listingService.GetListingsBySubCategory(subCategory, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the listings for given category
        /// </summary>
        /// <param name="category">listing category</param>
        /// <param name="startIndex">start index for page</param>
        /// <param name="pageCount">No of listings to include in result</param>
        /// <param name="isLast">Whether last page</param>
        /// <returns>Collection of listings</returns>
        public List<Listing> GetListingsByCategory(string category, int startIndex = 1, int pageCount = 10, bool isLast = false)
        {
            try
            {
                _userEmail = GetUserEmail();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                if (startIndex < 0 || pageCount <= 0)
                {
                    string param = startIndex < 0 ? "Start Index" : "Page Count";
                    throw new Exception(param + "passed cannot be negative!");
                }

                return _listingService.GetListingsByCategory(category, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        /// <summary>
        /// Insert new listing item into the database
        /// </summary>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public HttpResponseMessage Post(Listing listing)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                listing.Status = Status.Active.ToString();
                listing.SubmittedDate = DateTime.Now;
                //listing.Photos = CreateImagePath(listing).ToArray();
                var classified = _listingService.CreateListing(listing);
                result = Request.CreateResponse(HttpStatusCode.Created, classified);
                var newItemUrl = Url.Link("Listings", new { id = classified._id });
                result.Headers.Location = new Uri(newItemUrl);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// Update listing item for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage Put(string id, Listing listing)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                var classified = _listingService.UpdateListing(id, listing);
                result = Request.CreateResponse(HttpStatusCode.Accepted, classified);
                var newItemUrl = Url.Link("Listings", new { id = classified._id });
                result.Headers.Location = new Uri(newItemUrl);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Delete listing item for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <returns></returns>
        public HttpResponseMessage Delete(string id)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                _listingService.DeleteListing(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Returns top listings from database  
        /// </summary>
        /// <param name="noOfRecords">Number of records to be return </param>
        /// <returns></returns>
        public List<Listing> GetTopListings(int noOfRecords = 10)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                return _listingService.GetTopListings(noOfRecords);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        #region GetListingsByEmail

        /// <summary>
        /// Returns the listings for given email
        /// </summary>
        /// <param name="email">listing email</param>
        /// <param name="startIndex">listing startIndex</param>
        /// <param name="pageCount">listing pageCount</param>
        /// <param name="isLast">listing isLast</param>
        /// <returns></returns>
        public List<Listing> GetListingsByEmail(string email, int startIndex = 1, int pageCount = 10, bool isLast = false)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                if (startIndex < 0 || pageCount <= 0)
                {
                    string param = startIndex < 0 ? "Start Index" : "Page Count";
                    throw new Exception(param + "passed cannot be negative!");
                }

                return _listingService.GetListingsByEmail(email, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        #endregion GetListingsByEmail

        #region GetListingsByCategoryAndSubCategory

        /// <summary>
        /// Returns the listings for given Category and Subcategory
        /// </summary>
        /// <param name="category">listing category</param>
        /// <param name="subCategory">listing subCategory</param>
        /// <param name="startIndex">listing startIndex</param>
        /// <param name="pageCount">listing pageCount</param>
        /// <param name="isLast">listing isLast</param>
        /// <returns></returns>
        public List<Listing> GetListingsByCategoryAndSubCategory(string category, string subCategory, int startIndex = 1, int pageCount = 10, bool isLast = false)
        {
            try
            {
                _userEmail = GetUserEmail();
                string authResult = _commonRepository.IsAuthenticated(Request);
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                if (startIndex < 0 || pageCount <= 0)
                {
                    string param = startIndex < 0 ? "Start Index" : "Page Count";
                    throw new Exception(param + "passed cannot be negative!");
                }

                return _listingService.GetListingsByCategoryAndSubCategory(category, subCategory, _userEmail, startIndex, pageCount, isLast).ToList();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        #endregion GetListingsByCategoryAndSubCategory

        /// <summary>
        /// Returns User wish list collection from database  
        /// </summary>
        /// <returns></returns>
        public List<Listing> GetMyWishlist()
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                _accessToken = GetAccessToken();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                var listingIds = _webApiServiceAgent.GetWishListListingIds(_accessToken, _userEmail);
                if (listingIds != null)
                {
                    return _listingService.GetMyWishList(listingIds);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        /// <summary>
        /// Returns users recommended listings from database  
        /// </summary>
        /// <returns></returns>
        public List<Listing> GetRecommendedList()
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                _accessToken = GetAccessToken();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                var tags = _webApiServiceAgent.GetRecommendedTag(_accessToken, _userEmail);
                if (tags != null)
                {
                    return _listingService.GetRecommendedList(tags);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }
        #region PutCLoseListing

        /// <summary>
        /// Update listing status for given Id
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="listing">Listing Object</param>
        /// <returns></returns>
        public HttpResponseMessage PutCLoseListing(string id, Listing listing)
        {
            HttpResponseMessage result;
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }
                listing.Status = Status.Closed.ToString();
                var classified = _listingService.CLoseListing(id, listing);
                result = Request.CreateResponse(HttpStatusCode.Accepted, classified);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
            return result;
        }

        #endregion PutCLoseListing
        /// <summary>
        /// This method will add images to existing images array.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ListingImages>> PostListingImages(string listingId)
        {
            try
            {
                string authResult = _commonRepository.IsAuthenticated(Request);
                _userEmail = GetUserEmail();
                if (!(authResult.Equals("200")))
                {
                    throw new Exception(authResult);
                }

                if (Request.Content.IsMimeMultipartContent())
                {
                    string path = HttpContext.Current.Server.MapPath("~/");
                    path = path + ConfigurationManager.AppSettings["ListingImagePath"].ToString();
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string uploadPath = HttpContext.Current.Server.MapPath("~"+ConfigurationManager.AppSettings["ListingImagePath"].ToString());
                    StreamProvider streamProvider = new StreamProvider(uploadPath);
                    await Request.Content.ReadAsMultipartAsync(streamProvider);

                    List<ListingImages> lstImages = new List<ListingImages>();
                    foreach (var file in streamProvider.FileData)
                    {
                        FileInfo fi = new FileInfo(file.LocalFileName);
                        ListingImages objImage = new ListingImages();
                        objImage.ImageName = fi.Name;
                        objImage.Image = ConfigurationManager.AppSettings["ListingImagePath"].ToString()+ fi.Name;
                        lstImages.Add(objImage);
                    }
                    ListingImages[] imgArr = new ListingImages[] {};
                    imgArr = lstImages.ToArray();
                    //we can pass ImageList array and save in database
                    _listingService.UpdateImagePath(listingId, imgArr);
                    return lstImages;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
                    throw new HttpResponseException(response);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
       }

        #endregion

        #region private methods
        /// <summary>
        /// Returns user email string
        /// </summary>
        /// <returns>string</returns>
        private string GetUserEmail()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            message.Headers.TryGetValues("UserEmail", out headerValues);
            string hearderVal = headerValues == null ? string.Empty : headerValues.FirstOrDefault();
            return hearderVal;
        }

        private string GetAccessToken()
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            message.Headers.TryGetValues("AccessToken", out headerValues);
            string hearderVal = headerValues == null ? string.Empty : headerValues.FirstOrDefault();
            return hearderVal;
        }
        private List<ListingImages> CreateImagePath(Listing listing)
        {
            try
            {
                
                if (Request.Content.IsMimeMultipartContent())
                {
                    string path = HttpContext.Current.Server.MapPath("~/");
                    path = path + ConfigurationManager.AppSettings["ListingImagePath"].ToString()+listing.ListingCategory+"/"+listing.SubCategory;
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string uploadPath = HttpContext.Current.Server.MapPath("~" + ConfigurationManager.AppSettings["ListingImagePath"].ToString() + listing.ListingCategory + "/" + listing.SubCategory);
                    StreamProvider streamProvider = new StreamProvider(uploadPath);
                    Request.Content.ReadAsMultipartAsync(streamProvider);

                    List<ListingImages> lstImages = new List<ListingImages>();
                    foreach (var file in streamProvider.FileData)
                    {
                        FileInfo fi = new FileInfo(file.LocalFileName);
                        ListingImages objImage = new ListingImages();
                        objImage.ImageName = fi.Name;
                        objImage.Image = path + fi.Name;
                        lstImages.Add(objImage);
                    }
                    ListingImages[] imgArr = new ListingImages[] { };
                    imgArr = lstImages.ToArray();
                    return lstImages;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
                    throw new HttpResponseException(response);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, _userEmail);
                throw ex;
            }
        }

        #endregion
    }
}
