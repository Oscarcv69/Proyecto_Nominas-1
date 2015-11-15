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
            #region Ejecutar Menu Principal
            byte opcion = 0;
            bool salir = false;

            do
            {
                opcion = Interfaz.MenuPrincipal();

                switch (opcion)
                {
                    // Ver trabajadores
                    case 1:
                        Ficheros.getTrabajadores();
                        Console.ReadLine();
                        salir = false;
                        break;
                    // Operar trabajadores
                    case 2:
                        Console.WriteLine("En construccion..");
                        salir = true;
                        break;
                }
            } while (!salir);
            #endregion Fin Ejecutar
        }
    }
}
