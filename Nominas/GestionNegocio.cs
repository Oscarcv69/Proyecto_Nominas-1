using System;
using System.Configuration;
/// <summary>
/// La clase Gestión de Negocio coordina la  ejecución del programa, integrando la interfaz 
/// con las clases de gestion de trabajadores y personal, esto se realiza mediante los métodos
/// GestioOperaciones que controla mediante un switch case las opciones del menú de trabajadores
/// y Gestión Nómina que hace lo mismo para las opciones del menú nómina.
/// 
/// Por otra parte, están los métodos de gestión de la contraseña de administrador,
/// que implementan su uso, la validan o la modifican .
/// 
/// Un último método es el de Inicializar componentes que lanza tres métodos, el
/// de chequear la configuración (archivo app.config), la contraseña y el de empleado.
///
/// </summary>

namespace Nominas
{
    class GestionNegocio
    {
        #region GestionEmpleado - LLAMADA INTERFAZ
        public static void GestionOperaciones(int numb, ref bool flag, int mode)
        {
            Trabajador[] listaTrabajador = null;
            listaTrabajador = Ficheros.getTrabajadores();

            switch (numb)
            {
                //Agregar trabajadores
                case 1: // *AGREGAR TRABAJADORES FUNCIONA CORRECTAMENTE*
                    Gestion_Empleado.NuevoTrabajador(ref listaTrabajador, mode);
                    Ficheros.GuardarTrabajadores(listaTrabajador);
                    break;
                //Modificar trabajadores
                case 2:
                    // LLAMAR A MÉTODO MODIFICAR USUARIO (REF ARRAY)----
                    // LLAMAR A MÉTODO FICHERO MODIFICAR
                    Gestion_Empleado.ModificarTrabajador(ref listaTrabajador);
                    Ficheros.GuardarTrabajadores(listaTrabajador);
                    break;

                //Eliminar trabajadores 
                case 3: // *BORRADO FUNCIONA CORRECTAMENTE*
                    Gestion_Empleado.BorrarTrabajador(ref listaTrabajador);
                    Ficheros.GuardarTrabajadores(listaTrabajador);
                    break;

                //Modificar Contraseña
                case 4:
                    ModificarContraseña();
                    break;
                case 5: // * MOSTRAR TRABAJADORES FUNCIONA CORRECTAMENTE
                    Gestion_Empleado.ListarTrabajadores(listaTrabajador);
                    break;
                case 6: // SALIR
                    flag = true;
                    break;
            }
        }
        #endregion

