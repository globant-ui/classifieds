using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.Repository
{
    public interface IUserRepository<TEntity> where TEntity : ClassifiedsUser
    {
        string RegisterUser(TEntity user);
        TEntity GetUserProfile(string userEmail);
        TEntity UpdateUserProfile(TEntity userProfile);
        void AddTag(string userEmail, Tags tag);
        bool DeleteTag(string userEmail, string tagName);
        bool AddAlert(string userEmail, Alert alert);
        bool DeleteAlert(string userEmail, Alert alert);
        bool AddtoWishList(string userEmail, string listingId);
        bool DeleteFromWishList(string userEmail, string listingId);
        string[] GetUserWishList(string userEmail);
        Tags GetRecommondedTagList(string userEmail);
    }
}
