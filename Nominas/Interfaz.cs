using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

/// <summary> INFORME : 
/// Clase que contiene las métodos correspondientes para mostrar por pantalla y facilitar al usuario la introducción
/// de datos por teclado.
/// Contiene métodos tanto para trabajadores como para las nóminas.
/// Dichos Métodos actúan de plantilla para la creación de un nuevo trabajador, modificación y borrado de un trabajador
/// así como métodos para mostrar errores o continuaciones.
/// Los métodos de nómina se encargan de gestionar los procesos de entrada/salida de la parte relacionada con las nóminas.
/// </summary>

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
                if (eleccion == "")
                {
                    Error("Introduce una S para crear un nuevo empleado o una N para salir.");
                    salir = false;
                }
                else
                {

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
                        Error("Introduce una S para crear un nuevo empleado o una N para salir.");
                        salir = false;
                    }

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
                Console.Write("\t\t\tIntroduce la contraseña actual: ");
                passactual = Console.ReadLine();
                if (GestionNegocio.ValidarContraseña(passactual))
                {
                    Console.Write("\t\t\tIntroduce la contraseña nueva: ");
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
                                    Console.Write("\n\t\tNuevo valor de la retenciones (Porcentaje): ");
                                    ret = Console.ReadLine();
                                    if (Int32.TryParse(ret, out retencionTemp))
                                    {
                                        nm.RetencionPre = float.Parse(retencionTemp.ToString(), CultureInfo.InvariantCulture);
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

        //Método con una plantilla para la creación de trabajadores
        public static Trabajador PlantillaCrearTrabajador()
        {
            bool correcto = false;
            bool error = false;    // Inicializacion de variables
            string mensaje = null;
            bool existe = false;

            Trabajador trabajador = new Trabajador();  // Creamos un objeto de trabajador
            string aux = null;

            do
            {
                do
                {

                    if (error)
                    {
                        Error(mensaje);  // Presentación de Errores
                        Continuar("Pulse una tecla para continuar...");
                        error = false;   // Reinicio del Control de Errores
                    }
                    Header();
                    Console.WriteLine("\n\t\tDATOS DEL NUEVO TRABAJADOR\n");

                    // ENTRADA: DNI del Trabajador
                    if (dni_trb != null) // SI NO ES NULL LO ASIGNA A DNI
                    {
                        trabajador.dni_pre = dni_trb;
                    }

                    if (trabajador.dni_pre == null)
                    {
                        try
                        {
                            Console.Write("\t\t Introduzca DNI (12345678A): ");
                            aux = Console.ReadLine();
                            aux = aux.Trim().ToUpper(); //Pasamos a mayúsculas la letra del DNI
                            existe = Gestion_Empleado.ComprobarDni(aux); //Comprobación si existe en la base de datos
                            if (existe)
                            {
                                Error("El empleado ya se encuentra registrado");
                                Continuar();
                                correcto = false;
                            }
                            else
                            {
                                trabajador.dni_pre = aux; //Validación del DNI
                                correcto = true;
                            }
                        }
                        catch (Exception e)
                        {
                            error = true;
                            mensaje = e.Message; //Si no salta la excepción, mostramos porque ha saltado
                        }
                    }
                    else
                    {
                        Console.Write("\t\t Introduzca DNI (12345678A): ");
                        Console.WriteLine("{0}", trabajador.dni_pre);
                        correcto = true;
                    }
                } while (!correcto);
                // ENTRADA: Nombre del Trabajador
                if (!error)
                {
                    if (trabajador.nombre_pre == null)
                    {
                        try
                        {
                            Console.Write("\t\t Introduzca Nombre: ");
                            aux = Console.ReadLine();
                            aux = aux.Trim();
                            trabajador.nombre_pre = aux;  // Validación Nombre
                            error = false;
                        }
                        catch (Exception e)
                        {
                            error = true;
                            correcto = false;
                            mensaje = e.Message;
                        }

                    }
                    else  // SI NO ES NULL LO ASIGNA A NOMBRE
                    {
                        Console.WriteLine("{0}", trabajador.nombre_pre);   // Apellidos válidos
                    }
                }


                if (!error)
                {
                    if (trabajador.apellidos_pre == null)
                    {
                        try
                        {
                            Console.Write("\t\t Introduzca Apellidos: ");
                            aux = Console.ReadLine();
                            aux = aux.Trim();
                            trabajador.apellidos_pre = aux; // Validación Apellidos 
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
                else // SI NO ES NULL LO ASIGNA A APELLIDOS
                {
                    Console.WriteLine("{0}", trabajador.apellidos_pre);    // Apellidos válidos

                }

            } while (!correcto);

            return trabajador;     // Devolvemos los datos del trabajador
        }
        //Método con una plantilla para pedir el DNI
        public static string PlantillaPedirDni()
        {
            string dni = null;
            bool salir = false; //Inicialización de variables
            bool existe = false;
            Trabajador trabajador = new Trabajador();
            do
            {
                try
                {
                    Header(); //Mostramos el Header 
                    Console.WriteLine("\t\tA continuacion, introduce el DNI del empleado.");
                    Console.Write("\n\t\t\tIntroduce el DNI: ");
                    dni = Console.ReadLine();
                    dni = dni.ToUpper(); //Convertimos el DNI a mayúsculas
                    trabajador.dni_pre = dni; //COMPROBAR DNI PARA VER SI ES REAL
                    existe = Gestion_Empleado.ComprobarDni(dni.ToUpper()); //Comprobamos si ya está en la Base de datos
                    if (existe == true)
                    {
                        trabajador = null; //VACIAR OBJETO TRABAJADOR PARA AHORRAR MEMORIA
                        salir = true;
                    }
                    else
                    {
                        salir = true; //Si no existe, sale del bucle
                    }
                }
                catch (Exception e)
                {
                    salir = false;
                    Error(e.Message); //Si nos salta la excepcion, nos indica el motivo
                    Continuar();
                }
            } while (!salir);
            return dni; //Retornamos el DNI validado y comprobado
        }
        //Método que contiene una Plantilla para la modificación de los datos de los trabajadores
        public static String PlantillaEleccionModificar()
        {
            string eleccion = null;
            byte seleccion = 0; //Inicialización de las variables
            bool salir = false;
            do
            {
                Console.Clear(); //Limpiamos la consola, para no mostrar lo anterior
                Interfaz.Header();
                Console.WriteLine("\n\t\t¿Que desea modificar?.");
                Console.WriteLine("\n\t\t1.Modificar DNI.");
                Console.WriteLine("\n\t\t2.Modificar Nombre.");
                Console.WriteLine("\n\t\t3.Modificar Apellidos.");
                Console.Write("\n\t\t\tIntroduce tu elección: ");
                eleccion = Console.ReadLine();

                if (Byte.TryParse(eleccion, out seleccion) && (seleccion > 0) && (seleccion <= 4))
                {
                    salir = true; //Si la eleccione está entre 1 y 3 la validamos
                }
                else
                {
                    Error("Introduce una elección del 1 al 3"); //Si no está entre ese rango, salta el error
                    Continuar();
                }
            } while (!salir);
            return eleccion; //Retornamos la eleccion
        }
        //Método en el que devolvemos el cambio que se va a realizar
        public static String ElementoModificar(string eleccion)
        {
            bool salir = false;
            string cambio = null, mensaje = null; //Inicialización de variables
            bool existe = false;
            Trabajador trabajador = new Trabajador();
            do
            {
                try
                {
                    Header(); //Mostramos el Header
                    switch (eleccion) //Evaluamos diferentes condiciones según la eleccion especificada
                    {
                        //Caso 1: Cambiamos el DNI por el nuevo introducido
                        case "1":
                            Console.Write("\n\t\tIntroduce el DNI nuevo: ");
                            cambio = Console.ReadLine();
                            cambio = cambio.ToUpper(); //Ponemos el DNI en mayúsculas
                            trabajador.dni_pre = cambio; //Validamos el DNI
                            existe = Gestion_Empleado.ComprobarDni(cambio); //Comprobamos si ya está en la base de datos
                            if (existe == true)
                            {
                                salir = false;
                                mensaje = "DNI ya se encuentra en la base de datos";
                                Error(mensaje); //Si ya se encuentra, salta el error
                            }
                            else
                            {
                                salir = true;
                                 //Si no lo encuentra, salimos del bucle.
                            }
                            break;
                        //Caso 2: Modificación del Nombre del trabajador
                        case "2":
                            Console.Write("\n\t\tIntroduce el Nombre nuevo: ");
                            cambio = Console.ReadLine();
                            trabajador.nombre_pre = cambio; //Validación del Nombre introducido
                            salir = true; //Si se valida, salimos del bucle
                            break;
                        //Caso 3: Modificación del Apellido del trabajador
                        case "3":
                            Console.Write("\n\t\tIntroduce el Apellido nuevo: ");
                            cambio = Console.ReadLine();
                            trabajador.apellidos_pre = cambio; //Validación del Apellido introducido
                            salir = true; //Si se valida, salimos del bucle
                            break;
                    }
                }
                catch (Exception e)
                {
                    salir = false;
                    Continuar(e.Message);// Si algún cambio no es validado, salta la excepción y muestra el motivo
                }
            } while (!salir);
            return cambio;//Devolvemos el cambio que hayamos realizado
        }
        #endregion

        #region Interfaz Nómina - Antonio Baena
        #region MENÚS

        //MENÚ GENERAL DE LAS OPCIONES DE NÓMINA
        public static void OperacionesNomina()
        {
            byte seleccion = 0; //ALMACENA LA SELECCIÓN DEL USUARIO
            bool fail = false; //CONTROL DE ERRORES
            bool flag = false;  // Control de datos correctos
            string eleccion = null; //ENTRADA POR TECLADO DE LAS OPCIONES DEL USUARIO
            string msg = null; //SALIDA DE MENSAJES DESDE EL PROGRAMA
            string dni = null;
            string cadena = null; //ALMACENA TEXTOS DE ENTRADA POR TECLADO


            dni = Interfaz.PlantillaPedirDni(); // PIDE EL DNI
            do
            {
                if (Gestion_Empleado.ComprobarDni(dni))
                {
                    Ficheros.ExistOrEmptyNOM(dni);//SI DNI ESTÁ REGISTRADO SE COMPRUEBA QUE EXISTE LA NÓMINA

                    Header();
                    if (fail)//MOSTRAMOS EL FALLO SI HAY ERROR
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
                    eleccion.Trim();//COMO LAS OPCIONES SON DE UNA SOLA CIFRA NO SE CONTROLAN ESPACIOS INTERMEDIOS

                    if (Byte.TryParse(eleccion, out seleccion) && (seleccion >= 0) && (seleccion <= 6))
                    {
                        if (seleccion == 0) { dni_trb = null; }
                        GestionNegocio.GestionNominas(seleccion, ref flag, dni);//SE LANZA LA SELECCIÓN CORRESPONDIENTE
                    }
                    else
                    {
                        fail = true;
                        msg = "Opción Incorrecta (seleccione una opción del menú: 0 - 6)";
                    }

                }
                else//SI EL DNI PEDIDO NO EXISTE EN EL SISTEMA SE DA LA OPCIÓN DE REGISTRAR AL TRABAJADOR
                {
                    Console.WriteLine("\n\t\tEl trabajador no se encuentra registrado");
                    cadena = "¿Quieres registrar este trabajador? (s/n): ";
                    Interfaz.Pregunta(ref cadena, ref flag);
                    dni_trb = dni;
                    if (flag == false)
                    {
                        GestionNegocio.GestionOperaciones(1, ref flag, 2);
                    }
                }
            } while (!flag);
        }

        //MENÚ DE MODIFICAR NÓMINA
        public static int ModificarNomina(Nomina nomina)
        {
            int opcion = 0; //CONTROLA LA OPCIÓN A INTRODUCIR
            bool salir = false; //CONTROL DE ERRORES
            do
            {
                Header();
                Console.WriteLine("\n\t\tPor favor, introduzca:\n");
                Console.WriteLine("\n\t\t1 - Modificar las Horas de la semana");
                Console.WriteLine("\n\t\t2 - Modificar el Precio por hora de la semana");
                Console.WriteLine("\n\t\t3 - Modificar la Jornada predeterminada de la semana");
                Console.WriteLine("\n\t\t4 - Modificar la Retención de la semana");
                Console.WriteLine("\n\t\t0 - Volver atrás");
                Console.Write("\n\t\tElección: ");
                if (!Int32.TryParse(Console.ReadLine().Trim(), out opcion)) //NO CONTROLAMOS ESPACIOS INTERMEDIOS, PUESTO QUE LAS OPCIONES SON DE UNA CIFRA
                {
                    salir = false;
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca una opción de 0 a 4");

                }
                else if (opcion < 0 || opcion > 4)
                {
                    salir = false;
                    throw new Exception("El número no es válido, por favor, introduzca una opción de 0 a 4");
                }
                else
                {
                    // ELECCIÓN CORRECTA, SALIMOS DEL BUCLE DO-WHILE
                    salir = true;
                }
            } while (!salir);
            return opcion; //DEVOLVEMOS EL VALOR DE LA OPCIÓN ELEGIDA

        }

        //MENÚ DE ELIMINAR NÓMINA
        public static int EliminarSemanaOpcion()
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
                if (!Int32.TryParse(Console.ReadLine().Trim(), out opcion))//CONTROLAMOS QUE LA CADENA INTRODUCIDA SEA UN NÚMERO.
                {
                    salir = false;
                    Error("Se ha introducido un caracter no válido, por favor, introduzca una opción de 1 a 3");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else if (opcion < 1 || opcion > 3)//SE CONTROLA QUE EL NÚMERO PERTENEZCA A INTERVALO DETERMINADO.
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
            return opcion;//DEVOLVEMOS LA OPCIÓN
        }
        #endregion
        #region GESTIÓN DE LAS NÓMINAS (Creación Y modificación)
        //CARGA E INICIALIZA LOS DATOS DE UNA SEMANA
        public static Nomina DatosNomina()
        {
            String aux = null; //VARIABLE QUE ALMACENA LA ENTRADA DE TECLADO
            int numeroInt = 0; //VARIABLE QUE ALMACENA LA ENTRADA DE TECLADO CONVERTIDA EN INT

            Nomina nomtemp = new Nomina(); //CREAMOS UN OBJETO NOMINA (NOMTEMP)
            Gestion_Nomina.InicializaNomina(ref nomtemp);//CARGAMOS LOS VALORES DE LA NÓMINA POR DEFECTO EN NOMTEMP
            //PEDIMOS LAS HORAS
            Console.WriteLine("\t\tPor favor, introduzca las horas trabajadas esta semana");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n\t\tHoras: ");
            Console.ResetColor();
            aux = Console.ReadLine();
            aux = Regex.Replace(aux, " ", ""); //INTRODUCIMOS LAS HORAS Y ELIMINAMOS ESPACIOS INTERMEDIOS, AL PRINCIPIO Y AL FINAL.
            if (!Int32.TryParse(aux, out numeroInt))
            {
                throw new Exception("\t\tEl número no es válido, por favor introduzca un valor numérico");
            }
            else
            {
                nomtemp.Horas_pre = numeroInt; //ALMACENAMOS LAS HORAS EN NOMTEMP
            }
            //PEDIMOS EL PRECIO POR HORA
            Console.WriteLine("\t\tLas horas tiene un precio de " + nomtemp.PrecioPre + ", si desea modificarlo, introduzca el precio por hora para la semana, en caso contrario, pulse ENTER");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n\t\tPrecio: ");
            Console.ResetColor();
            aux = Console.ReadLine();
            aux = Regex.Replace(aux, " ", ""); //INTRODUCIMOS EL PRECIO Y ELIMINAMOS ESPACIOS INTERMEDIOS, AL PRINCIPIO Y AL FINAL.
            if (aux != null)
            {
                if (!Int32.TryParse(aux, out numeroInt))
                {
                    throw new Exception("\t\tEl número no es válido, por favor introduzca un valor numérico");
                }
                else
                {
                    nomtemp.PrecioPre = numeroInt;//ALMACENAMOS EL PRECIO POR HORA EN NOMTEMP
                }
            }
            return nomtemp;//DEVOLVEMOS EL OBJETO NOMTEMP CON LOS DATOS YA INTRODUCIDOS.
        }

        //AÑADE UNA SEMANA A UN ARRAY NÓMINA
        public static Nomina AgregarSemana(Nomina[] nominas)
        {
            int semana = 0; //NÚMERO DE LA SEMANA A INTRODUCIR
            string aux = null;
            bool correcto = false; //CONTROL DE ERRORES

            Nomina nomtemp = new Nomina(); //CREAMOS UN OBJETO NOMINA

            do
            {
                Header();
                try
                {
                    Console.Write("\n\t\tPor favor, introduzca la semana que quiere añadir");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("\n\t\tSemana: ");
                    Console.ResetColor();
                    aux = Console.ReadLine();
                    aux.Trim();
                    aux = Regex.Replace(aux, " ", "");
                    semana = Convert.ToInt32(aux); //NO SE ELIMINAN ESPACIOS INTERMEDIOS PUESTO QUE LA SEMANA MÁS ALTA SERÁ LA 6
                    if (Gestion_Nomina.ExisteNomina(ref nominas, semana))//COMPROBAMOS QUE LA LA SEMANA NO EXISTA YA EN EL ARRAY
                    {
                        Error("La semana que está intentando crear ya existe. por favor, introduzca otra semana");
                        Continuar("Pulsa una tecla para continuar...");
                        correcto = false;
                    }
                    else
                    {
                        nomtemp = DatosNomina(); //CARGAMOS LOS DATOS DE LA SEMANA EN LA SEMANA
                        nomtemp.ID_pre = semana; //EN CASO DE NO EXISTIR SE ALMACENA EL VALOR EN NOMTEMP
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
        #endregion

        #region VOLCADO A PANTALLA
        //CABECERA DE LA NÓMINA CON LOS DATOS DEL TRABAJADOR
        private static string HeaderNominaTrabajador(Trabajador trabajador)
        {
            string cadena = null;
            cadena += "\n" + LineaSeparador("-") + "\r";
            cadena += "\nTrabajador\r";
            cadena += "\nDNI: " + trabajador.dni_pre + "\t\t"; //RECUPERAMOS EL DNI DEL TRABAJADOR
            cadena += "Nombre: " + trabajador.nombre_pre + "\t"; //RECUPERAMOS EL NOMBRE DEL TRABAJADOR
            cadena += "Apellidos: " + trabajador.apellidos_pre + "\r"; //RECUPERAMOS EL APELLIDO DEL TRABAJADOR
            cadena += "\n" + LineaSeparador("-") + "\r";
            return cadena; //DEVOLVEMOS LA CADENA FORMATEADA CON LOS DATOS DE LA CABECERA

        }

        //VOLCADO POR PANTALLA DE LOS PARÁMETROS DE LA NÓMINA TEMPORAL
        public static string MostrarNominaTemporal(Nomina[] nomina, string dni)
        {
            String cadena = null; //ALMACENA EL VOLCADO DE LA NOMINA
            int i = 0; //CONTADOR

            Trabajador trabajador = new Trabajador(); //OBJETO TRABAJADOR PARA MOSTRAR LOS DATOS DEL MISMO

            Header();

            trabajador = Ficheros.GetDatosTrabajador(dni); //CARGAMOS LOS DATOS DEL TRABAJADOR
            cadena += "\n";
            cadena += HeaderNominaTrabajador(trabajador); //CARGAMOS CABECERA DE NÓMINA CON LOS DATOS DEL TRABAJADOR
            cadena += LineaSeparador("-");
            cadena += "ID\t Horas\t  Euros/Hora\t  Jornada\tRetenciones\tV.Horas Extras\n";
            cadena += LineaSeparador("-");
            if (nomina.Length != 0)
            {
                for (i = 0; i < nomina.Length; i++) //SE OBTIENEN LOS DATOS DE LAS VARIABLES DE LAS NÓMINAS NO NULAS
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
            else //SI LAS NÓMINAS SON TODAS NULAS, SE MUESTRA UN MENSAJE DE ADVERTENCIA.
            {
                cadena += "\t\t\t<¡ No hay semanas registradas !>\n";
            }

            cadena += "\r\n" + LineaSeparador("=") + "\r";

            return cadena;
        }
        
        //VOLCADO DE MOSTRAR NOMINA
        public static string MostrarNomina(Nomina[] nomina, string dni)
        {
            String cadena = null; //ALMACENA  EL VOLCADO DE LA NÓMINA
            int i = 0; //CONTADOR

            Trabajador trabajador = new Trabajador();

            trabajador = Ficheros.GetDatosTrabajador(dni);//CARGAMOS LOS DATOS DEL TRABAJADOR EN EL OBJETO TRABAJADOR

            Header(); //LLAMAMOS A LA CABECERA DEL PROGRAMA
            cadena += "\n";
            cadena += HeaderNominaTrabajador(trabajador); //ALMACENAMOS EN LA CADENA LA CABECERA DEL TRABAJADOR
            cadena += "\n\t\tHoras\tEur/h\thExt.\tSExtra\tSBruto\tImp.\tSNeto\r\n";
            cadena += "\n" + LineaSeparador("-") + "\r";
            if (nomina.Length != 0) //CARGAMOS EN LA CADENA LAS VARIABLES DE LOS OBJETOS NÓMINA QUE NO SON NULAS.
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

            else //SI NO HAY NÓMINAS REGISTRADAS PARA ESTE TRABAJADOR SALTA EL ERROR
            {
                cadena += "\t\t\t<¡ No hay semanas registradas !>\n";
            }


            cadena += "\n" + LineaSeparador("=") + "\r";

            Console.WriteLine(cadena); //MOSTRAMOS LA CADENA EN PANTALLA
            return cadena; //PASAMOS LA CADENA PARA ALMACENARLA Y GUARDARLA EN ARCHIVO
        }
              
        //VOLCADO DE CIERRE DEL MES
        public static string CierreMes(Nomina[] nomina)
        {
            String cadena = null; //CADENA PARA HACER EL VOLCADO DE LOS VALORES DE LA NÓMINA 
            int i = nomina.Length;

            
            cadena += "\nTOTAL MES:\t";
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 1)) + "\t"; //ALMACENA EL TOTAL DE HORAS
            cadena += Convert.ToString(Math.Round(Gestion_Nomina.CalculaTotal(nomina, 2) / i, 2)) + "\t"; //CALCULAMOS EL PRECIO MEDIO DE LA HORA
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 3)) + "\t"; //ALMACENA LAS HORAS EXTRAS;
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 4)) + "\t";//ALMACENA EL SALARIO EXTRA TOTAL
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 5)) + "\t";//ALMACENA EL SALARIO BRUTO TOTAL
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 6)) + "\t"; //ALMACENA LAS RETENCIONES POR IMPUESTOS
            cadena += Convert.ToString(Gestion_Nomina.CalculaTotal(nomina, 7)) + "\t\r"; //ALMACENA EL SALARIO NETO TOTAL

            Console.WriteLine(cadena);

            return cadena; //DEVOLVEMOS LA CADENA PARA ALMACENARLA EN EL FICHERO DE TEXTO
        }

        #endregion

        #region INTERACCIÓN Y CONTROL
        //ELIGE UNA SEMANA COMPROBANDO QUE EXISTE EN EL ARRAY DE NÓMINAS
        public static int ElegirSemana(Nomina[] Nomina)
        {
            int semana = 0;
            bool salir = false;
            do
            {
                Header();
                Console.WriteLine("\n\t\tPor favor, introduzca el número de semana:\n");
                Console.Write("\n\t\t\tSemana: ");
                if (!Int32.TryParse(Console.ReadLine().Trim(), out semana))//CONTROLAMOS QUE LA CADENA INTRODUCIDA ES UN NÚMERO
                {
                    salir = false;
                    Error("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else if (semana < 1 || semana > 6)//CONTROLAMOS EL RANGO DE SEMANAS QUE PUEDE TENER UN MES
                {
                    salir = false;
                    Error("El número no es válido, por favor, introduzca una semana que exista");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else
                {
                    salir = true;
                }
            } while (!Gestion_Nomina.ExisteNomina(ref Nomina, semana) && !salir);//MIENTRAS EL VALOR INTRODUCIDO NO SEA UNA SEMANA YA EXISTENTE SE MANTIENE EN EL BUCLE
            return semana;//DEVOLVEMOS LA SEMANA 
        }

        //ELIGE UNA SEMANA 
        public static int ElegirSemana()
        {
            int semana = 0;//NÚMERO DE SEMANA A ELEGIR
            bool salir = false;//CONTROL DE ERRORES

            do
            {
                Header();
                Console.WriteLine("\n\tIntroduzca el número de semana que desea eliminar de la nómina:\n");
                Console.Write("\n\t\tSemana: ");
                if (!Int32.TryParse(Console.ReadLine().Trim(), out semana)) //CONTROLAMOS QUE LA CADENA INTRODUCIDA ES UN NÚMERO
                {
                    salir = false;
                    Error("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else if (semana < 1 || semana > 6)//CONTROLAMOS EL RANGO DE SEMANAS QUE PUEDE TENER UN MES
                {
                    salir = false;
                    Error("El número no es válido, por favor, introduzca una semana que exista");
                    Continuar("Pulsa una tecla para continuar...");
                }
                else
                {
                    salir = true;
                }
            } while (!salir);//MIENTRAS EL VALOR INTRODUCIDO NO SEA UNA SEMANA SE MANTIENE EN EL BUCLE
            return semana;//DEVOLVEMOS LA SEMANA 

        }

        //INTERFAZ PARA PEDIR LA FECHA
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
                aux.Trim();
                aux = Regex.Replace(aux, " ", "");
                if (!Int32.TryParse(aux, out num))//CONVERTIMOS LA CADENA EN NÚMERO (O SE INTENTA)
                {
                    ctrl = false; //SI SE INTRODUCE UNA CADENA NO NUMÉRICA)
                    Error("Ha introducido un valor no válido, debe introducir un valor numérico");
                    Continuar("Pulsa una tecla para continuar");
                }
                else
                {
                    if (num < 1 || num > anho.Year)//SI INTENTAMOS INTRODUCIR UN AÑO ANTES DE NUESTRA ERA O DE MÁS ALLÁ DEL AÑO ACTUAL
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
                aux.Trim();
                aux = Regex.Replace(aux, " ", "");
                if (!Int32.TryParse(aux, out num))//CONVERTIMOS LA CADENA EN UN VALOR NUMÉRICO
                {
                    ctrl = false;//LA CADENA NO ES NUMÉRICA
                    Error("Ha introducido un valor no válido, debe introducir un valor numérico");
                    Continuar("Pulsa una tecla para continuar");
                }
                else
                {
                    if (num < 1 || num > 12)//SE INTENTA INTRODUCIR FUERA DEL RANGO DE LOS MESES DE UN AÑO
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

            return fecha;//DEVOLVEMOS UNA CADENA CON FORMATO AAAAMM QUE SE USA PARA DARLE NOMBRE AL FICHERO
        }
        #endregion

        #region INTERFACES PARA PARÁMETROS

        //MÉTODO QUE SOLICITA EL NÚMERO DE HORAS
        public static int SolicitarHoras()
        {
            int horas = 0; //HORAS (LO QUE DEVUELVE EL MÉTODO)
            bool flag = false; //CONTROL DE ERRORES
            string aux = null; //CADENA DE ENTRADA
            do
            {
                Header();
                Console.WriteLine("\n\t\tPor favor, introduzca el número de horas trabajadas esta semana\n");
                Console.Write("\n\t\tHoras: ");
                aux = Regex.Replace(Console.ReadLine().Trim(), " ", "");//ELIMINAMOS ESPACIOS INICIALES, FINALES E INTERMEDIOS
                if (!int.TryParse(aux, out horas))//INTENTAMOS CONVERTIR A NÚMERO
                {
                    flag = false;//CADENA NO NUMÉRICA
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");

                }
                if (horas < 1 || horas > 168)//NUMERO DE HORAS NULAS ONEGATIVAS (SI NO TRABAJA NO TIENE SENTIDO HACERLE UNA NOMINA PARA ESA SEMANA) O SUPERIOR AL NÚMERO DE HORAS DE UNA SEMANA
                {
                    flag = false;
                    throw new Exception("El número no es válido, por favor, introduzca un número de horas adecuado");

                }
                else flag = true;
            } while (!flag);

            return horas;

        }

        //MÉTODO PARA SOLICITAR EL PRECIO POR HORA
        internal static float SolicitarPrecio()
        {
            float precio = 0.0F;//ALMACENAMOS EL PRECIO
            string aux = null;
            bool flag = false;
            do
            {
                Header();
                Console.WriteLine("\n\t\tPor favor, introduzca el precio por hora de esta semana\n");
                Console.Write("\n\t\t\tPrecio: ");
                aux = Regex.Replace(Console.ReadLine().Trim(), " ", "");//ELIMINAMOS ESPACIOS INICIALES, FINALES E INTERMEDIOS
                if (!float.TryParse(aux, out precio))//INTENTAMOS CONVERTIR A NÚMERO
                {
                    flag = false; //SE HA INTRODUCIDO UNA CADENA NO NUMÉRICA
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");

                }
                if (precio < 0) //NO SE PUEDE PAGAR POR TRABAJAR, POR LO QUE EL VALOR NUMÉRICO NO PUEDE SER INFERIOR A 0
                {
                    flag = false;
                    throw new Exception("El número no es válido, por favor, introduzca un precio por horas adecuado");

                }
                else flag = true;
            } while (!flag);
            return precio;
        }

        //MÉTODO PARA SOLICITAR LA DURACIÓN DE LA JORNADA SEMANAL
        internal static int SolicitarJornada()
        {
            int jor = 0;
            bool flag = false;
            string aux = null;

            do
            {
                Header();
                Console.WriteLine("\n\t\tPor favor, introduzca la nueva jornada predeterminada\n");
                Console.Write("\n\t\t\tNueva Jornada: ");
                aux = Regex.Replace(Console.ReadLine().Trim(), " ", "");//ELIMINAMOS ESPACIOS INICIALES, FINALES E INTERMEDIOS
                if (!int.TryParse(aux, out jor))//INTENTAMOS CONVERTIR A NÚMERO
                {
                    flag = false; //SE HA INTRODUCIDO UN VALOR NO NUMÉRICO
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");

                }
                if (jor < 1 || jor > 168)
                {
                    flag = false; //SE HA INTRODUCIDO UN VALOR NO VÁLIDO (INFERIOR A 1 O SUPERIOR A LAS HORAS DE LA SEMANA)
                    throw new Exception("El número no es válido, por favor, introduzca un número de horas adecuado");

                }
                else flag = true;
            } while (!flag);
            return jor;//DEVUELVE LA JORNADA LABORAL SEMANAL
        }

        //MÉTODO PARA SOLICITAR EL PORCENTAJE DE RETENCIÓN
        internal static float SolicitarRetencion()
        {
            float ret = 0.0F;
            string aux = null;
            bool flag = false;
            do
            {
                Header();
                Console.WriteLine("\n\t\tPor favor, introduzca la retención de esta semana\n");
                Console.Write("\n\t\t\tNueva Retención: ");
                aux = Regex.Replace(Console.ReadLine().Trim(), " ", "");//ELIMINAMOS ESPACIOS INICIALES, FINALES E INTERMEDIOS
                if (!float.TryParse(aux, out ret))//INTENTAMOS CONVERTIR A NÚMERO
                {
                    flag = false;//SE INTRODUCE CADENA NO NUMÉRICA
                    throw new Exception("Se ha introducido un caracter no válido, por favor, introduzca un valor numérico");

                }
                if (ret < 1 || ret > 100)//NO SE PUEDE HACER UNA RETENCIÓN INFERIOR AL 1% NI SUPERIOR AL 100%
                {
                    flag = false;
                    throw new Exception("El número no es válido, por favor, introduzca un valor del 1 al 100%");

                }
                else flag = true;
            } while (!flag);
            return ret / 100;
        }
        #endregion
        #endregion

    }
}
