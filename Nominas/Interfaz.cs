using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Nominas
{
    class Interfaz
    {
        private static string dni_trb = null;
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
                    trabajador.dni_pre = dni; //COMPROBAR DNI PARA VER SI ES REAL 
                    trabajador = null; //VACIAR OBJETO TRABAJADOR PARA AHORRAR MEMORIA
                    existe = Gestion_Empleado.ComprobarDni(dni);
                    if (existe == true)
                    {
                        salir = true;
                    }
                    else
                    {

                        salir = false;
                        mensaje = "DNI Válido, pero no está en la base de datos";
                        mensaje2 = "Pulse Enter para Continuar";
                        Error(mensaje);
                        Continuar(mensaje2);

                    }
                }
                catch
                {

                    salir = false;
                    mensaje = "DNI No válido";
                    mensaje2 = "Pulse Enter para Continuar";
                    Error(mensaje);
                    Continuar(mensaje2);

                }
            } while (!salir);
            return dni;
        }

       

        public static void Pregunta(ref string pregunta, ref bool salida)
        {
            bool salir = false;
            do
            {
                string eleccion = null;
                Console.Write("\n\t\t " + pregunta);
                eleccion = Console.ReadLine();
                if (eleccion.Equals("s"))
                {
                    salir = true;
                    salida = false;
                }
                else if (eleccion.Equals("n"))
                {
                    salir = true;
                    salida = true;
                }
                else
                {
                    Error("Introduce una S para borrar otro empleado o una N para salir.");
                    Continuar();
                }
            } while (!salir);
        }

        #endregion
        #region Modificar Trabajador - Óscar Calvente
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
            string cambio = null, mensaje = null, mensaje2 = null;
            bool existe = false;
            Trabajador trabajador = new Trabajador();
            do
            {
                Header();
                switch (eleccion)
                {

                    case "1":
                        Console.Write("\n\t\tIntroduce el DNI nuevo: ");
                        cambio = Console.ReadLine();
                        trabajador.dni_pre = cambio;
                        trabajador = null;
                        existe = Gestion_Empleado.ComprobarDni(cambio);
                        if (existe == true)
                        {
                            salir = true;
                        }
                        else
                        {
                            salir = false;
                            mensaje2 = "Pulse Enter para Continuar";
                            Continuar(mensaje2);
                        }
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
        #region Metodo ListarTrabajadores - Óscar Calvente
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

        #region Menús Nómina - Antonio Baena
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
                    Console.WriteLine("\t\t\t7 -> Salir\n");
                    Console.Write("\t\t\tEleccion: ");
                    eleccion = Console.ReadLine();
                    eleccion = eleccion.Trim();

                    if (Byte.TryParse(eleccion, out seleccion) && (seleccion >= 1) && (seleccion <= 7))
                    {
                        GestionNegocio.GestionNominas(seleccion, ref flag, dni);
                    }
                    else
                    {
                        fail = true;
                        msg = "Opción Incorrecta (seleccione una opción del menú: 1 - 7)";
                    }

                }
                else
                {
                    Console.WriteLine("\t\tEl trabajador no se encuentra registrado");
                    cadena = "¿Quieres registrar este trabajador?";
                    Interfaz.Pregunta(ref cadena, ref flag);
                    dni_trb = dni;
                    GestionNegocio.GestionOperaciones(1, ref flag);

                }
            } while (!flag);
        }
        #endregion

        #region Interfaz Nómina - Antonio Baena
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

        // PEDIR DATOS PARA MODIFICAR EL ARCHIVO DE CONFIGURACIÓN
        public static void PedirDatosArchivoConf(ref string name, ref string valor)
        {
            string nombre = null, value = null;
            int eleccion;
            Boolean salir = false;
            do
            {
                Console.WriteLine("\t\t¿Qué aspecto quieres modificar?");
                Console.WriteLine("\t\t1 - Jornada Laboral Semanal");
                Console.WriteLine("\t\t2 - Valor de las horas extras");
                Console.WriteLine("\t\t3 - Valor de las retenciones");
                Console.Write("\t\tEleccion: ");
                if (int.TryParse(Console.ReadLine(), out eleccion) && (eleccion >= 1) && (eleccion <= 3))
                {
                    switch (eleccion)
                    {
                        case 1:
                            Console.Write("\t\tNuevo valor de la jornada: ");
                            break;
                        case 2:
                            Console.Write("\t\tNuevo valor de la jornada: ");
                            break;
                        case 3:
                            Console.Write("\t\tNuevo valor de la jornada: ");
                            break;
                    }
                    salir = true;
                }
                else
                {
                    throw new Exception("La elección no es correcta, inserte un número del 1-3");
                    salir = false;
                }
            } while (!salir);
        }

        //Interfaz de volcado de pantalla de mostrar Nomina
        public static string MostrarNomina(Nomina[] nomina)
        {
            Header();
            String cadena = null;
            cadena += "\n";
            float precioMedio = 0.0F;
            int i = 0;
            Trabajador trabajador = new Trabajador();
            trabajador = Ficheros.GetDatosTrabajador(dni_trb);
            cadena += HeaderNominaTrabajador(trabajador);//COMO PASO EL TRABAJADOR AL METODO?
            cadena += LineaSeparador("-");
            cadena += "\tHoras\tEuros/Hora\tHoras extra\tSal. extra\tSal. Bruto\tImpuestos\tSal. Neto\r";
            cadena += LineaSeparador("-");
            if (nomina == null)
            {
                for (i = 0; i < nomina.Length; i++)
                {
                    if (nomina[i] != null)
                    {
                        cadena += "\tSemana " + (nomina[i].ID_pre);
                        cadena += "\t" + nomina[i].Horas_pre;
                        cadena += "\t" + nomina[i].PrecioPre;
                        cadena += "\t" + nomina[i].SalExtra_pre;
                        cadena += "\t" + nomina[i].SalBruto_pre;
                        cadena += "\t" + nomina[i].SalRetencion_pre;
                        cadena += "\t" + nomina[i].SalNeto_pre;
                        cadena += "\r" + LineaSeparador("-");
                        precioMedio += nomina[i].PrecioPre;
                    }
                }
            }
            else
            {
                cadena += "\t\t\t<¡ No hay semanas registradas !>\n";
            }
            cadena += LineaSeparador("=");

            return cadena;
        }

        public static String CierreMes(Nomina[] nomina)
        {
            String cadena = null;
            float precioMedio = 0.0F;
            int i = nomina.Length;
            cadena += "TOTAL MES:\t\t";
            cadena += Gestion_Nomina.CalculaTotal(nomina, 1) + "\t";
            cadena += precioMedio / i + "\t";//Hacemos el cálculo del precio de la hora media
            cadena += Gestion_Nomina.CalculaTotal(nomina, 2) + "\t";
            cadena += Gestion_Nomina.CalculaTotal(nomina, 3) + "\t";
            cadena += Gestion_Nomina.CalculaTotal(nomina, 4) + "\t";
            cadena += Gestion_Nomina.CalculaTotal(nomina, 5) + "\t";
            cadena += Gestion_Nomina.CalculaTotal(nomina, 6) + "\t\r";

            return cadena;
        }

        internal static bool Confirmar()
        {
            String cad = null;
            bool confirma = false;          
                cad= "\t¿Desea usted guardar los cambios?(s/n)";
                Pregunta(ref cad,ref confirma);
            
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
                Console.WriteLine("\n\tPor favor, introduzca el número de semana que desea eliminar de la nómina:\n");
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

        //Cabecera de la Nomina con los datos del trabajador

        private static string HeaderNominaTrabajador(Trabajador trabajador)
        {
            string cadena = null;
            cadena += LineaSeparador("-");
            cadena += "Trabajador";
            cadena += "DNI: \t" + trabajador.dni_pre + "\t";
            cadena += "Nombre: \t" + trabajador.nombre_pre + "\t";
            cadena += "Apellido: \t" + trabajador.apellidos_pre + "\t";
            cadena += LineaSeparador("-");
            return cadena;

        }

        private static string LineaSeparador(String car)
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

    }
}
