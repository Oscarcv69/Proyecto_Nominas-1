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
                case 0:
                    flag = true;
                    break;
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

                //Modificar archivo de configuracion
                case 3:
                    string name = null, valor = null;
                    
                    Interfaz.PedirDatosArchivoConf(ref name, ref valor);
                    Ficheros.ModConfig(name, valor);
                    break;
                    //Eliminar nominas
                case 4:
                    int ordinal = 0, opcion = 0;
                    opcion = Interfaz.EliminarSemanaOpcion();
                    if (opcion ==1)
                    {
                        ordinal = Interfaz.EliminarSemana();
                    }
                
                   Gestion_Nomina.eliminarNomina(ref Nomina, ordinal, opcion); 
                    break;
                //Mostrar Nómina Temporal
                case 5:
                    Gestion_Nomina.CalculaParcial(ref Nomina);
                    Console.WriteLine(Interfaz.MostrarNomina(Nomina));
                    Console.ReadLine();
                    break;
                //Cerrar Nómina del Mes
                case 6:
                    Gestion_Nomina.CierraNomina(ref Nomina);
                    Interfaz.MostrarNomina(Nomina);
                    break;
            
            }
            /* Ficheros.GuardarNominas(Nomina);*/
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

        public static void InicializarComponentes()
        {
            Ficheros.setConfig();
            Ficheros.ExistOrEmptyEMP();
        }
    } // FIN CLASE
} // FIN ESP. NOMBRES