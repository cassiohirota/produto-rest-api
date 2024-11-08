using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductCatalogApi.Models;
using ProductCatalogApi.Services;

namespace ProductCatalogApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase {

        private readonly CategoriaServices _categoriaService;

        public CategoriaController(CategoriaServices categoriaService) {

            _categoriaService = categoriaService;
        }

        //GET

        //Busca todas as categorias
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetAllCategorias() {
            var listCateg = await _categoriaService.GetAllAsync();

            if (listCateg is null || !listCateg.Any()) {
                return NotFound();
            }
            return Ok(listCateg);
        }

        //Busca uma categoria
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Categoria>> GetCategoria(string id) {
            var categoria = await _categoriaService.GetOneAsync(id);

            if (categoria is null) {
                return NotFound();
            }
            
            return Ok(categoria);
        }
        
        //POST

        //Cria categoria
        [HttpPost]
        public async Task<ActionResult> PostCategoria(Categoria categoria) {
            await _categoriaService.CreateAsync(categoria);

            return CreatedAtAction(nameof(GetAllCategorias), new { id = categoria.Id }, categoria);
        }

        //PUT

        //Atualiza uma categoria
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

        //Deleta uma categoria
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
