using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario_1.Models
{
    public class Producto 
    {
        [Required(ErrorMessage = "Title is required")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "Title is required")]

        public string Nombre { get; set; }
        [Required(ErrorMessage = "Title is required")]

        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Title is required")]

        public int Cant_Productos { get; set; }
        public IFormFile FileC { get; set; }

    }
}
