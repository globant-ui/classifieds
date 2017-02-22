using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Listings.BusinessServices.ServiceAgent
{
    public class WebApiServiceAgent : IWebApiServiceAgent
    {
        #region Private Variable

        private readonly string _baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
        private readonly string _userWishListApi = ConfigurationManager.AppSettings["UserWishListAPI"];
        private readonly string _userRecommondedTagListApi = ConfigurationManager.AppSettings["UserRecommondedTagListAPI"];

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
    }

   
}