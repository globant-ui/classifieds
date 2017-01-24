using System.Net.Http;
using Classifieds.Common.Entities;

namespace Classifieds.Common.Repositories
{
    public interface ICommonRepository
    {
        string IsAuthenticated(HttpRequestMessage request);
        UserToken SaveToken(UserToken userToken);
    }
}
