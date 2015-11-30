using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Gestion_Nomina
    {
        //TODO: Cargar ajustes por dfecto

         
        private static float horasExtra =0.0F;
        private static short horas = 0; 
        private static short extra = 0; 
        private static short precio = 0; 
        private static float salarioExtra = 0.0F;
        private static float bruto = 0.0F;
        private static float neto = 0.0F;
        private static float retenciones = 0.0F;
        private static int jornada = 0;

        #region GESTION NOMINAS - ANTONIO

       
        //Métodos para el cálculo de la nómina

            //Inicialización de nóminas
            public void InicializaNomina(Nomina nomina)
        {
            Ficheros.getConfig(ref jornada, ref horasExtra, ref retenciones);
            horas = nomina.Horas_pre;
            extra = nomina.HExtra_pre;
            precio = nomina.Precio_pre;
            salarioExtra = nomina.SalExtra_pre;
            bruto = nomina.SalBruto_pre;
            neto = nomina.SalNeto_pre;
        }
        //Fichero temporal lleva las horas por semana

        //Cálculo de las horas extra
        private static short CalculoExtra(short hora)
        {
            horas = hora;
            extra = (short)(horas - jornada);

            return extra;

        }

        //Cálculo del salario bruto
        private static float CalculoSalarioBruto(short horas, int jornada, float precio)
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
        private static float CalculoSalarioExtra(short extra, float precio)
        {
            return (extra * precio * horasExtra);
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
        public static short CalculoSemanas(short anho, short mes)
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
        //Calculo de los parciales de nomina
        internal static void CalculaParcial(ref Nomina[] Nomina)
        {
            for (int i = 0; i < Nomina.Length; i++)
            {
                Nomina[i].Horas_pre = horas;
                Nomina[i].HExtra_pre = CalculoExtra(horas);
                Nomina[i].SalBruto_pre = CalculoSalarioBruto(horas, jornada, precio);
                Nomina[i].SalExtra_pre = CalculoSalarioExtra(Nomina[i].Horas_pre, precio);
                Nomina[i].SalRetencion_pre = CalculoRetenciones();
                Nomina[i].SalNeto_pre = CalculoSalarioNeto();
            }

        }
        //Calculo de los totales de nomina
        internal static string CalculaTotal(Nomina[] Nomina, int v)
        {
            String cadena = null;
            switch (v)
            {
                case 1:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        cadena += Nomina[i].Horas_pre;
                    }
                    break;
                case 2:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        cadena += Nomina[i].HExtra_pre;
                    }
                    break;
                case 3:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        cadena += Nomina[i].SalExtra_pre;
                    }
                    break;
                case 4:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        cadena += Nomina[i].SalBruto_pre;
                    }
                    break;
                case 5:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        cadena += Nomina[i].SalRetencion_pre;
                    }
                    break;
                case 6:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        cadena += Nomina[i].SalNeto_pre;
                    }
                    break;
            }
            return cadena;
        }


        internal static void ListarNominaHist()//TODO: PorDesarrollar
        {
            throw new NotImplementedException();
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
        public static void CambiaSemana(ref Nomina[] Nomina)
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
                cadena = GestionNegocio.CambiaNomina(ref Nomina[semana], opcion);
                Interfaz.Continuar(cadena);

            }

        }
        
        //Método de eliminación de nómina
        public static void eliminarNomina(ref Nomina[] Nomina, byte semana)
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
        public static void CierraNomina(ref Nomina[] Nomina)//TODO: Terminar Desarrollo Confirmación, Guardar y borrar temporal
        {
            //Calcula nominas semanales
            CalculaParcial(ref Nomina);
            Interfaz.MostrarNomina(Nomina);
            //Confirmacion
            //Almacena en el fichero - LLamar metodos de Fran
            //Eliminar fichero - Llamar metodos de Fran
        }//TERMINAR

        
        #endregion

    }
}
