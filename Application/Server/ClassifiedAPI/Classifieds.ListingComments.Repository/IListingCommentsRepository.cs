using System.Collections.Generic;
using Classifieds.ListingComments.BusinessEntities;

namespace Classifieds.ListingComments.Repository
{
    public interface IListingCommentsRepository<TEntity> where TEntity : ListingComment
    {
        List<TEntity> GetAllListingComment(string listingId);
        List<TEntity> GetListingCommentsById(string id);
        TEntity CreateListingComment(TEntity listObject);
        TEntity UpdateListingComment(string id, TEntity listObject);
        void DeleteListingComment(string id);
    }
}
