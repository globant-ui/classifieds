using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifieds.Listings.BusinessEntities
{
    public class ServiceAgentEntities
    {

    }
    public class Tags
    {
        public string[] SubCategory { get; set; }
        public string[] Location { get; set; }
    }

    public class UserInfo
    {
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string Location { get; set; }
        public string[] WishList { get; set; }
        public string Image { get; set; }
        public Tags Tags;
    }
}
