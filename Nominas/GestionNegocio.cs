using System;
using System.Configuration;


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

        public static void GestionNominas(int numb, ref bool flag, string dni)
        {
            string fecha;
            Nomina[] Nomina = null;
            Nomina semana = null;
            try
            {
                Nomina = Ficheros.GetNomina(dni);
                Gestion_Nomina.OrdenaNomina(ref Nomina);

                switch (numb)
                {
                    case 0:
                        flag = true;
                        break;
                    //Introducir nóminas
                    case 1:
                        if (!Gestion_Nomina.LimiteSemanas(Nomina))
                        {
                            semana = Interfaz.PedirSemana(Nomina);
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
                        Gestion_Nomina.CambiaSemana(ref Nomina);
                        Ficheros.GuardarNominaTemporal(ref Nomina);
                        break;

                    //Modificar archivo de configuracion
                    case 3:
                        int option = 0;
                        float valor =  0;
                        string pass = null;
                        pass = Interfaz.PedirContraseña();
                        if (GestionNegocio.ValidarContraseña(pass))
                        {
                            Interfaz.PedirDatosArchivoConf(ref option, ref valor);
                            Ficheros.ModConfig(option, valor);                
                        } else
                        {
                            Interfaz.Error("La contraseña no coincide.");
                            Interfaz.Continuar();
                            break;
                        }
                        break;
                    //Eliminar nominas
                    case 4:
                        int ordinal = 0, opcion = 0;
                        opcion = Interfaz.EliminarSemanaOpcion();
                        if (opcion == 1)
                        {
                            ordinal = Interfaz.EliminarSemana(); // ELIMINA UNA SEMANA DE LA NOMINA
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
                        Console.WriteLine(Interfaz.MostrarNominaTemporal(Nomina, dni));
                        Console.ReadLine();
                        break;
                    //Cerrar Nómina del Mes
                    case 6:
                        string cadena = null;
                        Gestion_Nomina.CalculaParcial(ref Nomina);
                        cadena = Interfaz.MostrarNomina(Nomina, dni);
                        cadena += Interfaz.CierreMes(Nomina);
                        if (!Interfaz.Confirmar())
                        {
                            //Almacena en el fichero
                            fecha = Interfaz.Pidefecha();
                            Ficheros.CerrarNomina(cadena, fecha);
                            //Eliminar fichero
                            Ficheros.BorrarTemporal(dni);
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
            Ficheros.CheckConfig();
            Ficheros.ExistOrEmptyEMP();
            Ficheros.CheckArchivoPass();
        }
    } // FIN CLASE
} // FIN ESP. NOMBRES