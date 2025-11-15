using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace LW3_OKR
{
    internal class Orders
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Number")]
        public string Number { get; set; }
        [BsonElement("Date")]
        public DateTime Date { get; set; }
        [BsonElement("PersonalId")] // типу хто приняв чи видав замовлення
        public string PersonalId { get; set; }
        [BsonElement("Sum")]
        public decimal Sum { get; set; }
        [BsonElement("Tip")]
        public decimal Tip { get; set; }
        public Orders()
        {
            Number = "";
            Date = DateTime.Now;
            PersonalId = "";
            Sum = 0;
            Tip = 0;
        }
        public Orders(string number, DateTime date, string personalId)
        {
            Number = number;
            Date = date;
            PersonalId = personalId;
            Sum = 0;
            Tip = 0;
        }
    }
}
