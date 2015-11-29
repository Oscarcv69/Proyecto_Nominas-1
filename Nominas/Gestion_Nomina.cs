using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Gestion_Nomina
    {
/*
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
        private static bool ExisteNomina(Nomina[] Nomina, short semana)
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
        */
    }
}
