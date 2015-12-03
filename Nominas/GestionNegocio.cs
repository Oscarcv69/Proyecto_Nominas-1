using System;
using System.Configuration;


namespace Nominas
{
    class GestionNegocio
    {
        #region GestionEmpleado - LLAMADA INTERFAZ
        public static void GestionOperaciones(int numb, ref bool flag)
        {
            Trabajador[] listaTrabajador = null;
            listaTrabajador = Ficheros.getTrabajadores();

            switch (numb)
            {
                //Agregar trabajadores
                case 1: // *AGREGAR TRABAJADORES FUNCIONA CORRECTAMENTE*
                    Gestion_Empleado.NuevoTrabajador(ref listaTrabajador);
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

        public static void GestionNominas(int numb, ref bool flag, string dni)
        {
            Nomina[] Nomina = null;
            Nomina = Ficheros.GetNomina(dni);
            Nomina semana = null;


            switch (numb)
            {
                //Necesitamos pedir antes el DNI del trabajador para operar con sus nóminas
                //Introducir nóminas
                case 1:
                    Gestion_Nomina.CargaNomina(ref Nomina);
                    semana = Interfaz.PedirSemana(Nomina);
                    Gestion_Nomina.Grabar(ref Nomina, ref semana);
                    Ficheros.GuardarNominaTemporal(ref Nomina);
                    break;
                //Modificar Nóminas
                case 2:
                    Gestion_Nomina.CambiaSemana(ref Nomina);
                    break;

                //Eliminar Nómina 
                case 3:
                    string name = null, valor = null;
                    Interfaz.PedirDatosArchivoConf(ref name, ref valor);
                    Ficheros.ModConfig(name, valor);
                   // Gestion_Nomina.eliminarNomina(ref Nomina, semana);
                    break;
                //Mostrar Nómina Temporal
                case 4:
                    Gestion_Nomina.CalculaParcial(ref Nomina);
                    Console.WriteLine(Interfaz.MostrarNomina(Nomina));
                    Console.ReadLine();
                    break;
                //Cerrar Nómina del Mes
                case 5:
                    Gestion_Nomina.CierraNomina(ref Nomina);
                    Interfaz.MostrarNomina(Nomina);
                    break;
                case 6: // SALIR
                    flag = true;
                    break;
                case 7: // SALIR
                    flag = true;
                    break;
            }
           /* Ficheros.GuardarNominas(Nomina);*/
        }

        internal static string CambiaNomina(ref Nomina nomina, byte opcion)
        {
            String cadena = null;
            switch (opcion)
            {
                case 0:
                    cadena = "Modificación cancelada";
                    break;
                //Modificación de los datos
                case 1:
                    Interfaz.SolicitarHoras(ref nomina);
                    cadena = "Horas modificadas con éxito";
                    break;
                case 2:
                    Interfaz.SolicitarPrecio(ref nomina);
                    cadena = "Precio de la hora de trabajo modificado con éxito";
                    break;
                case 3:
                    Interfaz.SolicitarRetencion(ref nomina);
                    cadena = "Porcentaje de retención por impuestos modificado con éxito";
                    break;
            }
            return cadena;
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
            string pass = ConfigurationManager.AppSettings["Password"];
            if (password.Length > 3 && password.Length <= 6)
            {
                if (password.Equals(pass))
                {
                    return true;
                }
            }
            return false;
        }

        public static void ModificarContraseña()
        {
            string nuevapass = Interfaz.PedirContraseñaModificar();
            if (ConfigurationManager.AppSettings["Password"] == null)
            {
                throw new ArgumentNullException("La contraseña ", "<" + "Password" + "> No existe en la configuración.");
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Password"].Value = nuevapass;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
        #endregion

        #region GET VALORES POR DEFECTO APP.CONF
        public void getHextras() // obtiene el valor por defecto de las horas extraordinarias
        {

        }

        public static string getJornada()//Obtiene el valor por defecto de la jornada
        {
            string jor = ConfigurationManager.AppSettings["Jornada"];
            return jor;
        }
        public string getRetencion()//Obtiene el valor por defecto del porcentaje de retención
        {
            string ret = ConfigurationManager.AppSettings["Retenciones"];
            return ret;
        }
        #endregion

        public static void InicializarComponentes()
        {
            Ficheros.setConfig();
            Ficheros.ExistOrEmptyEMP();
        }
    } // FIN CLASE
} // FIN ESP. NOMBRES