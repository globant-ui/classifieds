using Classifieds.UserService.BusinessEntities;
using System.Collections.Generic;

namespace Classifieds.UserService.BusinessServices
{
    public interface IUserService
    {
        string RegisterUser(ClassifiedsUser user);
        ClassifiedsUser GetUserProfile(string userEmail);
        ClassifiedsUser UpdateUserProfile(ClassifiedsUser userProfile);
        Subscription AddSubscription(Subscription subObject);
        void DeleteSubscription(string id);
        bool AddTag(string userEmail, Tags tag);
        bool AddAlert(string userEmail, Alert alert);
        bool DeleteTag(string userEmail, Tags tag);
        bool DeleteAlert(string userEmail, Alert alert);
    }
}
