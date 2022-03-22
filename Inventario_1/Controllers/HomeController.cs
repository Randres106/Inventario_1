using Inventario_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Inventario_1.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Inventario_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment hostingEnvironment;
        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Ingresar(IFormCollection collection)
        {
            ViewData["Existe"] = default;
            bool Existe = false;
            try
            {
                var NewCustomer = new Models.Producto
                {
                    Codigo = collection["Codigo"],
                    Nombre = collection["Nombre"],
                    Descripcion = collection["Descripcion"],
                    Cant_Productos = Convert.ToInt32(collection["Cant_Productos"])
                };
                if (NewCustomer.Codigo != null)
                {
                    for (int i = 0; i < Singleton.Instance.ListCliente.Count; i++)
                    {
                        if (Singleton.Instance.ListCliente.ElementAt(i).Codigo == NewCustomer.Codigo)
                        {
                            Existe = true;
                        }
                    }
                    if (Existe)
                    {
                        ViewData["Existe"] = "Ya existe un valor con ese código";
                    }
                    else
                    {
                        Singleton.Instance.ListCliente.AddLast(NewCustomer);
                    }
                }
                return View();

            }
            catch
            {
                return View();
            }
        }
        public IActionResult UploadFilecsv()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadFilecsv(Producto model)
        {
            string uniqueFileName = null;
            if (model.FileC != null)
            {
                string uploadsfolder = Path.Combine(hostingEnvironment.WebRootPath, "Upload");
                uniqueFileName = model.FileC.FileName;
                string filepath = Path.Combine(uploadsfolder, uniqueFileName);
                if (!System.IO.File.Exists(filepath))
                {
                    using (var iNeedToLearnAboutDispose = new FileStream(filepath, FileMode.CreateNew))
                    {
                        model.FileC.CopyTo(iNeedToLearnAboutDispose);
                    }
                }
                string ccc = System.IO.File.ReadAllText(filepath);
                foreach (string row in ccc.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (row.Split(',')[0] != "Codigo")
                        {
                            Singleton.Instance.ListCliente.AddLast(new Producto
                            {
                                Codigo= row.Split(',')[0],
                                Nombre = row.Split(',')[1],
                                Descripcion = row.Split(',')[2],
                                Cant_Productos = Convert.ToInt32(row.Split(',')[3])
                            });
                        }

                    }
                }
                return RedirectToAction("Lista_Cliente");
            }
            return View();
        }
            public async Task<IActionResult> Lista_Cliente(string BTipo, string Bbuscar)
        {
            Singleton.Instance.ListaBusqueda.Clear();
            switch (BTipo)
            {
                case "Código":
                    for (int i = 0; i < Singleton.Instance.ListCliente.Count; i++)
                    {
                        if (Singleton.Instance.ListCliente.ElementAt(i).Codigo == Bbuscar)
                        {
                            Singleton.Instance.ListaBusqueda.AddLast(Singleton.Instance.ListCliente.ElementAt(i));
                        }
                    }
                    return View(Singleton.Instance.ListaBusqueda);

                case "Nombre":
                    for (int i = 0; i < Singleton.Instance.ListCliente.Count; i++)
                    {
                        if (Singleton.Instance.ListCliente.ElementAt(i).Nombre == Bbuscar)
                        {
                            Singleton.Instance.ListaBusqueda.AddLast(Singleton.Instance.ListCliente.ElementAt(i));
                        }
                    }
                    return View(Singleton.Instance.ListaBusqueda);
                case "Cantidad":
                    for (int i = 0; i < Singleton.Instance.ListCliente.Count; i++)
                    {
                        if (Singleton.Instance.ListCliente.ElementAt(i).Cant_Productos.ToString() == Bbuscar)
                        {
                            Singleton.Instance.ListaBusqueda.AddLast(Singleton.Instance.ListCliente.ElementAt(i));
                        }
                    }
                    return View(Singleton.Instance.ListaBusqueda);
            }
            return View(Singleton.Instance.ListCliente);
        }
        public IActionResult Edit(string Codigo)
        {
            var Edit = Singleton.Instance.ListCliente.FirstOrDefault(x => x.Codigo == Codigo);
            return View(Edit);
        }
        [HttpPost]
        public IActionResult EditConfirm(string Codigo, IFormCollection collection)
        {
            try
            {
                var NewCustomer = new Models.Producto
                {
                    Codigo = collection["Codigo"],
                    Nombre = collection["Nombre"],
                    Descripcion = collection["Descripcion"],
                    Cant_Productos = Convert.ToInt32(collection["Cant_Productos"])
                };
                Singleton.Instance.ListCliente.Remove(Singleton.Instance.ListCliente.FirstOrDefault(x => x.Codigo == Codigo));
                Singleton.Instance.ListCliente.AddLast(NewCustomer);
                return RedirectToAction("Lista_Cliente");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult Delete(string Codigo)
        {
            var Edit = Singleton.Instance.ListCliente.FirstOrDefault(x => x.Codigo == Codigo);
            return View(Edit);
        }
        [HttpPost]
        public IActionResult DeleteCofirm(string Codigo)
        {
            try
            {
                
                Singleton.Instance.ListCliente.Remove(Singleton.Instance.ListCliente.FirstOrDefault(x => x.Codigo == Codigo));
                return RedirectToAction("Lista_Cliente");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
