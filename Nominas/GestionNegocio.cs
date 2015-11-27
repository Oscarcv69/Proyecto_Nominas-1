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
        private static int CalculoExtra(int horas)
        {
            extra = horas - jornada;

            return extra;

        }
        //Cálculo del salario bruto
        private static float CalculoSalarioBruto(int horas, int jornada, float precio)
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
        private static float CalculoSalarioExtra(int extra, float precio)
        {
            return (extra * precio * 1.5F);
        }
        //Cálculo del salario Neto
        private static float CalculoSalarioNeto(float bruto, float retenciones)
        {
            retenciones = CalculoRetenciones(bruto, retenciones);
            neto = bruto - retenciones;
            return neto;
        }
        //Cálculo de las retenciones
        private static float CalculoRetenciones(float bruto, float retenciones)
        {
            retenciones = bruto * retenciones;
            return retenciones;
        }

        //Métodos para la gestión de las nóminas
        //Cálculo de las semanas del mes
        private static short CalculoSemanas(short anho, short mes)
        {
            //Control de errores
            if (mes<1 && mes >12)
            {
                throw new Exception("Ha introducido un valor erróneo, por favor, introduzca un mes válido");
            }
            
            short semanas;
            int dias = System.DateTime.DaysInMonth(anho, mes);
            semanas = Convert.ToInt16(dias / 7);
            DateTime fecha = new DateTime(anho, mes, 1);

            //Rectificación del número de semanas según donde empieza el mes.
            switch (dias)
            {
                case 28:
                    if (fecha.DayOfWeek != DayOfWeek.Monday){

                        semanas++;
                    }
                    break;
                case 30: if (fecha.DayOfWeek == DayOfWeek.Sunday)
                    {
                        semanas++;
                    }
                    break;
                case 31:
                    if (fecha.DayOfWeek == DayOfWeek.Sunday ||fecha.DayOfWeek == DayOfWeek.Sunday)
                    {
                        semanas++;
                    }
                    break;
             }
            return semanas;

        }
            
        //Método de creación de semanas
        public static void NuevaSemana(ref Nomina[] Nomina, short semana)
        {
            Nomina nominastemp;
            Nomina[] nomcop = null;
            String cadena;
            Boolean ctrl = false;
            
            //Entrada de Datos
            do
            {
                //ENTRADA
                nominastemp = Interfaz.DatosNomina();
                if (!ExisteNomina(semana))
                {
                    if (Nomina == null)
                    {
                        Nomina = new Nomina[1];
                    }
                    else
                    {
                        nomcop = new Nomina[Nomina.Length];
                        Nomina.CopyTo(nomcop, 0);
                        Nomina = new Nomina[Nomina.Length + 1];
                        nomcop.CopyTo(Nomina, 0);
                        nomcop = null;
                    }
                    Nomina[Nomina.Length - 1] = nominastemp;
                cadena = "\n\t Se ha añadido la cotización de la semana número: "+semana;
            }
                else
                {
                cadena = "\n\t La nómina correspondiente a la "+semana + " se encuentra registrada en el sistema";
            }

            // SALIDA
            cadena += "\n\n\t ¿Desea registrar otra semana? (s/n): ";


            ctrl = Interfaz.Continuar(cadena) ? false : true;

        } while (!ctrl);

          
}

        //Comprobación existe la semana
        private static bool ExisteNomina(Nomina[] Nomina, short semana)
        {
            bool existe = false;
            if (Nomina[semana]!=null)
            {
                existe = true;
            }
            return existe;
        }

        //Método de modificacion de nómina semanal
        private static void CambiaSemana(Nomina[] Nomina)
        {
            short semana = 0;
            short opcion = 0;
            String cadena = null;
            short horas = 0;
            float precio = 0.0F;
            float porcentaje = 0.0F;

            //Entrada de Datos
            semana = Interfaz.PedirSemana();
            //PROCESO
            //Comprobamos que existe la semana
            if (!ExisteNomina(Nomina, semana))
            {
                cadena = "Semana no encontrada";
            }
            else
            {
                opcion = Interfaz.NominaModificar(Nomina[semana]);
                switch (opcion)
                {
                    case 0:
                        cadena = "Modificación cancelada";
                        break;
                    //Modificación de los datos
                    case 1:
                        Interfaz.SolicitarHoras(Nomina[semana], ref horas);
                        cadena = "Horas modificadas con éxito";
                        break;
                    case 2:
                        Interfaz.SolicitarPrecio(Nomina[semana], ref precio);
                        cadena = "Precio de la hora de trabajo modificado con éxito";
                        break;
                    case 3:
                        Interfaz.SolicitarRetencion(Nomina[semana], ref porcentaje);
                        cadena = "Porcentaje de retención por impuestos modificado con éxito";
                        break;
                }
            }
            cadena += "Pulse ENTER para continuar";
            Interfaz.Continuar(cadena);
        }
        //Método de eliminación de nómina

        //Cerrar nómina
        //Serializar nómina
        //Convertir nómina a texto.
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