        #region GestionNomina - LLAMADA INTERFAZ (Antonio Baena)
        //Métodos que controlan el funcionamiento de la clase GestionNomina y la Interfaz
        public static void GestionNominas(int numb, ref bool flag, string dni)
        {
            string fecha;
            Nomina[] Nomina = null;
            Nomina semana = null;
            try
            {
                //Recuperamos la nómina del trabajador y la cargamos en un Array de Nóminas de trabajo.
                Nomina = Ficheros.GetNomina(dni);
                //Ordenamos el array y lo redimensionamos, dejando huecos en las semanas no creadas aún
                Gestion_Nomina.OrdenaNomina(ref Nomina);

                switch (numb)
                {
                    case 0:
                        flag = true;
                        break;
                    //Introducir nóminas
                    case 1:
                        /*Comprobamos que el array no ha llenado los seis espacios que es el máximo de semanas
                        * que tiene un mes. En caso de haber huecos, lanzamos los métodos para pedir la semana
                        *  e introducirla en el array nómina, tras lo cual se almacena en el fichero.*/
                        if (!Gestion_Nomina.LimiteSemanas(Nomina))
                        {
                            semana = Interfaz.AgregarSemana(Nomina);
                            Gestion_Nomina.CreaSemana(ref Nomina, ref semana);
                            Ficheros.GuardarNominaTemporal(ref Nomina);
                        }
                        else
                        {
                            Interfaz.Error("No se pueden agregar más semanas, se ha alcanzado el máximo posible de semanas de un mes");
                        }
                        break;
                    //Modificar Nóminas
                    case 2:
                        /*Con esta opción vamos a cambiar los parámetros de la nómina, para lo que lanzamos el 
                        * método de cambio de la semana. Y, una vez terminado, se guarda la nómina en el fichero.*/
                        Gestion_Nomina.CambiaSemana(ref Nomina);
                        Ficheros.GuardarNominaTemporal(ref Nomina);
                        break;

                    //Modificar archivo de configuracion
                    case 3:
                        /* Esta opción lanza una interfaz que nos va a permitir modificar los parámetros almacenados en 
                        * app.config, previa validación de la contraseña de administrador*/
                        int option = 0;
                        float valor = 0;
                        string pass = null;
                        pass = Interfaz.PedirContraseña();
                        if (GestionNegocio.ValidarContraseña(pass))
                        {
                            Interfaz.PedirDatosArchivoConf(ref option, ref valor);
                            Ficheros.ModConfig(option, valor);
                            Interfaz.Continuar("Valor modificado con éxito");
                        }
                        else
                        {
                            Interfaz.Error("La contraseña no coincide.");
                            Interfaz.Continuar();
                            break;
                        }
                        break;
                    //Eliminar nominas
                    case 4:
                        /*La opción de eliminar nóminas a su vez lanza un menú por interfaz que permite eliminar una sóla semana
                        * del array de nóminas o, por el contrario, eliminar todas las posiciones de dicho array.
                        * Tanto en un método como en el otro, se lanza un método en gestión de nómina que es el que se encarga de 
                        * hacer la eliminación y redimensión del array. */
                        int ordinal = 0, opcion = 0;
                        opcion = Interfaz.EliminarSemanaOpcion();
                        if (opcion == 1)
                        {
                            ordinal = Interfaz.ElegirSemana(); // Pide la semana a eliminar
                            Gestion_Nomina.ProcesoEliminarSemana(ref Nomina, ordinal);
                        }
                        else
                        {
                            Gestion_Nomina.ProcesoEliminarNomina(ref Nomina);
                        }
                        Ficheros.GuardarNominaTemporal(ref Nomina);
                        break;
                    //Mostrar Nómina Temporal
                    case 5:
                        /*En esta opción se muestra la nómina temporal, con el número de semanas
                        * como los parámetros tales como número de horas, horas extra, 
                        * precio de la hora extra, porcentaje de retencion, etc.*/
                        Console.WriteLine(Interfaz.MostrarNominaTemporal(Nomina, dni));
                        Console.ReadLine();
                        break;
                    //Cerrar Nómina del Mes
                    case 6:
                        /*Este método sirve para guardar la nómina del mes, exportándola a un archivo de texto 
                        * (después de mostrarla en pantalla y pedir confirmación) y eliminar el archivo temporal*/
                        string cadena = null;
                        /*Se calculan los parámetros tales como horas extra trabajadas, salario base, extra, 
                        * retenciones y neto, y se almacenan en él array de nóminas*/
                        Gestion_Nomina.CalculaParcial(ref Nomina);
                        //La interfaz muestra por pantalla los parciales semanales y se devuelve en la variable cadena.
                        cadena = Interfaz.MostrarNomina(Nomina, dni);
                        //El método cierreMes devuelve (y concatena) en la variable cadena el resultado del cálculo de los totales de cada semana.
                        cadena += Interfaz.CierreMes(Nomina);
                        if (!Confirmar())//se pide confirmación para guardar los cambios.
                        {
                            //Almacena en el fichero
                            fecha = Interfaz.Pidefecha();//En este método vamos a pedir la fecha de mes y año de la nómina para almacenarla.
                            Ficheros.CerrarNomina(cadena, fecha);//En este método almacenamos la cadena de texto en un archivo nombrado con la fecha y el dni del trabajador.
                            //Eliminar fichero
                            Ficheros.BorrarTemporal(dni); //Eliminamos el fichero temporal de la nómina del sistema.
                            Interfaz.Continuar("Nómina Exportada Correctamente \n \t\tPulsa una tecla para continuar...");
                        }
                        else
                        {
                            Interfaz.Continuar();
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                Interfaz.Error(e.Message);
                Interfaz.Continuar("Pulsa una tecla para continuar...");
            }
        }
        
        //CONFIRMACIÓN PARA GUARDAR CAMBIOS
        public static bool Confirmar()
        {
            String cad = null; //CADENA QUE ALMACENA LA PREGUNTA
            bool confirma = false; //BOOLEANO DE RESPUESTA
            cad = "\t¿Desea usted guardar los cambios?(s/n)";
           Interfaz.Pregunta(ref cad, ref confirma); //LLAMADA AL MÉTODO QUE REALIZA LA PREGUNTA

            return confirma;
        }
        #endregion


        #region Gestion Contraseña - Francisco Romero
        public static bool GestionContraseña()
        {
            bool correcto = false;
            string password = null;
            password = Interfaz.PedirContraseña();
            if (ValidarContraseña(password))
            {
                correcto = true;
            }
            else
            {
                correcto = false;
            }
            return correcto;
        }

        public static bool ValidarContraseña(string password)
        {
            string originalpassword = null;
            Ficheros.CheckPass(ref originalpassword);
            if (Encriptacion.Encriptar(password).Equals(originalpassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ModificarContraseña()
        {
            string pass = null;
            pass = Interfaz.PedirContraseñaModificar();
            Ficheros.ModPass(pass);
        }
        #endregion

        public static void InicializarComponentes()
        {
            Ficheros.CheckConfig();
            Ficheros.ExistOrEmptyEMP();
            Ficheros.CheckArchivoPass();
        }
    } // FIN CLASE
} // FIN ESP. NOMBRES