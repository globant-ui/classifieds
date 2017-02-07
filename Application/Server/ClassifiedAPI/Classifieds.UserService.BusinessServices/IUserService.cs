using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.BusinessServices
{
    public interface IUserService
    {
        string RegisterUser(ClassifiedsUser user);
        ClassifiedsUser GetUserProfile(string userEmail);
        ClassifiedsUser UpdateUserProfile(string id, ClassifiedsUser userProfile);
    }
}
