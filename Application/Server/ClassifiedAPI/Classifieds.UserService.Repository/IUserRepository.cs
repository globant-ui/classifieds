using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.Repository
{
    public interface IUserRepository
    {
        string RegisterUser(ClassifiedsUser user);
        ClassifiedsUser GetUserProfile(string userEmail);
        ClassifiedsUser UpdateUserProfile(string id, ClassifiedsUser userProfile);
    }
}
