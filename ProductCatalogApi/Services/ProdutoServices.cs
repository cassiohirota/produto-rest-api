using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogApi.Models;
using System.Linq;

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
        public async Task<List<Produto>> GetAllFilteredAsync( string nome, decimal? precoMin, decimal? precoMax) {

            var listProd = await _produtoCollection.Find(x => true).ToListAsync();

            if (!string.IsNullOrWhiteSpace(nome)) {
                listProd = listProd.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();
            }
            if (precoMin.HasValue) {
                listProd = listProd.Where(x => x.Preco >= precoMin).ToList();
            }
            if (precoMax.HasValue) {
                listProd = listProd.Where(x => x.Preco <= precoMax).ToList();
            }

            return listProd;
        }

        public async Task<List<Produto>> GetAllByPageAsync(int? page, int? pageSize) {
            
            if (page is null) { page = 1; }
            if (pageSize is null) { pageSize = 10; }

            var listProd = await _produtoCollection.Find(x => true).ToListAsync();

            var listpage = listProd.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            return listpage;
        }

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
