using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.Repository
{
    public interface IUserRepository<TEntity> where TEntity : ClassifiedsUser
    {
        string RegisterUser(TEntity user);
        TEntity GetUserProfile(string userEmail);
        TEntity UpdateUserProfile(string id, TEntity userProfile);
    }
}
