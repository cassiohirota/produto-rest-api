using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase {

        private readonly ProdutoServices _produtoService;

        public ProdutoController(ProdutoServices produtoService) {
        
            _produtoService = produtoService;
        }

        //GET

        //Busca todos produtos
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> GetAllProdutos() {
           var listProd = await _produtoService.GetAllAsync();

            if (listProd is null || !listProd.Any()) {
                return NotFound();
            }

            return Ok(listProd);
        }

        //Busca todos produtos de uma categoria
        [HttpGet("categoria/{id:length(24)}")]
        public async Task<ActionResult<List<Produto>>> GetListProdutos(string id) { 
            var listProd = await _produtoService.GetListAsync(id);

            if (listProd is null || !listProd.Any()) { 
                return NotFound(); 
            }

            return Ok(listProd);
        }

        //Busca um produto
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Produto>> GetProduto(string id) {
            var produto = await _produtoService.GetOneAsync(id);

            if (produto is null) {
                return NotFound();
            }

            return Ok(produto);
        }

        //POST

        //Cria um produto
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto) {
            await _produtoService.CreateAsync(produto);
            return Ok(produto);
        }

        //PUT

        //Atualiza um produto
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> UpdateProduto(string id, Produto produtoAlter) {
            var produto = await _produtoService.GetOneAsync(id);

            if (produto is null) {
                return NotFound();
            }

            produtoAlter.Id = produto.Id;

            await _produtoService.UpdateAsync(id, produtoAlter);

            return NoContent();
        }
        
        //Atualiza a categoria de um produto
        [HttpPut("{idProd:length(24)}/categoria/{idCateg:length(24)}")]
        public async Task<ActionResult> UpdateCategProduto(string idProd, string idCateg) {
            var produto = await _produtoService.GetOneAsync(idProd);

            if (produto is null) {
                return NotFound();
            }

            await _produtoService.UpdateCategAsync(idCateg, produto);

            return NoContent();
        }

        //DELETE

        //Deleta um produto
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteProduto(string id) {
            var produto = await _produtoService.GetOneAsync(id);

            if (produto is null) {
                return NotFound();
            }

            await _produtoService.RemoveAsync(id);

            return NoContent();
        }

    }
}
