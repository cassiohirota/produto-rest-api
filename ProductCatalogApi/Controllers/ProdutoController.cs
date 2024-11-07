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
        public async Task<List<Produto>> GetAllProdutos() => await _produtoService.GetAsync();

        //Busca um produto
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Produto>> GetProduto(string id) {
            var produto = await _produtoService.GetAsync(id);

            if (produto is null) {
                return NotFound();
            }

            return produto;
        }

        //POST

        //Cria categoria
        [HttpPost]
        public async Task<Produto> PostProduto(Produto produto) {
            await _produtoService.CreateAsync(produto);
            return produto;
        }

        //PUT

        //Atualiza uma categoria
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateProduto(string id, Produto produtoAlter) {
            var categoria = await _produtoService.GetAsync(id);

            if (categoria is null) {
                return NotFound();
            }

            produtoAlter.Id = categoria.Id;

            await _produtoService.UpdateAsync(id, produtoAlter);

            return NoContent();
        }

        //DELETE

        //Deleta uma categoria
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteProduto(string id) {
            var categoria = await _produtoService.GetAsync(id);

            if (categoria is null) {
                return NotFound();
            }

            await _produtoService.RemoveAsync(id);

            return NoContent();
        }

    }
}
