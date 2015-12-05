using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Nominas
{
    class Interfaz
    {
        private static string dni_trb = null;
        
        #region UTILIDADES - Francisco Romero
        public static void Header()
        {
            Console.Clear();
            Console.WriteLine("\t\t|>-----------------------------------------<|");
            Console.WriteLine("\t\t|                                           |");
            Console.WriteLine("\t\t|              GESTION DE NOMINAS           |");
            Console.WriteLine("\t\t|                                           |");
            Console.WriteLine("\t\t|>-----------------------------------------<|");
        }
        public static void Error(string err) // MÉTODO QUE MUESTRA EL ERROR RECIBIDO POR PANTALLA
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\t\tERROR >> {0}", err);
            Console.ResetColor();
        }
        public static void Continuar() // Método por defecto de continuar
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("");
            Console.Write("\t\t\tPulsa una tecla para continuar...");
            Console.ReadLine();
            Console.ResetColor();
        }
        public static bool Continuar(string mensaje)  //Método sobrescrito de continuar
        {
            bool seguir = false;    // Control de la confirmación
            string aux = null;

            // ENTRADA
            Console.Write("\n\t\t" + mensaje);
            aux = Console.ReadLine();
            aux = aux.Trim().ToLower(); // Métodos en cadena: 1º limpia ; 2º minúsculas

            // VALIDACION --> PENDIENTE

            // PROCESAMIENTO
            if ((aux != "") && (aux[0] == 's')) seguir = true;   // Sólo true si 's', false en resto de casos

            // SALIDA
            return seguir;
        }
        public static void Pregunta(ref string pregunta, ref bool salida) // Recibe por referencia la pregunta a realizar y el booleano.
        {
            bool salir = false;
            do
            {
                string eleccion = null;
                Console.Write("\n\t\t " + pregunta);
                eleccion = Console.ReadLine();
                eleccion = eleccion.ToLower();
                if (eleccion.Substring(0, 1) == "s")
                {
                    salir = true;
                    salida = false;
                }
                else if (eleccion.Substring(0, 1) == "n")
                {
                    salir = true;
                    salida = true;
                }
                else
                {
                    Error("Introduce una S para borrar otro empleado o una N para salir.");
                    salir = false;
                    Continuar();
                }
            } while (!salir);
        }
        private static string LineaSeparador(String car) // Antonio Baena - Crea guiones utilizados en la salida por pantalla, acorde al ancho de la consola.
        {
            String cadena = null;
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                cadena += car;
            }
            cadena += "\r";
            return cadena;
        }
        #endregion 

        #region MENUS PRINCIPALES - Francisco Romero
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
                Console.WriteLine("\t\t\t2 -> Operaciones nómina");
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
        public static void OperacionesEmpleado()
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

                if (Byte.TryParse(eleccion, out seleccion) && (seleccion >= 1) && (seleccion <= 6))
                {
                    GestionNegocio.GestionOperaciones(Int32.Parse(eleccion), ref flag, 1);
                }
                else
                {
                    fail = true;
                    msg = "Opción Incorrecta (seleccione una opción del menú: 1 - 5)";
                }

            } while (!flag);
        }
        #endregion

        #region CONTRASEÑA  - Francisco Romero
        public static string PedirContraseña() // PIDE LA CONTRASEÑA PARA PODER MODIFICAR EL ARCHIVO DE CONFIGURACIÓN
        {
            bool salir = false, error = false;
            string password = null, mensaje = null;
            do
            {
                if (error)
                {
                    Error(mensaje);
                    Continuar();
                }
                Header();
                Console.Write("\t\t\tIntroduzca la contraseña: ");
                password = Console.ReadLine();
                if (password.Length >= 3 && password.Length <= 16)
                {
                    salir = true;
                }
                else
                {
                    mensaje = "La contraseña introducida es incorrecta, introduzca una contraseña de 3 - 16 carácteres.";
                    salir = false;
                }
            } while (!salir);
            return password;
        }
        public static string PedirContraseñaModificar() // PIDE LA CONTRASEÑA ACTUAL PARA PODER MODIFICAR ESTA (POR DEFECTO ES 1234)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9]+$");
            string passactual = null, passnueva = null;
            bool salir = false;
            do
            {
                Header();
                Console.Write("\t\t\tIntroduce la contraseña actual");
                passactual = Console.ReadLine();
                if (GestionNegocio.ValidarContraseña(passactual))
                {
                    Console.Write("\t\t\tIntroduce la contraseña nueva");
                    passnueva = Console.ReadLine();
                    if (!rg.IsMatch(passnueva) || passnueva.Length < 3)
                    {
                        Error("La cadena introducida solo puede contener números o letras y un mínimo de 3 caracteres.");
                        Continuar();
                        salir = false;
                }
                else
                {
                        salir = true;
                    }
                }
                else
                {
                    Error("La contraseña actual no es correcta, por favor, intentelo de nuevo.");
                    Continuar();
                    salir = false;
                }
            } while (!salir);
            return passnueva;
        }
        #endregion
        
        #region FORMATO SALIDA POR PANTALLA - Francisco Romero
        public static void HeaderVerTrabajadores() // Cabecera del trabajador
        {
            Console.WriteLine("---------------------------------------------------".PadLeft(64));
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("|     DNI    |     NOMBRE      |  APELLIDOS       |".PadLeft(64));
            Console.ResetColor();
            Console.WriteLine("---------------------------------------------------".PadLeft(64));
            Console.WriteLine("");
        }
        public static void FormatoLeerXML(string dni, string nombre, string apellidos) // Muestra los datos del trabajador de la siguiente manera...
        {
            Console.WriteLine("╔══════════════════════════════════════════════════".PadLeft(64));
            Console.WriteLine(string.Format("\t     ║ {0,-9} | {1,15} | {2,10}", dni, nombre, apellidos).PadRight(70));
            Console.WriteLine("╚══════════════════════════════════════════════════".PadLeft(64));
            Console.WriteLine("");
        }
        #endregion Formato de Salida VER TRABAJADORES
        
        #region Archivo Configuración - Francisco Romero
        // PEDIR DATOS PARA MODIFICAR EL ARCHIVO DE CONFIGURACIÓN
        public static void PedirDatosArchivoConf(ref int option, ref float valor)
        {
            int eleccion, retencionTemp;
            Boolean salir = false, error = false;
            string mensaje = null, ret = null;
            Nomina nm = new Nomina();


            do
            {
                Header();
                Console.WriteLine("\t\t\t¿Qué aspecto quieres modificar?");
                Console.WriteLine("\t\t\t1 - Jornada Laboral Semanal");
                Console.WriteLine("\t\t\t2 - Valor de las retenciones");
                Console.WriteLine("\t\t\t3 - Volver");
                Console.Write("\t\t\tElección: ");
                if (int.TryParse(Console.ReadLine(), out eleccion) && (eleccion >= 1) && (eleccion <= 3))
                {
                    do
                    {
                        if (error)
                        {
                            Error(mensaje);
                            Continuar("Pulsa una tecla para continuar...");
                        }
                        try
                        {
                            Header();
                            switch (eleccion)
                            {
                                case 1:
                                    Console.Write("\n\t\t\tNuevo valor de la jornada: ");
                                    nm.JornadaPre = Int32.Parse(Console.ReadLine());
                                    valor = nm.JornadaPre;
                                    option = 1;
                                    salir = true;
                                    break;
                                case 2:
                                    Console.Write("\n\t\t\tNuevo valor de la retenciones (Porcentaje): ");
                                    ret = Console.ReadLine();
                                    if (Int32.TryParse(ret, out retencionTemp))
                                    {
                                        nm.RetencionPre = float.Parse(retencionTemp.ToString(), CultureInfo.CurrentUICulture);
                                        valor = nm.RetencionPre;
                                        option = 2;
                                        salir = true;
                                    }
                                    else
                                    {
                                        mensaje = "El valor debe ser mayor que 0 y menor que 100";
                                        error = true;
                                    }
                                    break;
                                case 3:
                                    salir = true;
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            mensaje = e.Message;
                            Continuar("Pulsa una tecla para continuar...");
                            salir = false;
                            error = true;
                        }
                    } while (error); // SI FALLA ALGO VUELVE...
                }
                else
                {
                    salir = false;
                    Error("La elección no es correcta, inserte un número del 1-3");
                    Continuar("Pulsa una tecla para continuar...");
                }
            } while (!salir);
        }
        #endregion

        #region Interfaz Trabajadores - Óscar Calvente
        public static Trabajador PlantillaCrearTrabajador()
        {
            bool correcto = false;
            bool error = false;    // Control error inicializado
            string mensaje = null;
            bool existe = false;

            Trabajador trabajador = new Trabajador();  // Cliente Temporal
            string aux = null;

            do
            {
                do
                {

                    if (error)
                    {
                        Error(mensaje);  // Presentación de Errores
                        Continuar("Pulse una tecla para continuar...");
                        error = false;          // Reinicio del Control de Errores
                    }
                    Header();
                    Console.WriteLine("\n\t\t APERTURA CUENTA: DATOS DEL NUEVO TRABAJADOR\n");

                    // ENTRADA: DNI del Cliente
                    if (dni_trb != null) // SI NO ES NULL LO ASIGNA A DNI
                    {
                        trabajador.dni_pre = dni_trb;
                    }

                    if (trabajador.dni_pre == null)
                    {
                        try
                        {
                            Console.Write("\t\t Introduzca DNI (12345678A): ");
                            // El DNI es válido
                            aux = Console.ReadLine();
                            aux = aux.Trim().ToUpper();
                            existe = Gestion_Empleado.ComprobarDni(aux);
                            if (existe)
                            {
                                Error("El empleado ya se encuentra registrado");
                                Continuar("Pulsa una tecla para continuar");
                                correcto = false;
                            }
                            else
                            {
                                trabajador.dni_pre = aux;
                                correcto = true;
                            }
                        }
                        catch (Exception e)
                        {
                            error = true;
                            mensaje = e.Message;
                        }
                    }
                    else
                    {
                        Console.Write("\t\t Introduzca DNI (12345678A): ");
                        Console.WriteLine("{0}", trabajador.dni_pre);

                        correcto = true;
                    }
                } while (!correcto);
                // ENTRADA: Nombre y Apellidos
                if (!error)
                {
                    Console.Write("\t\t Introduzca Nombre: ");
                    if (trabajador.nombre_pre == null) // Dato introducido?
                    {
                        try
                        {
                            // Validación Nombre --> NO TESTADO (PENDIENTE)
                            aux = Console.ReadLine();
                            trabajador.nombre_pre = aux;
                            error = false;
                        }
                        catch (Exception e)
                        {
                            error = true;
                            correcto = false;
                            mensaje = e.Message;
                        }

                    }
                    else
                    {
                        Console.WriteLine("{0}", trabajador.nombre_pre);   // Apellidos válidos
                    }
                }


                if (!error)
                {
                    Console.Write("\t\t Introduzca Apellidos: ");
                    if (trabajador.apellidos_pre == null) // Dato introducido?
                    {
                        try
                        {
                            aux = Console.ReadLine();  // Limpieza de entrada (espacios en blanco)
                                                       // Validación Apellidos --> NO TESTADO (PENDIENTE)
                            trabajador.apellidos_pre = aux;
                            error = false;
                        }

                        catch (Exception e)
                        {
                            error = true;
                            correcto = false;
                            mensaje = e.Message;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("{0}", trabajador.apellidos_pre);    // Apellidos válidos

                }

            } while (!correcto);

            return trabajador;     // Datos del Cliente
        }
        public static string PlantillaPedirDni()
        {
            string dni = null, mensaje = null, mensaje2 = null;
            bool salir = false;
            bool existe = false;
            Trabajador trabajador = new Trabajador();
            do
            {
                try
                {
                    Interfaz.Header();
                    Console.WriteLine("\t\tA continuacion, introduce el DNI del empleado.");
                    Console.Write("\n\t\t\tIntroduce el DNI: ");
                    dni = Console.ReadLine();
                    dni = dni.ToUpper();
                    trabajador.dni_pre = dni; //COMPROBAR DNI PARA VER SI ES REAL
                    existe = Gestion_Empleado.ComprobarDni(dni.ToUpper());
                    if (existe == true)
                    {
                        trabajador = null; //VACIAR OBJETO TRABAJADOR PARA AHORRAR MEMORIA
                        salir = true;
                    } else
                    {
                        salir = true;
                    }
                }
                catch (Exception e)
                {
                    salir = false;
                    mensaje2 = "Pulse Enter para Continuar";
                    Error(e.Message);
                    Continuar(mensaje2);
                }
            } while (!salir);
            return dni;
        }
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
            string cambio = null, mensaje = null;
            bool existe = false;
            Trabajador trabajador = new Trabajador();
            do
            {
                try
                {
                    Header();
                    switch (eleccion)
                    {

                        case "1":
                            Console.Write("\n\t\tIntroduce el DNI nuevo: ");
                            cambio = Console.ReadLine();
                            cambio = cambio.ToUpper();
                            trabajador.dni_pre = cambio;
                            existe = Gestion_Empleado.ComprobarDni(cambio);
                            if (existe == true)
                            {
                                salir = false;
                                mensaje = "DNI ya se encuentra en la base de datos";
                                Continuar(mensaje);
                            }
                            else
                            {
                                salir = true;
                                mensaje = "DNI agregado con éxito";
                                Continuar(mensaje);
                            }
                            break;
                        case "2":
                            Console.Write("\n\t\tIntroduce el Nombre nuevo: ");
                            cambio = Console.ReadLine();
                            trabajador.nombre_pre = cambio;
                            salir = true;
                            break;
                        case "3":
                            Console.Write("\n\t\tIntroduce el Apellido nuevo: ");
                            cambio = Console.ReadLine();
                            trabajador.apellidos_pre = cambio;
                            salir = true;
                            break;
                    }
                }
                catch (Exception e)
                {
                    salir = false;
                    Continuar(e.Message);
                }
            } while (!salir);
            return cambio;
        }
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

        #region Interfaz Nómina - Antonio Baena
        //Menú general de opciones de nómina
        public static void OperacionesNomina()
        {
            byte seleccion = 0;
            bool fail = false;
            bool flag = false;  // Control de datos correctos
            string eleccion = null;
            string msg = null;
            string dni = null;
            string cadena = null;


            dni = Interfaz.PlantillaPedirDni(); // PIDE EL DNI
            do
            {
                if (Gestion_Empleado.ComprobarDni(dni))
                {
                    Ficheros.ExistOrEmptyNOM(dni);

                    Header();
                    if (fail)
                    {
                        Error(msg);
                    }
                    Console.WriteLine("\n\t\t\t1 -> Introducir nueva nómina");
                    Console.WriteLine("\t\t\t2 -> Modificar nómina ");
                    Console.WriteLine("\t\t\t3 -> Modificar Datos Archivo de Configuracion");
                    Console.WriteLine("\t\t\t4 -> Eliminar nómina ");
                    Console.WriteLine("\t\t\t5 -> Mostrar nómina");
                    Console.WriteLine("\t\t\t6 -> Cerrar nómina");
                    Console.WriteLine("\t\t\t0 -> Salir\n");
                    Console.Write("\t\t\tEleccion: ");
                    eleccion = Console.ReadLine();
                    eleccion = eleccion.Trim();

                    if (Byte.TryParse(eleccion, out seleccion) && (seleccion >= 0) && (seleccion <= 6))
                    {
                        if (seleccion == 0) { dni_trb = null; }
                        GestionNegocio.GestionNominas(seleccion, ref flag, dni);
                    }
                    else
                    {
                        fail = true;
                        msg = "Opción Incorrecta (seleccione una opción del menú: 0 - 6)";
                    }

                }
                else
                {
                    Console.WriteLine("\n\t\t\tEl trabajador no se encuentra registrado");
                    cadena = "¿Quieres registrar este trabajador?";
                    Interfaz.Pregunta(ref cadena, ref flag);
                    dni_trb = dni;
                    if (flag == false)
                    {
                        GestionNegocio.GestionOperaciones(1, ref flag, 2);
                    }
                }
            } while (!flag);
        }
        
        //Recoge los datos de la nómina
        internal static Nomina DatosNomina()//TODO: Comprobacion de errores
        {
            String aux = null;
            int numeroInt = 0;

            Nomina nomtemp = new Nomina();
            Gestion_Nomina.InicializaNomina(ref nomtemp);

            Console.WriteLine("Por favor, introduzca las horas trabajadas esta semana");
            aux = Console.ReadLine();
            if (!Int32.TryParse(aux, out numeroInt))
            {
                throw new Exception("El número no es válido, por favor introduzca un valor numérico");
            }
            else
            {
                nomtemp.Horas_pre = numeroInt;
            }
            Console.WriteLine("Las horas tiene un precio de " + nomtemp.PrecioPre + ", si desea modificarlo, introduzca el precio por hora para la semana, en caso contrario, pulse ENTER");
            aux = Console.ReadLine();
            if (aux != null)
            {
                if (!Int32.TryParse(aux, out numeroInt))
                {
                    throw new Exception("El número no es válido, por favor introduzca un valor numérico");
                }
                else
                {
                    nomtemp.PrecioPre = numeroInt;
                }
            }
            return nomtemp;
        }

        //Pide los datos de la semana
        internal static Nomina PedirSemana(Nomina[] nominas)
        {
            int semana = 0;
            int horas = 0;
            int precio = 0;
            int jornada = 0;
            float retencion = 0.0F;
            float valorHExtra = 0.0F;
            bool correcto = false;

            Nomina nomtemp = new Nomina();

            do
            {
                Header();
                try
                {
                    Console.Write("\n\t\tPor favor, introduzca la semana que quiere añadir" +
                               "\n\t\tSemana:");
                    semana = Convert.ToInt32(Console.ReadLine());
                    if (Gestion_Nomina.ExisteNomina(ref nominas, semana))
                    {
                        Error("La semana que está intentando crear ya existe. por favor, introduzca otra semana");
                        Continuar("Pulsa una tecla para continuar...");
                        correcto = false;
                    }
                    else
                    {

                        nomtemp.ID_pre = semana;

                        Console.Write("\t\tPor favor, introduzca el número de horas trabajadas" +
                                   "\n\t\tHoras:");
                        horas = Int32.Parse(Console.ReadLine());
                        Console.Write("\t\tPor favor, introduzca el precio por hora trabajada" +
                                   "\n\t\tPrecio:");
                        precio = Int32.Parse(Console.ReadLine());
                        nomtemp.Horas_pre = horas;
                        nomtemp.PrecioPre = precio;
                        Ficheros.getConfig(ref jornada, ref valorHExtra, ref retencion);
                        nomtemp.JornadaPre = jornada;
                        nomtemp.HextrasPre = valorHExtra;
                        nomtemp.RetencionPre = retencion;

                        correcto = true;
                    }
                }
                catch (Exception e)
                {
                    Error(e.Message.ToString());
                    Continuar("Pulsa una tecla para continuar...");
                    correcto = false;
                }
            } while (!correcto);

            return nomtemp;
        }

        //Interfaz del método de modificar nómina
        public static int NominaModificar(Nomina nomina)
        {
            int opcion = 0;
            bool salir = false;
            do
            {
                Header();
                Console.WriteLine("\n\tPor favor, introduzca:\n");
                Console.WriteLine("\n\t\t1 - Modificar las horas de la semana");
                Console.WriteLine("\n\t\t2 - Modificar el precio por hora de la semana");
                Console.WriteLine("\n\t\t3 - Volver atrás");

                if (!Int32.TryParse(Console.ReadLine(), out opcion))
                {
                    salir = false;
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca una opción de 1 a 3");

                }
                else if (opcion < 1 || opcion > 3)
                {
                    salir = false;
                    throw new Exception("El número no es válido, por favor, introduzca una opción de 1 a 3");
                }
                else
                {
                    // HACER SWITCH
                    salir = true;
                }
            } while (!salir);
            return opcion;

        }

        //Interfaz de volcado de pantalla de mostrar Nomina
        public static string MostrarNomina(Nomina[] nomina, string dni)
        {
            Header();
            String cadena = null;
            cadena += "\n";
            int i = 0;
            Trabajador trabajador = new Trabajador();
            trabajador = Ficheros.GetDatosTrabajador(dni);
            cadena += HeaderNominaTrabajador(trabajador);
            cadena += "\n\t\tHoras\tEur/h\thExt.\tSExtra\tSBruto\tImp.\tSNeto\r\n";
            cadena += "\n" + LineaSeparador("-") + "\r";
            if (nomina.Length != 0)
            {
                for (i = 0; i < nomina.Length; i++)
                {
                    if (nomina[i] != null)
                    {
                        cadena += "\nSemana " + (nomina[i].ID_pre);
                        cadena += "\t" + nomina[i].Horas_pre;
                        cadena += "\t" + nomina[i].PrecioPre;
                        cadena += "\t" + nomina[i].HExtra_pre;
                        cadena += "\t" + nomina[i].SalExtra_pre;
                        cadena += "\t" + nomina[i].SalBruto_pre;
                        cadena += "\t" + nomina[i].SalRetencion_pre;
                        cadena += "\t" + nomina[i].SalNeto_pre + "\r";
                    }
                }
            }

            else
            {
                cadena += "\t\t\t<¡ No hay semanas registradas !>\n";
            }


            cadena += "\n" + LineaSeparador("=") + "\r";

                Console.WriteLine(cadena);
            return cadena;
        }

        public static string MostrarNominaTemporal(Nomina[] nomina, string dni)
        {
            Header();
            String cadena = null;
            cadena += "\n";
            int i = 0;
            Trabajador trabajador = new Trabajador();
            trabajador = Ficheros.GetDatosTrabajador(dni);
            cadena += HeaderNominaTrabajador(trabajador);
            cadena += LineaSeparador("-");
            cadena += "ID\t Horas\t  Euros/Hora\t  Jornada\tRetenciones\tV.Horas Extras\n";
            cadena += LineaSeparador("-");
            if (nomina.Length != 0)
            {
                for (i = 0; i < nomina.Length; i++)
                {
                    if (nomina[i] != null)
                    {
                        cadena += "\nSemana " + (nomina[i].ID_pre);
                        cadena += "   " + nomina[i].Horas_pre;
                        cadena += "\t  " + nomina[i].PrecioPre;
                        cadena += "\t\t  " + nomina[i].JornadaPre;
                        cadena += "\t\t" + nomina[i].RetencionPre;
                        cadena += "\t\t" + nomina[i].HextrasPre;
                    }
                }
            }
            else
            {
                cadena += "\t\t\t<¡ No hay semanas registradas !>\n";
            }

            cadena += "\r\n" + LineaSeparador("=")+ "\r";

            return cadena;
        }

        public static string CierreMes(Nomina[] nomina)
        {
            String cadena = null;
            int i = nomina.Length;
            cadena += "\nTOTAL MES:\t";
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 1)) + "\t";
            cadena += Convert.ToString(Math.Round(Gestion_Nomina.CalculaTotal(nomina, 2) / i, 2)) + "\t";//Hacemos el cálculo del precio de la hora media
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 3)) + "\t";
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 4)) + "\t";
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 5)) + "\t";
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 6)) + "\t";
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 7)) + "\t\r";

            Console.WriteLine(cadena);

            return cadena;
        }

        internal static bool Confirmar()
        {
            String cad = null;
            bool confirma = false;
            cad = "\t¿Desea usted guardar los cambios?(s/n)";
            Pregunta(ref cad, ref confirma);

            return confirma;
        }

        internal static int EliminarSemana()
        {
            Header();
            int semana = 0;
            bool salir = false;
            do
            {
                Header();
                Console.WriteLine("\n\tIntroduzca el número de semana que desea eliminar de la nómina:\n");
                Console.Write("\n\t\tSemana: ");
                if (!Int32.TryParse(Console.ReadLine(), out semana))
                {
                    salir = false;
                    Error("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else if (semana < 1 || semana > 6)
                {
                    salir = false;
                    Error("El número no es válido, por favor, introduzca una semana que exista");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else
                {
                    salir = true;
                }
            } while (!salir);
            return semana;

        }

        internal static int EliminarSemanaOpcion()
        {
            int opcion = 0;
            bool salir = false;
            do
            {
                Header();
                Console.WriteLine("\n\t\t\tPor favor, introduzca:\n");
                Console.WriteLine("\t\t\t 1 - Eliminar una semana");
                Console.WriteLine("\t\t\t 2 - Eliminar la nómina completa");
                Console.WriteLine("\t\t\t 3 - Volver atrás");
                Console.Write("\n\t\t\t Eleccion: ");
                if (!Int32.TryParse(Console.ReadLine(), out opcion))
                {
                    salir = false;
                    Error("Se ha introducido un caracter no válido, por favor, introduzca una opción de 1 a 3");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else if (opcion < 1 || opcion > 3)
                {
                    salir = false;
                    Error("El número no es válido, por favor, introduzca una opción de 1 a 3");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else
                {
                    salir = true;
                }
            } while (!salir);
            return opcion;
        }

        internal static int QueSemana(Nomina[] Nomina)
        {
            int semana = 0;
            bool salir = false;
            do
            {
                Console.WriteLine("\n\tPor favor, introduzca el número de semana:\n");
                Console.WriteLine("\n\t\tSemana: ");
                if (!Int32.TryParse(Console.ReadLine(), out semana))
                {
                    salir = false;
                    Error("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else if (semana < 1 || semana > 6)
                {
                    salir = false;
                    Error("El número no es válido, por favor, introduzca una semana que exista");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else
                {
                    salir = true;
                }
            } while (!Gestion_Nomina.ExisteNomina(ref Nomina, semana) && !salir);
            return semana;
        }

        internal static int SolicitarHoras()
        {
            int horas = 0;
            bool flag = false;
            do
            {
                Console.WriteLine("\n\tPor favor, introduzca el número de horas trabajadas esta semana\n");
                Console.WriteLine("\n\t\tHoras: ");
                if (!Int32.TryParse(Console.ReadLine(), out horas))
                {
                    flag = false;
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");

                }
                if (horas < 1 || horas > 168)
                {
                    flag = false;
                    throw new Exception("El número no es válido, por favor, introduzca un número de horas adecuado");

                }
                else flag = true;
            } while (!flag);

            return horas;

        }

        internal static float SolicitarPrecio()
        {
            float precio = 0.0F;
            bool flag = false;
            do
            {
                Console.WriteLine("\n\tPor favor, introduzca el precio por hora de esta semana\n");
                Console.WriteLine("\n\t\tPrecio: ");
                if (!Single.TryParse(Console.ReadLine(), out precio))
                {
                    flag = false;
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");

                }
                if (precio < 1 || precio > 168)
                {
                    flag = false;
                    throw new Exception("El número no es válido, por favor, introduzca un número de horas adecuado");

                }
                else flag = true;
            } while (!flag);
            return precio;
        }

        internal static string Pidefecha()
        {
            DateTime anho = new DateTime();

            string fecha = null;
            bool ctrl = false;
            int num = 0;
            string aux = null;
            anho = DateTime.Now;
            do
            {
                Header();
                Console.WriteLine("\t\tPor favor, introduzca el año de la nómina:");
                Console.Write("\t\tAño: ");
                aux = Console.ReadLine();
                if (!Int32.TryParse(aux, out num))
                {
                    ctrl = false;
                    Error("Ha introducido un valor no válido, debe introducir un valor numérico");
                    Continuar("Pulsa una tecla para continuar");
                }
                else
                {
                    if (num < 1 || num > anho.Year)
                    {
                        ctrl = false;
                        Error("Ha introducido un año no válido.");
                        Continuar("Pulsa una tecla para continuar");
                    }
                    else
                    {
                        aux.Trim();
                        fecha = Regex.Replace(aux, " ", "");
                        ctrl = true;
                    }
                }
            } while (!ctrl);
            do
            {
                Header();
                Console.WriteLine("\t\tPor favor, introduzca el mes de la nómina:");
                Console.Write("\t\tMes: ");
                aux = Console.ReadLine();
                if (!Int32.TryParse(aux, out num))
                {
                    ctrl = false;
                    Error("Ha introducido un valor no válido, debe introducir un valor numérico");
                    Continuar("Pulsa una tecla para continuar");
                }
                else
                {
                    if (num < 1 || num > 12)
                    {
                        ctrl = false;
                        Error("\n\tHa introducido un valor no válido.\r\n\tPor favor, introduzca un mes entre Enero (1) y Diciembre (12)\r");
                        Continuar("Pulsa una tecla para continuar");
                    }
                    else
                    {
                        aux.Trim();
                        fecha += Regex.Replace(aux, " ", "");
                        ctrl = true;
                    }
                }
            } while (!ctrl);
            
            return fecha;


        }

        //Cabecera de la Nomina con los datos del trabajador
        private static string HeaderNominaTrabajador(Trabajador trabajador)
        {
            string cadena = null;
            cadena += "\n" + LineaSeparador("-") + "\r";
            cadena += "\nTrabajador\r";
            cadena += "\nDNI: " + trabajador.dni_pre + "\t\t";
            cadena += "Nombre: " + trabajador.nombre_pre + "\t";
            cadena += "Apellidos: " + trabajador.apellidos_pre + "\r";
            cadena += "\n"+LineaSeparador("-") + "\r";
            return cadena;

        }

        #endregion

    }
}
