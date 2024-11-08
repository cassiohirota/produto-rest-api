using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Services {
    public class ProdutoServices {

        private readonly IMongoCollection<Produto> _produtoCollection;
        private readonly IMongoCollection<Categoria> _categoriaCollection;

        public ProdutoServices(IOptions<ProdutoDatabaseSettings> prodServices) {

            var mongoClient = new MongoClient(prodServices.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(prodServices.Value.DatabaseName);

            _produtoCollection = mongoDatabase.GetCollection<Produto>(prodServices.Value.ProdutoCollectionName);
            _categoriaCollection = mongoDatabase.GetCollection<Categoria>(prodServices.Value.CategoriaCollectionName);
        }

        //GET
        public async Task<List<Produto>> GetAllAsync() => await _produtoCollection.Find(x => true).ToListAsync();
        public async Task<Produto> GetOneAsync(string id) => await _produtoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<List<Produto>> GetListAsync(string id) => await _produtoCollection.Find(x => x.Categoria.Id == id).ToListAsync();

        //POST
        public async Task CreateAsync(Produto produto) => await _produtoCollection.InsertOneAsync(produto);

        //PUT
        public async Task UpdateAsync(string id, Produto produto) => await _produtoCollection.ReplaceOneAsync(x => x.Id == id, produto);
        public async Task UpdateCategAsync(string id, Produto produto) {

            Categoria categ = await _categoriaCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            produto.Categoria = categ;
            await _produtoCollection.ReplaceOneAsync(x => x.Id == produto.Id, produto);
        } 

        //DELETE
        public async Task RemoveAsync(string id) => await _produtoCollection.DeleteOneAsync(x => x.Id == id);
    }

}
