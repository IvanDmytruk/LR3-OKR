using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LW3_OKR
{
    public class Goods
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Type")]
        public string? Type { get; set; }
        [BsonElement("Name")]
        public string? Name { get; set; }
        [BsonElement("Quantity")]
        public int? Quantity { get; set; }
        public Goods()
        {
            Type = "";
            Name = "";
            Quantity = 0;
        }
        public Goods(string type, string name, int quantity)
        {
            Type = type;
            Name = name;
            Quantity = quantity;
        }
    }
    public class Sets
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("SetName")]
        public string? SetName { get; set; }
        [BsonElement("GoodsIds")]
        public List<string> GoodsIds { get; set; }
        public Sets()
        {
            SetName = "";
            GoodsIds = new List<string>();
        }
        public Sets(string setName, List<string> goodsIds)
        {
            SetName = setName;
            GoodsIds = goodsIds;
        }
    }
}
