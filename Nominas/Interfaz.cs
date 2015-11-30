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
        #region Menus PRINCIPALES - Francisco Romero
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
                Console.WriteLine("\t\t\t1 -> Operaciones trabajadores");
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
                Console.WriteLine("\t\t\t5 -> Mostrar Trabajadores");
                Console.WriteLine("\t\t\t6 -> Salir\n");
                Console.Write("\t\t\tEleccion: ");
                eleccion = Console.ReadLine();
                eleccion = eleccion.Trim();

                if (Byte.TryParse(eleccion, out seleccion) && (seleccion >= 1) && (seleccion <= 5))
                {
                    GestionNegocio.GestionOperaciones(Int32.Parse(eleccion), ref flag);
                }
                else
                {
                    fail = true;
                    msg = "Opción Incorrecta (seleccione una opción del menú: 1 - 5)";
                }

            } while (!flag);
        }
        #endregion

        #region Mostrar Error - Francisco Romero
        public static void Error(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\t\tERROR >> {0}", err);
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
        #region Pedir <Modificar Contraseña> - Francisco Romero
        public static string PedirContraseñaModificar()
        {
            string passactual = null, passnueva = null;
            bool salir = false;
            do
            {
                Header();
                Console.WriteLine("Introduce la contraseña actual");
                passactual = Console.ReadLine();
                if (GestionNegocio.ValidarContraseña(passactual))
                {
                    Console.WriteLine("Introduce la contraseña nueva");
                    passnueva = Console.ReadLine();
                }
                else
                {
                    Error("La contraseña actual no es correcta, por favor, intentelo de nuevo.");
                }
            } while (!salir);
            return passnueva;
        }
        #endregion
        #region Continuar
        public static void Continuar()
        {
            Console.WriteLine("");
            Console.Write("\t\t\tPulsa una tecla para continuar...");
            Console.ReadLine();
        }
        //Método sobrescrito de continuar, solo para probar
        public static bool Continuar(string message)
        {
            bool seguir = false;    // Control de la confirmación
            string aux = null;

            // ENTRADA
            Console.Write("\n\t\t" + message);
            aux = Console.ReadLine();
            aux = aux.Trim().ToLower(); // Métodos en cadena: 1º limpia ; 2º minúsculas

            // VALIDACION --> PENDIENTE

            // PROCESAMIENTO
            if ((aux != "") && (aux[0] == 's')) seguir = true;   // Sólo true si 's', false en resto de casos

            // SALIDA
            return seguir;
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
        #region Operaciones Usuario
        public static Trabajador PlantillaCrearTrabajador()
        {
            Trabajador trb = null;
            bool salir = false;
            do
            {
                Header();
                trb = new Trabajador();
                Console.Write("\n\t\t\tIntroduce el DNI: ");
                trb.dni_pre = Console.ReadLine();
                Console.Write("\t\t\tIntroduce el Nombre: ");
                trb.nombre_pre = Console.ReadLine();
                Console.Write("\t\t\tIntroduce los apellidos: ");
                trb.apellidos_pre = Console.ReadLine();
                return trb;

            } while (!salir);
        }
        public static string PlantillaPedirDni()
        {
            string dni = null;
            bool salir = false;
            do
            {
                Interfaz.Header();
                Console.WriteLine("\t\tA continuacion, introduce el DNI del empleado.");
                Console.Write("\n\t\t\tIntroduce el DNI: ");
                dni = Console.ReadLine();
                salir = true;
            } while (!salir);
            return dni;
        }
        public static bool Pregunta(ref string pregunta)
        {
            bool salir = false;
            do
            {
                string eleccion = null;
                Console.Write("\n\t\t " + pregunta);
                eleccion = Console.ReadLine();
                if (eleccion.Equals("s"))
                {
                    salir = false;
                }
                else if (eleccion.Equals("n"))
                {
                    salir = true;
                }
                else
                {
                    Error("Introduce una S para borrar otro empleado o una N para salir.");
                    Continuar();
                }
            } while (!salir);
            return salir;
        }

        #endregion
        #region
        public static String PlantillaEleccionModificar()
        {
            string eleccion = null;
            byte seleccion = 0;
            bool salir = false;
            do
            {
                Console.Clear();
                Interfaz.Header();
                Console.WriteLine("\n\t\t¿Que desea modificar?.");
                Console.WriteLine("\n\t\t1.Modificar DNI.");
                Console.WriteLine("\n\t\t2.Modificar Nombre.");
                Console.WriteLine("\n\t\t3.Modificar Apellidos.");
                Console.Write("\n\t\t\tIntroduce tu elección: ");
                eleccion = Console.ReadLine();

                if (Byte.TryParse(eleccion, out seleccion) && (seleccion > 0) && (seleccion <= 4))
                {
                    salir = true;
                }
                else
                {
                    Error("Introduce una elección del 1 al 3");
                    Continuar();
                }
            } while (!salir);
            return eleccion;
        }

        public static String ElementoModificar(string eleccion)
        {
            bool salir = false;
            byte seleccion = 0;
            string cambio = null;
            do
            {
                Header();
                switch (eleccion)
                {

                    case "1":
                        Console.Write("\n\t\tIntroduce el DNI nuevo: ");
                        cambio = Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("\n\t\tIntroduce el Nombre nuevo: ");
                        cambio = Console.ReadLine();
                        break;
                    case "3":
                        Console.Write("\n\t\tIntroduce el Apellido nuevo: ");
                        cambio = Console.ReadLine();
                        break;
                }
                if (Byte.TryParse(eleccion, out seleccion) && (seleccion > 0) && (seleccion <= 3))
                {
                    salir = true;
                }
                else
                {
                    Error("Introduce una elección del 1 al 3");
                    Continuar();
                }
            } while (!salir);
            return cambio;
        }

        #endregion
        #region Metodo ListarTrabajadores(Todavia no implementado) - Óscar
        public static void MostrarLista(Trabajador[] listaTrabajadores)
        {
            int indice = 0;

            Header();
            Console.WriteLine("\t LISTADO DE TRABAJADORES");
            HeaderVerTrabajadores();

            for (indice = 0; indice < listaTrabajadores.Length; indice++)
            {
                Console.WriteLine(listaTrabajadores[indice]);
            }
        }
        #endregion

    }
}
