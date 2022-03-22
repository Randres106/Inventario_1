using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventario_1.Models;

namespace Inventario_1.Helpers
{
    public class Singleton
    {
        private static Singleton _instance = null;
        public static Singleton Instance 
        {
            get 
            {
                if (_instance == null) _instance = new Singleton();
                return _instance;
            }
        }
        public LinkedList<Producto> ListCliente = new LinkedList<Producto>();
        public LinkedList<Producto> ListaBusqueda = new LinkedList<Producto>();
    }
}
