using Classifieds.Listings.BusinessEntities;
using Classifieds.Listings.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Classifieds.ListingsAPI.Controllers
{
    public class ListingsController : ApiController
    {
        private IListingService _listingService;

        public ListingsController(IListingService listingService)
        {
            _listingService = listingService;
        }

        public string Get()
        {
            return "Hi Classifieds";
        }

        public List<Listing> GetListingById(string id)
        {
            try
            {
                return _listingService.GetListingById(id).ToList();
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }

        }

        /// <summary>
        /// Returns the listings for given category
        /// </summary>
        /// <param name="category">listing category</param>
        /// <returns></returns>
        public List<Listing> GetListingsByCategory(string category)
        {
            try
            {
                return _listingService.GetListingsByCategory(category).ToList();
            }
            catch (Exception ex)
            {
                //log exception
                throw ex;
            }
        }

        public HttpResponseMessage Post(Listing listingObj)
        {
            HttpResponseMessage result = null;

            if (ModelState.IsValid)
            {
                try
                {
                    var Classified = _listingService.CreateListing(listingObj);
                    result = Request.CreateResponse<Listing>(HttpStatusCode.Created, Classified);
                    string newItemURL = Url.Link("Listings", new { id = Classified._id });
                    result.Headers.Location = new Uri(newItemURL);
                }
                catch (Exception ex)
                {
                    //log exception //Trace.TraceError(ex.Message, ex);
                    result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                result = GetBadRequestResponse();
            }

            return result;
        }

        public HttpResponseMessage Put(string id, Listing value)
        {
            HttpResponseMessage result = null;

            if (ModelState.IsValid)
            {
                try
                {
                   var Classified = _listingService.UpdateListing(id, value);
                    result = Request.CreateResponse<Listing>(HttpStatusCode.Accepted, Classified);
                    //result = Request.CreateResponse(HttpStatusCode.NoContent);
                }
                catch (Exception ex)
                {
                    //Trace.TraceError(ex.Message, ex);
                    result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                result = GetBadRequestResponse();
            }

            return result;
        }

        public HttpResponseMessage Delete(string id)
        {
            HttpResponseMessage result = null;

            try
            {
                _listingService.DeleteListing(id);
                result = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                //Trace.TraceError(ex.Message, ex);
                result = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }

            return result;
        }

        private HttpResponseMessage GetBadRequestResponse()
        {
            HttpResponseMessage response = null;
            List<string> errors = new List<string>();
            foreach (var modelSt in ModelState.Values)
            {
                foreach (var error in modelSt.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            response = Request.CreateResponse<IEnumerable<string>>(HttpStatusCode.BadRequest, errors);
            return response;
        }
    }
}
