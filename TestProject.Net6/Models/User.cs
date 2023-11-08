using MongoDB.Bson.Serialization.Attributes;

namespace TestProject.Net6.Models
{
    public class User
    {
        /// <summary>
        /// Gets the ID for this <see cref="User"/>
        /// </summary>
        [BsonElement("id")]
        [BsonRequired]
        public int Id { get; set; }

        /// <summary>
        /// Gets the name of this <see cref="User"/>
        /// </summary>
        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        /// <summary>
        /// Gets the email this <see cref="User"/> has
        /// </summary>
        [BsonElement("email")]
        [BsonRequired]
        public string Email { get; set; }

        /// <summary>
        /// Gets the password of this <see cref="User"/>
        /// </summary>
        [BsonElement("password")]
        [BsonRequired]
        public string Password { get; set; }
    }
}
