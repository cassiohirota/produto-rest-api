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
        //O método busca todos os produtos registrados
        public async Task<List<Produto>> GetAllProducts() => await _produtoCollection.Find(x => true).ToListAsync();
        //O método busca o produto pelo Id
        public async Task<Produto> GetOneProductById(string id) => await _produtoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        //O método lista todos os produtos que pertencem a mesma categoria
        public async Task<List<Produto>> GetListProductsByCategoryId(string id) => await _produtoCollection.Find(x => x.Categoria.Id == id).ToListAsync();
        //O método lista os produtos por nome, preço mínimo e preço máximo
        public async Task<List<Produto>> GetListProductsByFilter( string nome, decimal? precoMin, decimal? precoMax) {

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
        //O método lista os produtos por paginação
        public async Task<List<Produto>> GetListByPage(int? page, int? pageSize) {
            
            if (page is null) { page = 1; }
            if (pageSize is null) { pageSize = 10; }

            var listProd = await _produtoCollection.Find(x => true).ToListAsync();

            var listpage = listProd.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            return listpage;
        }

        //POST
        //O método registra um produto
        public async Task CreateOneProduct(Produto produto) => await _produtoCollection.InsertOneAsync(produto);

        //PUT
        //O método altera os dados de um produto
        public async Task UpdateOneProductById(string id, Produto produto) => await _produtoCollection.ReplaceOneAsync(x => x.Id == id, produto);
        //O método altera a categoria de um produto
        public async Task UpdateCategOfOneProduct(string id, Produto produto) {

            Categoria categ = await _categoriaCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            produto.Categoria = categ;
            await _produtoCollection.ReplaceOneAsync(x => x.Id == produto.Id, produto);
        }

        //DELETE
        //O método exclui um produto
        public async Task RemoveOneProduct(string id) => await _produtoCollection.DeleteOneAsync(x => x.Id == id);
    }

}
