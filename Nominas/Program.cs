using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Program
    {
        static void Main(string[] args)
        {

            Ficheros.crearTrabajadores("77181313Y", "Fran", "Romero");
            Ficheros.CrearTxtNomina();
            Console.ReadLine();
        }
    }
}
