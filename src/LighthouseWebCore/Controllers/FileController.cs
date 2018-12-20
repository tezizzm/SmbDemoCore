using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LighthouseUiCore.Controllers
{
    public class FileController : Controller
    {
        private readonly ISmbService _smbService;

        public FileController(ISmbService smbService)
        {
            _smbService = smbService;
        }

        // GET: Operations
        public ActionResult Index()
        {
            var files = _smbService.GetFiles();
            return View(files);
        }

        // GET: Operations/Details/5
        public async Task<ActionResult> Details(string name)
        {
            var file = await _smbService.GetFileAsync(name);
            if (file?.Name == null)
            {
                return NotFound();
            }
            return View(file);
        }

        // GET: Operations/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Operations/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormFile file)
        {
            try
            {
                Console.WriteLine($"IFormFile == {file == null}");
                await _smbService.CreateFileAsync(file);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: Operations/Delete/5
        public async Task<ActionResult> Delete(string name)
        {
            var file = await _smbService.GetDeleteModel(name);
            if (file?.Name == null)
            {
                return NotFound();
            }
            return View(file);
        }

        // POST: Operations/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(string name, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _smbService.DeleteFile(name);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }
    }
}