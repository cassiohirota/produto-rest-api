using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Services {
    public class ProdutoServices {

        private readonly IMongoCollection<Produto> _produtoCollection;

        public ProdutoServices(IOptions<ProdutoDatabaseSettings> prodServices) {

            var mongoClient = new MongoClient(prodServices.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(prodServices.Value.DatabaseName);

            _produtoCollection = mongoDatabase.GetCollection<Produto>(prodServices.Value.ProdutoCollectionName);
        }

        //GET
        public async Task<List<Produto>> GetAsync() => await _produtoCollection.Find(x => true).ToListAsync();

        public async Task<Produto> GetAsync(string id) => await _produtoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        //POST
        public async Task CreateAsync(Produto produto) => await _produtoCollection.InsertOneAsync(produto);

        //PUT
        public async Task UpdateAsync(string id, Produto produto) => await _produtoCollection.ReplaceOneAsync(x => x.Id == id, produto);

        //DELETE
        public async Task RemoveAsync(string id) => await _produtoCollection.DeleteOneAsync(x => x.Id == id);
    }

}
