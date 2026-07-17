using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        [BsonElement("Name")]
        public string Name { get; set;  }
    }
}
