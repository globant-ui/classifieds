using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.Repository
{
    public interface IUserRepository<TEntity> where TEntity : ClassifiedsUser
    {
        string RegisterUser(TEntity user);
        TEntity GetUserProfile(string userEmail);
        TEntity UpdateUserProfile(TEntity userProfile);
        bool AddTag(string userEmail, Tags tag);
        bool DeleteTag(string userEmail, Tags tag);
        bool AddAlert(string userEmail, Alert alert);
        bool DeleteAlert(string userEmail, Alert alert);
    }
}
