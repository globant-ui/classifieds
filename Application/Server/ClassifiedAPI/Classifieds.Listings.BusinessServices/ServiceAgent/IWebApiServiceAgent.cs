using Classifieds.Listings.BusinessEntities;

namespace Classifieds.Listings.BusinessServices.ServiceAgent
{
    public interface IWebApiServiceAgent
    {
        string[] GetWishListListingIds(string accessToken, string userEmail);
        Tags GetRecommendedTag(string accessToken, string userEmail);
        UserInfo GetUserDetails(string accessToken, string userEmail, string submittedBy);
        string[] GetFilters(string accessToken, string userEmail, string subCategory);
    }
}
