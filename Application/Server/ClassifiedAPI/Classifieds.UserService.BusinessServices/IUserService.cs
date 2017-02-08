using Classifieds.UserService.BusinessEntities;

namespace Classifieds.UserService.BusinessServices
{
    public interface IUserService
    {
        string RegisterUser(ClassifiedsUser user);
        ClassifiedsUser GetUserProfile(string userEmail);
        ClassifiedsUser UpdateUserProfile(string id, ClassifiedsUser userProfile);
        Subscription AddSubscription(Subscription subObject);
        void DeleteSubscription(string id);
    }
}
