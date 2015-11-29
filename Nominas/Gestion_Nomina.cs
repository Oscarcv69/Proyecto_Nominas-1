using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Gestion_Nomina
    {
        private static byte horas = 0;
        private static byte extra = 0;
        private static byte jornada = 0;
        private static float salarioExtra = 0.0F;
        private static float bruto = 0.0F;
        private static float neto = 0.0F;
        private static byte retenciones = 0;

        #region GESTION NOMINAS - ANTONIO

        //Métodos para el cálculo de la nómina

        //Cálculo de las horas extra
        private static byte CalculoExtra(byte hora)
        {
            horas = hora;
            extra = (byte)(horas - jornada);

            return extra;

        }

        //Cálculo del salario bruto
        private static float CalculoSalarioBruto(byte horas, byte jornada, float precio)
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
        private static float CalculoSalarioExtra(byte extra, float precio)
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
        private static short CalculoSemanas(short anho, byte mes)
        {
            //Control de errores
            if (mes < 1 && mes > 12)
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
                    if (fecha.DayOfWeek != DayOfWeek.Monday)
                    {

                        semanas++;
                    }
                    break;
                case 30:
                    if (fecha.DayOfWeek == DayOfWeek.Sunday)
                    {
                        semanas++;
                    }
                    break;
                case 31:
                    if (fecha.DayOfWeek == DayOfWeek.Sunday || fecha.DayOfWeek == DayOfWeek.Sunday)
                    {
                        semanas++;
                    }
                    break;
            }
            return semanas;

        }

        //Método de creación de semanas
        public static void NuevaSemana(ref Nomina[] Nomina, byte semana)
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
                if (!ExisteNomina(Nomina, semana))
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
                    cadena = "\n\t Se ha añadido la cotización de la semana número: " + semana;
                }
                else
                {
                    cadena = "\n\t La nómina correspondiente a la " + semana + " se encuentra registrada en el sistema";
                }

                // SALIDA
                cadena += "\n\n\t ¿Desea registrar otra semana? (s/n): ";


                ctrl = Interfaz.Continuar(cadena) ? false : true;

            } while (!ctrl);


        }

        //Comprobación existe la semana
        private static bool ExisteNomina(Nomina[] Nomina, byte semana)
        {
            bool existe = false;
            if (Nomina[semana] != null)
            {
                existe = true;
            }
            return existe;
        }

        //Método de modificacion de nómina semanal
        private static void CambiaSemana(Nomina[] Nomina)
        {
            byte semana = 0;
            byte opcion = 0;
            String cadena = null;


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
                cadena = GestionNegocio.OperacionesNomina(opcion);
                Interfaz.Continuar(cadena);

                /* PASAR ESTO A EL METODO GESTIONNEGOCIO.OPERACIONESNOMINA(byte opcion)
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
                cadena += "Pulse ENTER para continuar";*/

            }

        }
        //Método de eliminación de nómina
        private void eliminarNomina(Nomina[] Nomina, byte semana)
        {
            int opcion = 0;
            int i = 0;
            int j = 0;
            Nomina[] copiaNomina;
            bool existesemana;
            String cadena = "";
            //Existe la semana?
            existesemana = ExisteNomina(Nomina, semana);
            if (!existesemana)
            {
                cadena = "La semana no existe";
            }
            else
            {
                copiaNomina = new Nomina[Nomina.Length - 1];

                switch (opcion)
                {
                    case 0:
                        cadena = "Operación abortada";
                        break;
                    case 1:
                        if (semana > Nomina.Length)
                        {
                            cadena = "Esta semana no se ha cargado en el archivo.";
                        }
                        else
                        {
                            for (i = 0; i < Nomina.Length; i++, j++)
                            {
                                if (i != semana)
                                {
                                    copiaNomina[j] = Nomina[i];
                                }
                                else j -= 1;
                            }
                            //Array dinámico
                            Nomina = new Nomina[copiaNomina.Length];
                            copiaNomina.CopyTo(Nomina, 0);
                            //Ponemos el array de copia en Null para ahorrar memoria
                            copiaNomina = null;
                            cadena = "\n\t Semana eliminada con éxito\n";
                        }
                       
                        break;
                    case 2:
                        //Borrar toda la nómina
                        for (i = 0; i < Nomina.Length; i++)
                        {
                            Nomina[i] = null;
                        }
                        break;
                }
                cadena = "Pulse ENTER para continuar";
                Interfaz.Continuar(cadena);
            }
        }

        //Cerrar nómina
        private void CierraNomina(Nomina[] Nomina, Trabajador trabajador)
        {
            //Calcula nomina total
            for (int i = 0; i < Nomina.Length; i++)
            {
                Nomina[i].setHoras(horas);
                Nomina[i].setExtra(CalculoExtra);
                Nomina[i].setBruto(CalculoSalarioBruto);
                Nomina[i].setExtra(CalculoSalarioExtra);
                Nomina[i].setRetenciones(CalculoRetenciones);
                Nomina[i].setNeto(CalculoSalarioNeto);

            }
            //Calcula nomina mensual
            //Muestra datos pantalla
            //Confirmacion
            //Almacena en el fichero
            //Eliminar fichero
        }
            #endregion
        
    }
}
