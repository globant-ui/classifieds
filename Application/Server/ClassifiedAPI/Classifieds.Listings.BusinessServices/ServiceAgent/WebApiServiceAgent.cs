using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Classifieds.Listings.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.Listings.BusinessServices.ServiceAgent
{
    public class WebApiServiceAgent : IWebApiServiceAgent
    {
        #region Private Variable

        private readonly string _baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
        private readonly string _userWishListApi = ConfigurationManager.AppSettings["UserWishListAPI"];
        private readonly string _userRecommondedTagListApi = ConfigurationManager.AppSettings["UserRecommondedTagListAPI"];
        private readonly string _userProfileApi = ConfigurationManager.AppSettings["UserProfileAPI"];
        private readonly string _filtersNameOnlyAPI = ConfigurationManager.AppSettings["FiltersNameOnlyAPI"];

        #endregion

        public string[] GetWishListListingIds(string accessToken, string userEmail)
        {
            string[] wishlist = {};

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AccessToken", accessToken); 
            client.DefaultRequestHeaders.Add("UserEmail", userEmail); 
            HttpResponseMessage response = client.GetAsync(_userWishListApi + userEmail).Result;
            if (response.IsSuccessStatusCode)
            {
                wishlist = response.Content.ReadAsAsync<string[]>().Result;
            }
            else
            {
                //Something has gone wrong, handle it here
            }
            return wishlist;
        }

        public Tags GetRecommendedTag(string accessToken, string userEmail)
        {
            var recommendedTags = new Tags();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AccessToken", accessToken);
            client.DefaultRequestHeaders.Add("UserEmail", userEmail);
            HttpResponseMessage response = client.GetAsync(_userRecommondedTagListApi + userEmail).Result;
            if (response.IsSuccessStatusCode)
            {
                recommendedTags = response.Content.ReadAsAsync<Tags>().Result;
            }
            return recommendedTags;
        }

        public UserInfo GetUserDetails(string accessToken, string userEmail, string submittedBy)
        {
            var userDetails = new UserInfo();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AccessToken", accessToken);
            client.DefaultRequestHeaders.Add("UserEmail", userEmail);
            HttpResponseMessage response = client.GetAsync(_userProfileApi + submittedBy).Result;
            if (response.IsSuccessStatusCode)
            {
                userDetails = response.Content.ReadAsAsync<UserInfo>().Result;
            }
            return userDetails;
        }

        public string[] GetFilters(string accessToken, string userEmail, string subCategory)
        {
            var filter = new string[] { };

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AccessToken", accessToken);
            client.DefaultRequestHeaders.Add("UserEmail", userEmail);
            HttpResponseMessage response = client.GetAsync(_filtersNameOnlyAPI + subCategory).Result;
            if (response.IsSuccessStatusCode)
            {
                filter = response.Content.ReadAsAsync<string[]>().Result;
            }
            return filter;
        }
    }

   
}