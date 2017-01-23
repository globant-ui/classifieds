using System.Collections.Generic;
using Classifieds.ListingComments.BusinessEntities;

namespace Classifieds.ListingComments.BusinessServices
{
    public interface IListingCommentService
    {
        List<ListingComment> GetAllListingComment(string listingId);
        ListingComment CreateListingComment(ListingComment listObject);
        ListingComment UpdateListingComment(string id, ListingComment listObject);
        void DeleteListingComment(string id);
    }
}
