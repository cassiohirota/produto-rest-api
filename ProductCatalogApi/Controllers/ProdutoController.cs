using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers {
    [Route("v1/[controller]")]
    [ApiController]
    public class produtosController : ControllerBase {

        private readonly ProdutoServices _produtoService;

        public produtosController(ProdutoServices produtoService) {

            _produtoService = produtoService;
        }

        //GET

        /// <summary>
        /// Lista todos os produtos
        /// </summary>
        /// <returns> Retorna todos os produtos registrados </returns>
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> GetAllProdutos() {
            var listProd = await _produtoService.GetAllProducts();

            if (listProd is null || !listProd.Any()) {
                return NotFound();
            }

            return Ok(listProd);
        }


        /// <summary>
        /// Lista todos os produtos de uma categoria
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <returns> Retorna todos os produtos de uma categoria </returns>
        [HttpGet("categorias/{categoriaId:length(24)}")]
        public async Task<ActionResult<List<Produto>>> GetListProdutos(string categoriaId) {
            var listProd = await _produtoService.GetListProductsByCategoryId(categoriaId);

            if (listProd is null || !listProd.Any()) {
                return NotFound();
            }

            return Ok(listProd);
        }
        
        /// <summary>
        /// Lista todos os produtos por filtragem
        /// </summary>
        /// <returns> Retorna todos os produtos filtrados </returns>
        [HttpGet("filter")]
        public async Task<ActionResult<List<Produto>>> GetAllFilteredProdutos([FromQuery] string nome = null, [FromQuery] decimal? precoMin = null, [FromQuery] decimal? precoMax = null) {

            if (precoMin.HasValue && precoMax.HasValue && precoMin > precoMax) {
                return BadRequest("O preço mínimo não pode ser maior que o preço máximo");
            }

            var listProd = await _produtoService.GetListProductsByFilter(nome, precoMin, precoMax);

            if (listProd is null || !listProd.Any()) {
                return NotFound();
            }

            return Ok(listProd);
        }
        /// <summary>
        /// Lista todos os produtos por paginação
        /// </summary>
        /// <returns> Retorna todos os produtos por paginação </returns>
        [HttpGet("query")]
        public async Task<ActionResult<List<Produto>>> GetAllByPageProdutos([FromQuery] int? page = null, [FromQuery] int? pageSize = null) {

            if (page.HasValue && pageSize.HasValue && page > pageSize) {
                return BadRequest("O número da página ultrapassou o número de itens permitido por página");
            }

            var listProd = await _produtoService.GetListByPage(page, pageSize);

            if (listProd is null || !listProd.Any()) {
                return NotFound();
            }

            return Ok(listProd);
        }


        /// <summary>
        /// Busca um produto
        /// </summary>
        /// <param name="produtoId"></param>
        /// <returns> Retorna o produto encontrado </returns>
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Produto>> GetProduto(string produtoId) {
            var produto = await _produtoService.GetOneProductById(produtoId);

            if (produto is null) {
                return NotFound();
            }

            return Ok(produto);
        }

        //POST

        /// <summary>
        /// Cria um produto
        /// </summary>
        /// <returns> Retorna o produto registrada </returns>
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto) {
            await _produtoService.CreateOneProduct(produto);
            return Ok(produto);
        }

        //PUT

        /// <summary>
        /// Atualiza um produto
        /// </summary>
        /// <param name="produtoId"></param>
        /// <returns></returns>
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> UpdateProduto(string produtoId, Produto produtoAlter) {
            var produto = await _produtoService.GetOneProductById(produtoId);

            if (produto is null) {
                return NotFound();
            }

            produtoAlter.Id = produto.Id;

            await _produtoService.UpdateOneProductById(produtoId, produtoAlter);

            return NoContent();
        }

        /// <summary>
        /// Atualiza a categoria de um produto
        /// </summary>
        /// <param name="produtoId"></param>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        [HttpPut("{produtoId:length(24)}/categorias/{categoriaId:length(24)}")]
        public async Task<ActionResult> UpdateCategProduto(string produtoId, string categoriaId) {
            var produto = await _produtoService.GetOneProductById(produtoId);

            if (produto is null) {
                return NotFound();
            }

            await _produtoService.UpdateCategOfOneProduct(categoriaId, produto);

            return NoContent();
        }

        //DELETE

        /// <summary>
        /// Deleta um produto
        /// </summary>
        /// <param name="produtoId"></param>
        /// <returns></returns>
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteProduto(string produtoId) {
            var produto = await _produtoService.GetOneProductById(produtoId);

            if (produto is null) {
                return NotFound();
            }

            await _produtoService.RemoveOneProduct(produtoId);

            return NoContent();
        }

    }
}
