using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Listings.BusinessServices.ServiceAgent
{
    public interface IWebApiServiceAgent
    {
        string[] GetWishListListingIds(string accessToken, string userEmail);
    }
}
