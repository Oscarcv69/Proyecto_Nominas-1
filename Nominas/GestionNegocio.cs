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
        private int extra = 0;
        private int jornada = 40;
        private int horas = 0;
        private float precio = 0.0F;
        private float retenciones = 0.16F;
        private float bruto = 0.0F;
        private float neto = 0.0F;
        private float salarioExtra = 0.0F;

        #region GESTION NOMINAS - ANTONIO

        //Métodos para el cálculo de la nómina

        //Cálculo de las horas extra
        public int CalculoExtra(int horas)
        {
            extra = horas - jornada;

            return extra;

        }
        //Cálculo del salario bruto
        public float CalculoSalarioBruto(int horas, int jornada, float precio)
        {
            if (horas > jornada)
            {
                extra = CalculoExtra(horas);
                salarioExtra = CalculoSalarioExtra(extra, precio);
                bruto = jornada + salarioExtra * precio;
            }
            else
            {
                bruto = horas * precio;
            }
            return bruto;
        }
        //Calculo del salario Extra
        public float CalculoSalarioExtra(int extra, float precio)
        {
            return (extra * precio * 1.5F);
        }
        //Cálculo del salario Neto
        public float CalculoSalarioNeto(float bruto, float retenciones)
        {
            retenciones = CalculoRetenciones(bruto, retenciones);
            neto = bruto - retenciones;
            return neto;
        }
        //Cálculo de las retenciones
        public float CalculoRetenciones(float bruto, float retenciones)
        {
            retenciones = bruto * retenciones;
            return retenciones;
        }

        //Métodos para la gestión de las nóminas
        #endregion

        #region GESTION TRABAJADOR - OSCAR
        public static bool ExisteTrabajador(Trabajador[] listaTrabajadores, string dni)
        {
            bool existe = false;
            int indice = 0;

            if (listaTrabajadores != null)
            {
                for (indice = 0; (indice < listaTrabajadores.Length) && !existe; indice++)
                {
                    if (listaTrabajadores[indice].dni_pre.Equals(dni)) existe = true;
                }
            }

            return existe;
        }
        private static bool ExisteTrabajador(Trabajador[] listaTrabajadores, string dni, ref int posicion)
        {
            bool existe = false;    // Control de existencia
            int indice = 0;

            if (listaTrabajadores != null)
            {
                for (indice = 0; (indice < listaTrabajadores.Length) && !existe; indice++)
                {
                    if (listaTrabajadores[indice].dni_pre.Equals(dni))
                    {
                        existe = true;
                        posicion = indice;
                    }
                }
            }

            return existe;
        }
        public static void NuevoTrabajador(ref Trabajador[] listaTrabajadores)
        {
            Trabajador trabtemp;
            Trabajador[] copia = null;
            bool salida = false;
            string mensaje = null;

            // Entrada de Datos
            do
            {
                // ENTRADA

                trabtemp = Interfaz.PlantillaCrearTrabajador();

                if (!ExisteTrabajador(listaTrabajadores, trabtemp.dni_pre))
                {

                    if (listaTrabajadores == null)
                    {
                        listaTrabajadores = new Trabajador[1];
                    }
                    else
                    {
                        copia = new Trabajador[listaTrabajadores.Length];
                        listaTrabajadores.CopyTo(copia, 0);
                        listaTrabajadores = new Trabajador[listaTrabajadores.Length + 1];
                        copia.CopyTo(listaTrabajadores, 0);
                        copia = null;
                    }

                    listaTrabajadores[listaTrabajadores.Length - 1] = trabtemp;

                    mensaje = "\n\t Trabajador registrado correctamente";
                }
                else
                {
                    mensaje = "\n\t El Trabajador se encuentra registrado";
                }

                // SALIDA
                mensaje = mensaje + "\n\n\t Desea registrar otro Trabajador (s/n): ";


                //salida = Interfaz.Continuar(mensaje) ? false : true;
                salida = true;
            } while (!salida);

        }
        // Método para mostrar a todos los trabajadores que hay en la nómina
        public static void ListarTrabajadores(Trabajador[] listaTrabajadores)
        {

            if (listaTrabajadores != null)
            {
                //  Interfaz.MostrarLista(listaTrabajadores);
                // Interfaz.Continuar("\n\tPulse ENTER para continuar");
            }
            else
            {
                // Interfaz.Continuar("\n\tNo hay trabajadores a mostrar. \n\tPulse ENTER para continuar");
            }
        }
        //Método para borrar un trabajador
        public static void BorrarTrabajador(ref Trabajador[] listaTrabajadores)
        {
            Trabajador[] copia;
            int posicion = 0;
            string dni = null;
            bool existe = false;
            bool correcto = false;
            int j = 0;

            //En primer lugar pedimos la contraseña para realizar cambios
            correcto = GestionContraseña();

            //Vamos a pedir el DNI para buscar a la persona que vamos a borrar
            dni = Interfaz.PlantillaBorrarUsuario();

            //En segundo lugar buscamos si existe dicho DNI introducido.
            existe = ExisteTrabajador(listaTrabajadores, dni, ref posicion);

            // Condición para conocer si la cuenta se encuentra sin saldo y además si existe.
            if (correcto == true && existe == true)
            {

                // Creamos un array de Copia para volcar los datos, con longitud de los clientes -1
                copia = new Trabajador[listaTrabajadores.Length - 1];

                /* For para recorrer el array, si encotnramos dicho DNI, borramos dicha posición y datos.*/


                for (int i = 0; i < listaTrabajadores.Length; i++, j++)
                {
                    if (i != posicion)
                    {
                        copia[j] = listaTrabajadores[i];
                    }
                    else j -= 1;
                }
                //Array dinámico
                listaTrabajadores = new Trabajador[copia.Length];
                copia.CopyTo(listaTrabajadores, 0);
                //Ponemos el array de copia en Null para ahorrar memoria
                copia = null;
                //mensaje = "\n\t Cliente borrado con éxito, Pulse ENTER para continuar\n";
            }
            else
            {

               // mensaje = "\n\t ERROR: Asegurese que el DNI existe o que no tienes fondos en su cuenta\n";
            }

          //  Interfaz.Continuar(mensaje);

        }




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
        #endregion
    }
    #endregion
}