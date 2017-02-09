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
        void DeleteTag(string userEmail, Tags tag);
        void AddAlert(string userEmail, Alert alert);
        void DeleteAlert(string userEmail, Alert alert);
    }
}
