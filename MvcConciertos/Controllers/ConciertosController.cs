using Microsoft.AspNetCore.Mvc;
using MvcConciertos.Services;

namespace MvcConciertos.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceConciertos service;
        private ServiceLambdaIA serviceIA;
        public ConciertosController(ServiceConciertos service, ServiceLambdaIA serviceIA)
        {
            this.service = service;
            this.serviceIA = serviceIA;
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

        [HttpPost]
        public async Task<IActionResult> GetRespuestaIA(string pregunta)
        {
            string respuesta = await serviceIA.GetRespuestaIAAsync(pregunta);
            return Json(respuesta);
        }
    }
}
