using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<List<Categoria>> GetAllCategorias() => await _categoriaService.GetAsync();

        //Busca uma categoria
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Categoria>> GetCategoria(string id) {
            var categoria = await _categoriaService.GetAsync(id);

            if (categoria is null) {
                return NotFound();
            }

            return categoria;
        }

        //POST

        //Cria categoria
        [HttpPost]
        public async Task<IActionResult> PostCategoria(Categoria categ) {
            await _categoriaService.CreateAsync(categ);

            return CreatedAtAction(nameof(GetAllCategorias), new { id = categ.Id }, categ);
        }

        //PUT

        //Atualiza uma categoria
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateCategoria(string id, Categoria categoriaAlter) {
            var categoria = await _categoriaService.GetAsync(id);

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
        public async Task<IActionResult> DeleteCategoria(string id) {
            var categoria = await _categoriaService.GetAsync(id);

            if (categoria is null) {
                return NotFound();
            }

            await _categoriaService.RemoveAsync(id);

            return NoContent();
        }
    }
}
