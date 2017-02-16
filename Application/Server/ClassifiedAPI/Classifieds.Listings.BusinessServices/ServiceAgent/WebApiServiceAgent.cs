using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Classifieds.Listings.BusinessServices.ServiceAgent;

namespace Classifieds.Listings.BusinessServices.ServiceAgent
{
    public class WebApiServiceAgent : IWebApiServiceAgent
    {
        #region Private Variable

        private readonly string _baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
        private readonly string _userWishListApi = ConfigurationManager.AppSettings["UserWishListAPI"];

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
                wishlist = response.Content.ReadAsAsync<string[]>().Result;//var data = response.Content.ReadAsAsync<IEnumerable<string>>().Result;
            }
            else
            {
                //Something has gone wrong, handle it here
                return wishlist;
            }
            return wishlist;
        }
    }
}