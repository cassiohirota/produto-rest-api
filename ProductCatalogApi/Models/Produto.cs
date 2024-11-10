using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductCatalogApi.Models {
    public class Produto {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Nome { get; set; } = null;
        public string Desc { get; set; }
        public string Marca { get; set; } 
        public Categoria Categoria { get; set; }
        public int Qtd { get; set; }
        public decimal Preco { get; set; }
    }
}
