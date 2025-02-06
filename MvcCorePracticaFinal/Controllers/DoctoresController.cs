using Microsoft.AspNetCore.Mvc;
using MvcCorePracticaFinal.Models;
using MvcCorePracticaFinal.Repositories;

namespace MvcCorePracticaFinal.Controllers
{
    public class DoctoresController : Controller
    {
        RepositoryDoctores repo;

        public DoctoresController()
        {
            this.repo = new RepositoryDoctores();
        }
        public IActionResult Index()
        {
            List<Doctor> d = this.repo.GetDoctores();
            return View(d);
        }

        public IActionResult Details(int numeroDoc)
        {
            Doctor d = this.repo.FindDoctor(numeroDoc);
            return View(d);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int hospitalCod, string apellido, string especialidad, int salario)
        {
            await this.repo.InsertDoctorAsync(hospitalCod, apellido, especialidad, salario);
            return RedirectToAction("Index");
        }

        public IActionResult BuscadorDoctores()
        {
            ViewData["ESPECIALIDAD"] = this.repo.GetEspecialidadDoctores();
            return View();
        }

        [HttpPost]
        public IActionResult BuscadorDoctores(string especialidad)
        {
            ViewData["ESPECIALIDAD"] = this.repo.GetEspecialidadDoctores();
            List<Doctor> d= this.repo.GetDoctoresEspecialidad(especialidad);
            return View(d);
        }

        public IActionResult Delete(int numeroDoc)
        {
            this.repo.DeleteDoctor(numeroDoc);
            return RedirectToAction("Index");
        }
    }
}
