using System.Collections.Generic;
using Classifieds.UserService.BusinessEntities;

namespace Classifieds.UserService.BusinessServices
{
    public interface IUserService
    {
        string RegisterUser(ClassifiedsUser user);
        UserToken SaveToken(UserToken userToken);
    }
}
