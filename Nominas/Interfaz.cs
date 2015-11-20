using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Interfaz
    {
        #region Header - Francisco Romero
        public static void Header()
        {
            Console.Clear();
            Console.WriteLine("\t\t|>-----------------------------------------<|");
            Console.WriteLine("\t\t|                                           |");
            Console.WriteLine("\t\t|              GESTION DE NOMINAS           |");
            Console.WriteLine("\t\t|                                           |");
            Console.WriteLine("\t\t|>-----------------------------------------<|");
        }
        #endregion Header
        #region Menu Principal - Francisco Romero
        public static byte MenuPrincipal()
        {
            byte seleccion = 0;
            bool fail = false;
            bool salir = false;  // Control de datos correctos
            string aux = null;
            string msg = null;

            do
            {
                Header();
                if (fail)
                {
                    Error(msg);
                }
                Console.WriteLine("\n\t\t\t1 -> Ver trabajadores");
                Console.WriteLine("\t\t\t2 -> Operaciones trabajadores");
                Console.WriteLine("\t\t\t3 -> Salir\n");
                Console.Write("\t\t\tEleccion: ");
                aux = Console.ReadLine();
                aux = aux.Trim(); 

                if (Byte.TryParse(aux, out seleccion) && (seleccion >= 1) && (seleccion <= 3))
                {
                    salir = true;
                }
                else
                {
                    fail = true;
                    msg = "Opción Incorrecta (seleccione una opción del menú: 1 - 3)";
                }

            } while (!salir);

            return seleccion;
        }
        #endregion Menu Principal
        #region SubMenu Operaciones
        public static void Operaciones()
        {
            byte seleccion = 0;
            bool fail = false;
            bool flag = false;  // Control de datos correctos
            string eleccion = null;
            string msg = null;

            do
            {
                Header();
                if (fail)
                {
                    Error(msg);
                }
                Console.WriteLine("\n\t\t\t1 -> Agregar Trabajadores");
                Console.WriteLine("\t\t\t2 -> Modificar Trabajadores ");
                Console.WriteLine("\t\t\t3 -> Eliminar Trabajadores ");
                Console.WriteLine("\t\t\t4 -> Modificar Contraseña ");
                Console.WriteLine("\t\t\t5 -> Salir\n");
                Console.Write("\t\t\tEleccion: ");
                eleccion = Console.ReadLine();
                eleccion = eleccion.Trim();

                if (Byte.TryParse(eleccion, out seleccion) && (seleccion >= 1) && (seleccion <= 5))
                {
                    flag = true;
                }
                else
                {
                    fail = true;
                    msg = "Opción Incorrecta (seleccione una opción del menú: 1 - 5)";
                }

            } while (!flag);
        }
        #endregion SubMenu Operaciones
        #region Mostrar Error - Francisco Romero
        public static void Error(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\tERROR >> {0}", err);
            Console.ResetColor();
        }
        #endregion Mostrar Error
        #region Pedir Contraseña - Francisco Romero
        public static string PedirContraseña()
        {
            bool salir = false;
            string password = null;
            do
            {
                Header();
                Console.Write("\t\tIntroduzca la contraseña: ");
                password = Console.ReadLine();
                if (GestionNegocio.ValidarContraseña(password))
                {
                    salir = true;
                }
                else
                {
                    Error("La contraseña introducida es incorrecta, introduzca una contraseña de 3 - 6 carácteres.");
                    Continuar();
                    salir = false;
                }
            } while (!salir);
            return password;
        }
        #endregion
        #region Continuar
        public static void Continuar()
        {
            Console.WriteLine("");
            Console.Write("\t\t\tPulsa una tecla para continuar...");
            Console.ReadLine();
        }
        #endregion
        #region Header Tabla Trabajadores - Francisco Romero
        public static void HeaderVerTrabajadores()
        {
            Console.WriteLine("---------------------------------------------------".PadLeft(64));
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("|     DNI    |     NOMBRE      |  APELLIDOS       |".PadLeft(64));
            Console.ResetColor();
            Console.WriteLine("---------------------------------------------------".PadLeft(64));
            Console.WriteLine("");
        }
        #endregion Header Tabla Trabajadores
        #region Formato de Salida VER TRABAJADORES - Francisco Romero
        public static void FormatoLeerXML(string dni, string nombre, string apellidos)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════".PadLeft(64));
            Console.WriteLine(string.Format("\t     ║ {0,-9} | {1,15} | {2,10}", dni, nombre, apellidos).PadRight(70));
            Console.WriteLine("╚══════════════════════════════════════════════════".PadLeft(64));
            Console.WriteLine("");
        }
        #endregion Formato de Salida VER TRABAJADORES
    }
}
