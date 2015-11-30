using System;
using System.Configuration;


namespace Nominas
{
    class GestionNegocio
    {

        #region GestionNegocio - LLAMADA INTERFAZ (ÓSCAR)
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
/*
        public static void GestionNominas(int numb, ref bool flag)
        {
            Nomina[] Nomina = null;
            Nomina = Ficheros.getNomina();
            Byte semana = 0;


            switch (numb)
            {

                //Necesitamos pedir antes el DNI del trabajador para operar con sus nóminas
                case 0: // SALIR
                    flag = true;
                    break;
                //Introducir nóminas
                case 1:
                    semana = Interfaz.PedirSemana();
                    Gestion_Nomina.NuevaSemana(ref Nomina, semana);
                    break;
                //Modificar Nóminas
                case 2:
                    Gestion_Nomina.CambiaSemana(ref Nomina);
                    break;

                //Eliminar Nómina 
                case 3: 
                    Gestion_Nomina.eliminarNomina(ref Nomina, semana);
                    break;
                    //Lanza Submenú Mostrar Nómina
                case 4: Interfaz.SubmenuMostrarNomina();
                    break;
                //Cerrar Nómina del Mes
                case 5:
                    Gestion_Nomina.CierraNomina(ref Nomina);
                    break;
                //Mostrar Nómina temporal
                case 6:
                    Gestion_Nomina.CalculaParcial(ref Nomina);
                    Interfaz.MostrarNomina(Nomina);
                    break;
                //Mostrar Histórico Nóminas
                case 7:
                    Gestion_Nomina.ListarNominaHist();//TODO: POR DESARROLLAR
                    break;

            }
            Ficheros.GuardarNominas(Nomina);
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
        */
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

       
        /*  ANTONIO - Métodos para recuperar los datos de app.conf
public void getJornada()//Obtiene el valor por defecto de la jornada
{

}
public void getRetencion()//Obtiene el valor por defecto del porcentaje de retención
{

}
public void getPrecio()//Obtiene el valor por defecto del precio por hora
{

}*/

        #endregion
    } // FIN CLASE
} // FIN ESP. NOMBRES
        #endregion
