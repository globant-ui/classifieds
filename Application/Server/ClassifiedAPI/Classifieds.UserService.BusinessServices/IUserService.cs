using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.BusinessServices
{
    public interface IUserService
    {
        string RegisterUser(ClassifiedsUser user);
        ClassifiedsUser GetUserProfile(string userEmail);
        ClassifiedsUser UpdateUserProfile(ClassifiedsUser userProfile);
        void AddTag(string userEmail, Tags tag);
        void AddAlert(string userEmail, Alert alert);
        void DeleteTag(string userEmail, Tags tag);
        void DeleteAlert(string userEmail, Alert alert);
    }
}
