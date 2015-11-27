using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nominas
{
    class GestionNegocio
    {

        #region Gestion Operaciones - LLAMADA INTERFAZ
        public static void GestionOperaciones(int numb, ref bool flag)
        {
            Trabajador[] listaTrabajador = null;
            switch (numb)
            {
                //Agregar trabajadores
                case 1:
                    NuevoTrabajador(ref listaTrabajador);
                    break;
                //Modificar trabajadores
                case 2:

                    break;

                //Eliminar trabajadores 
                case 3:
                    BorrarTrabajador(ref listaTrabajador);
                    break;

                //Modificar Contraseña
                case 4:
                    ModificarContraseña();
                    break;
                case 5:
                   ListarTrabajadores(listaTrabajador);
                    break;
                case 6:    
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
                throw new ArgumentNullException("La contraseña ", "<" + "Password" + "> does not exist in the configuration. Update failed.");
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Password"].Value = nuevapass;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
    #endregion
}
