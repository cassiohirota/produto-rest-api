﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProductCatalogApi.Models {
    public class Categoria {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Nome { get; set; }
    }
}
