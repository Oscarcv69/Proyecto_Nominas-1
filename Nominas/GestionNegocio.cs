using System;
using System.Configuration;


namespace Nominas
{
    class GestionNegocio
    {

        #region GestionNegocio - LLAMADA INTERFAZ
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
        #endregion
    } // FIN CLASE
} // FIN ESP. NOMBRES