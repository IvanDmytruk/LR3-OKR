using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace LW3_OKR
{
    public class Personal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("SurName")]
        public string SurName { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Position")]
        public string Position { get; set; }
        [BsonElement("Stat")]
        public string Stat { get; set; }
        [BsonElement("Image")]
        public string Image { get; set; }
        public Personal () 
        {
            SurName = "";
            Name = "";
            Position = "";  
            Stat = "";
        }
        public Personal (string surname, string name, string position, string stat)
        {
            SurName = surname;
            Name = name;
            Position = position;
            Stat = stat;
        }
    }
    public class MongoDBPersonal
    {
        private IMongoCollection<Personal> personalCollection;
        public MongoDBPersonal()
        {
            var client = new MongoClient("mongodb+srv://ivandmytruk42_db_user:lwokr123@db.rdcvntl.mongodb.net/?appName=DB");

            var database = client.GetDatabase("LW3_OKR_DB");
            personalCollection = database.GetCollection<Personal>("Personal");
        }
        public List<Personal> GetAllPersonals()
        {
            return personalCollection.Find(new BsonDocument()).ToList();
        }
        public void AddPersonal(Personal personal)
        {
            personalCollection.InsertOne(personal);
        }
        public void UpdatePersonal(string id, Personal updatedPersonal)
        {
            var filter = Builders<Personal>.Filter.Eq(p => p.Id, id);
            personalCollection.ReplaceOne(filter, updatedPersonal);
        }
        public void DeletePersonal(string id)
        {
            var filter = Builders<Personal>.Filter.Eq(p => p.Id, id);
            personalCollection.DeleteOne(filter);
        }
    }
}

