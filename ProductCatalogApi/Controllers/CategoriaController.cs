using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers {
    [Route("v1/[controller]")]
    [ApiController]
    public class categoriasController : ControllerBase {

        private readonly CategoriaServices _categoriaService;

        public categoriasController(CategoriaServices categoriaService) {

            _categoriaService = categoriaService;
        }

        //GET

        /// <summary>
        /// Lista todas as categorias registradas
        /// </summary>
        /// <returns> Retorna todas as categorias registradas </returns>
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetAllCategorias() {
            var listCateg = await _categoriaService.GetAllAsync();

            if (listCateg is null || !listCateg.Any()) {
                return NotFound();
            }
            return Ok(listCateg);
        }

        /// <summary>
        /// Busca uma categoria
        /// </summary>
        /// <returns> Retorna a categoria encontrada </returns>
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Categoria>> GetCategoria(string id) {
            var categoria = await _categoriaService.GetOneAsync(id);

            if (categoria is null) {
                return NotFound();
            }

            return Ok(categoria);
        }

        //POST

        /// <summary>
        /// Cria uma categoria
        /// </summary>
        /// <returns> Retorna a categoria registrada </returns>
        [HttpPost]
        public async Task<ActionResult> PostCategoria(Categoria categoria) {
            await _categoriaService.CreateAsync(categoria);

            return CreatedAtAction(nameof(GetAllCategorias), new { id = categoria.Id }, categoria);
        }

        //PUT

        /// <summary>
        /// Altera os dados de uma categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> UpdateCategoria(string id, Categoria categoriaAlter) {
            var categoria = await _categoriaService.GetOneAsync(id);

            if (categoria is null) {
                return NotFound();
            }

            categoriaAlter.Id = categoria.Id;

            await _categoriaService.UpdateAsync(id, categoriaAlter);

            return NoContent();
        }

        //DELETE

        /// <summary>
        /// Exclui uma categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteCategoria(string id) {
            var categoria = await _categoriaService.GetOneAsync(id);

            if (categoria is null) {
                return NotFound();
            }

            await _categoriaService.RemoveAsync(id);

            return NoContent();
        }
    }
}
