using System.Collections.Generic;
using Classifieds.UserService.BusinessEntities;

namespace Classifieds.UserService.Repository
{
    public interface IUserRepository
    {
        string RegisterUser(ClassifiedsUser user);
        UserToken SaveToken(UserToken userToken);
    }
}
