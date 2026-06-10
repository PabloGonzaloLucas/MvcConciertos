using Microsoft.AspNetCore.Mvc;
using MvcConciertos.Services;

namespace MvcConciertos.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceConciertos service;

        public ConciertosController(ServiceConciertos service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index(int? idCategoria)
        {
            var categorias = await service.GetCategoriasAsync();
            ViewBag.Categorias = categorias;
            if (idCategoria.HasValue)
            {
                var conciertosCategoria = await service.GetEventosCategoriaAsync(idCategoria.Value);

                return View(conciertosCategoria);
            }
            var conciertos = await service.GetEventosAsync();
            return View(conciertos);
        }


    }
}
