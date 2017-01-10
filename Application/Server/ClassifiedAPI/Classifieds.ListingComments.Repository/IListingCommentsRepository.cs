using System.Collections.Generic;
using Classifieds.ListingComments.BusinessEntities;

namespace Classifieds.ListingComments.Repository
{
    public interface IListingCommentsRepository
    {
        List<ListingComment> GetAllListingComment(string listingId);
        ListingComment CreateListingComment(ListingComment listObject);
        ListingComment UpdateListingComment(string id, ListingComment listObject);
        void DeleteListingComment(string id);
    }
}
