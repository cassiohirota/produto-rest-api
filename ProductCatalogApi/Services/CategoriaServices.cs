using Microsoft.Extensions.Options;
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
        public async Task<List<Categoria>> GetAsync() => await _categoriaCollection.Find(x => true).ToListAsync();

        public async Task<Categoria> GetAsync(string id) => await _categoriaCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        //POST
        public async Task CreateAsync(Categoria categoria) => await _categoriaCollection.InsertOneAsync(categoria);

        //PUT
        public async Task UpdateAsync(string id, Categoria categoria) => await _categoriaCollection.ReplaceOneAsync(x => x.Id == id, categoria);
        
        //DELETE
        public async Task RemoveAsync(string id) => await _categoriaCollection.DeleteOneAsync(x => x.Id == id);
    }
}
