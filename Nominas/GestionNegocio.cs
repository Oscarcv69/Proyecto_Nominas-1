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
        public static void NuevoTrabajador(ref Trabajador[] trabajador)
        {
            Trabajador trabtemp;
            Trabajador[] copia = null;
            bool salida = false;
            string mensaje = null;

            // Entrada de Datos
            do
            {
                // ENTRADA

                trabtemp = Interfaz.DatosTrabajador();

                if (!ExisteTrabajador(trabajador, trabtemp.dni_pre))
                {

                    if (trabajador == null)
                    {
                        trabajador = new Trabajador[1];
                    }
                    else
                    {
                        copia = new Trabajador[trabajador.Length];
                        trabajador.CopyTo(copia, 0);
                        trabajador = new Trabajador[trabajador.Length + 1];
                        copia.CopyTo(trabajador, 0);
                        copia = null;
                    }

                    trabajador[trabajador.Length - 1] = trabtemp;

                    mensaje = "\n\t Trabajador registrado correctamente";
                }
                else
                {
                    mensaje = "\n\t El Trabajador se encuentra registrado";
                }

                // SALIDA
                mensaje = mensaje + "\n\n\t Desea registrar otro Trabajador (s/n): ";


                salida = Interfaz.Continuar(mensaje) ? false : true;

            } while (!salida);

        }
        // Método público: Operar con cuenta cliente
        public static void OperarTrabajadores(ref Trabajador[] trabjador)
        {
            string dni = null;      // Empleado para la búsqueda del cliente
            float cantidad = 0.0F;  // Cantidad a ingresar o reintegrar
            byte opcion;            // Opción del menú
            int indice = 0;         // Posición del cliente en la lista de clientes
            string mensaje = null;

            // ENTRADA: Solicitud del DNI del cliente para operar
            dni = Interfaz.SolicitarDni();

            // PROCESO
            // Comprobación de la existencia del Cliente
            if (ExisteTrabajador(trabjador, dni, ref indice))
            {
                opcion = Interfaz.MenuOperar(trabjador[indice]);

                switch (opcion)
                {
                    // Salida del Menú de Operación
                    case 0:
                        mensaje = "\n\tOperación Cancelada\n";
                        break;
                    // Ingreso en Cuenta del Cliente
                    case 1:
                        if (cantidad > 0)
                        {
                            Interfaz.SolicitarCantidad(trabjador[indice], ref cantidad, 1);

                            trabjador[indice].RecargarSaldo(cantidad);

                            mensaje = "\n\tIngreso realizado con éxito\n";
                        }

                        break;
                    // Reintegro en Cuenta del Cliente
                    case 2:
                        Interfaz.SolicitarCantidad(trabjador[indice], ref cantidad, 2);
                        if (trabjador[indice].pSaldo >= cantidad)
                        {
                            trabjador[indice].ExtraerSaldo(cantidad);
                            mensaje = "\n\tReintegro realizado con éxito\n";
                        }
                        else
                        {
                            mensaje = "\n\tNo tiene suficiente saldo en la cuenta\n";
                        }



                        break;
                }
            }
            else
            {
                mensaje = "\n\tERROR: No se encuentra el Cliente\n";
            }

            // SALIDA
            mensaje = mensaje + "\n\tPulse ENTER para continuar";
            Interfaz.Continuar(mensaje);

        }
        #endregion

        #region Gestion Operaciones - LLAMADA INTERFAZ
        public static void GestionOperaciones(int numb, ref bool flag)
        {
            switch (numb)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:
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
}
