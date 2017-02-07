using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Classifieds.Search.BusinessEntities
{
    public class Classified
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } //MongoDb uses this field as identity.
        public string ListingType { get; set; }
        public string ListingCategory { get; set; }
        public string SubCategory { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string ContactName { get; set; }
        public string Configuration { get; set; }
        public string Details { get; set; }
        public string Brand { get; set; }
        public Int32 Price { get; set; }
        public Int32 YearOfPurchase { get; set; }
        public string ExpiryDate { get; set; }
        public string Status { get; set; }
        public string Submittedby { get; set; }
        public string SubmittedDate { get; set; }
        public string IdealFor { get; set; }
        public string Furnished { get; set; }
        public string FuelType { get; set; }
        public Int32 KmDriven { get; set; }
        public Int32 YearofMake { get; set; }
        public string Dimensions { get; set; }
        public string TypeofUse { get; set; }
        public string Photos { get; set; }
    }
}
