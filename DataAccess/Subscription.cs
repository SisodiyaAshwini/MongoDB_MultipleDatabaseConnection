using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    [BsonIgnoreExtraElements]
    public class Subscription
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("IsActive")]
        public bool IsActive { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("LastUpdatedBy")]
        public string UpdatedBy { get; set; }

        [BsonElement("LastUpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        [BsonElement("Comments")]
        public string Comments { get; set; }

        [BsonElement("RetentionSettings")]
        public List<RetentionSetting> RetentionSettings { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RetentionSetting
    {
        [BsonElement("RetentionPeriod")]
        public int RetentionPeriod { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

    }
}
