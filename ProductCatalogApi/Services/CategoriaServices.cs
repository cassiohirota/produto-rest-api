using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Services {
    public class CategoriaServices {

        private readonly IMongoCollection<Categoria> _categoriaCollection;

        public CategoriaServices(IOptions<ProdutoDatabaseSettings> catServices) {

            var mongoClient = new MongoClient(catServices.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(catServices.Value.DatabaseName);

            _categoriaCollection = mongoDatabase.GetCollection<Categoria>(catServices.Value.CategoriaCollectionName);
        }

        //GET
        //O método busca todas as categorias registradas
        public async Task<List<Categoria>> GetAllCategories() => await _categoriaCollection.Find(x => true).ToListAsync();

        //O método busca a categoria pelo Id
        public async Task<Categoria> GetOneCategoryById(string id) => await _categoriaCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        //POST
        //O método registra uma categoria
        public async Task CreateOneCategory(Categoria categoria) => await _categoriaCollection.InsertOneAsync(categoria);

        //PUT
        //O método altera os dados de um categoria
        public async Task UpdateOneCategory(string id, Categoria categoria) => await _categoriaCollection.ReplaceOneAsync(x => x.Id == id, categoria);

        //DELETE
        //O método exclui uma categoria
        public async Task RemoveOneCategory(string id) => await _categoriaCollection.DeleteOneAsync(x => x.Id == id);
    }
}
