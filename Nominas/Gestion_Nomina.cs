using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Gestion_Nomina
    {

        //Datos de nómina a almacenar en el fichero temporal
        private static int horas = 0; //Total de horas trabajadas en la semana
        private static int extra = 0; //Horas extra trabajadas en la semana
        private static float precio = 0; //Precio de la hora trabajada
        private static int jornada = 0; //Horas a partir de las cuales se consideran extras
        private static float horasExtra = 0.0F;//Multplicador de precio para horas extra

        //Datos de Nómina a calcular sólo en el momento de cerrar la nómina
        private static float salarioExtra = 0.0F; //Calculo de las hora extra
        private static float bruto = 0.0F; //Calculo del salario bruto total
        private static float neto = 0.0F; //Calculo del salario neto total (Bruto-renteciones)
        private static float retenciones = 0.0F; //Calculo de retenciones salariales


        #region GESTION NOMINAS - ANTONIO


        //Inicialización de nóminas
        public static void InicializaNomina(ref Nomina nomina)
        {
            //Cargamos los datos por defecto de app.config
            Ficheros.getConfig(ref jornada, ref horasExtra, ref retenciones);
            
            //Cargamos los datos en el objeto nomina desde la referencia
            horas = nomina.Horas_pre;
            extra = nomina.HExtra_pre;
            precio = nomina.PrecioPre;
            salarioExtra = nomina.SalExtra_pre;
            bruto = nomina.SalBruto_pre;
            neto = nomina.SalNeto_pre;
        }


        /*CargaNomina: El método coge un array de semanas y lo ordena, 
        * creando huecos allí donde sea necesario para insertar después 
        * las semanas y completarlo.*/
        public static void CargaNomina(ref Nomina[] nomina)
        {
            Nomina[] nominatemp = null;
            int max = 0;

            //Recorremos el array de nóminas para ver la semana más alta que hemos guardado
            for (int i = 0; i < nomina.Length; i++)
            {
                if (nomina[i] != null && max < nomina[i].ID_pre)
                {
                    max = nomina[i].ID_pre;
                }

            }

            //Redimensionamos el array para hacer sitio (usando la semana más alta)
            nominatemp = new Nomina[max];
            for (int i = 0; i < nomina.Length; i++)
            {
                nominatemp[(nomina[i].ID_pre) - 1] = nomina[i];
            }
            nomina = null;
            nomina = new Nomina[nominatemp.Length];
            nomina = nominatemp;
            nominatemp = null;
        }

        #region Cálculo de las variables - Antonio Baena
        //Cálculo de las horas extra
        private static int CalculoExtra(int hora, int jornada)
        {
            extra = (short)(hora - jornada);
            return extra;

        }
        //Calculo del salario Extra
        private static float CalculoSalarioExtra(int extra, float precio)
        {
            return (extra * precio * horasExtra);
        }
        //Cálculo del salario bruto
        private static float CalculoSalarioBruto(int horas, int jornada, float precio)
        {
            if (horas > jornada)
            {
                extra = CalculoExtra(horas, jornada);
                salarioExtra = CalculoSalarioExtra(extra, precio);
                bruto = jornada + salarioExtra * precio;
            }
            else
            {
                bruto = horas * precio;
            }
            return bruto;
        }
        //Cálculo de las retenciones
        private static float CalculoRetenciones(float bruto, float retenciones)
        {
            retenciones = bruto * retenciones;
            return retenciones;
        }
        //Cálculo del salario Neto
        private static float CalculoSalarioNeto(float bruto, float retenciones)
        {
            retenciones = CalculoRetenciones(bruto, retenciones);
            neto = bruto - retenciones;
            return neto;
        }
        #endregion

        #region Gestión de las Nóminas - Antonio Baena

        #region Cálculo de las semanas - Veremos si  no lo borramos
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
        #endregion

        //Calculo de los parciales de nomina
        internal static void CalculaParcial(ref Nomina[] Nomina)
        {
            for (int i = 0; i < Nomina.Length; i++)
            {
                if (Nomina[i] != null)
                {
                    Nomina[i].HExtra_pre = CalculoExtra(horas, jornada);
                    Nomina[i].SalBruto_pre = CalculoSalarioBruto(horas, jornada, precio);
                    Nomina[i].SalExtra_pre = CalculoSalarioExtra(Nomina[i].Horas_pre, precio);
                    Nomina[i].SalRetencion_pre = CalculoRetenciones(bruto, retenciones);
                    Nomina[i].SalNeto_pre = CalculoSalarioNeto(bruto, retenciones);
                }
            }
        }

        //Calculo de los totales de nomina
        internal static float CalculaTotal(Nomina[] Nomina, int v)
        {
            float cadena = 0;
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

        //Método de creación de semanas
        public static void Grabar(ref Nomina[] Nomina, ref Nomina semana)
        {
            String cadena = null;

            Nomina[] nomcop = null;

            if (semana.ID_pre < Nomina.Length)
            {
                Nomina[semana.ID_pre - 1] = semana;
            }
            else
            {
                nomcop = new Nomina[Nomina.Length];
                Nomina.CopyTo(nomcop, 0);
                Nomina = new Nomina[semana.ID_pre + 1];
                nomcop.CopyTo(Nomina, 0);
                nomcop = null;
            }
            Nomina[semana.ID_pre - 1] = semana;

            // SALIDA
            cadena += "\n\n\t Se ha añadido la semana " + semana.ID_pre;
        }

        //Comprobación existe la semana
        public static bool ExisteNomina(ref Nomina[] Nomina, int semana)
        {
            bool existe = false;
            if (Nomina.Length == 0)
            {
                Nomina = new Nomina[semana];
                existe = false;
            }
            else if (Nomina.Length < semana)
            {
                existe = false;
            }
            else if (Nomina[semana - 1] != null)
            {
                existe = true;
            }
            return existe;
        }

        public static bool LimiteSemanas(Nomina[] Nomina) // COMPRUEBA SI LAS 6 SEMANAS HAN SIDO RELLENADAS
        {
            bool limite = true;
            if (Nomina.Length == 6)
            {
                for (int i = 0; i < Nomina.Length; i++)
                {
                    if (Nomina[i] == null)
                    {
                        limite = false;
                        return limite;
                    }
                }
            }
            return limite;
        }
        //Método de modificacion de nómina semanal

        public static void CambiaSemana(ref Nomina[] Nomina)
        {
            int semana = 0;
            int opcion = 0;
            String cadena = null;


            //Entrada de Datos
            semana = Interfaz.QueSemana(Nomina);

            opcion = Interfaz.NominaModificar(Nomina[semana]);
            //PROCESO
            switch (opcion)
            {
                case 0:
                    cadena = "Modificación cancelada";
                    break;
                //Modificación de los datos
                case 1:
                    Nomina[semana].Horas_pre = Interfaz.SolicitarHoras();
                    cadena = "Horas modificadas con éxito";

                    break;
                case 2:
                    Nomina[semana].PrecioPre = Interfaz.SolicitarPrecio();
                    cadena = "Precio de la hora de trabajo modificado con éxito";
                    break;
            }
            Interfaz.Continuar(cadena);
        }

        // Método de eliminacion de nomina
        public static void ProcesoEliminarNomina(ref Nomina[] Nomina)
        {
            for (int i = 0; i < Nomina.Length; i++)
            {
                Nomina[i] = null;
            }
        }

        //Método de eliminación de semana
        public static void ProcesoEliminarSemana(ref Nomina[] Nomina, int semana)
        {
            int i = 0;
            int j = 0;
            Nomina[] copiaNomina;
            bool existesemana;
            String cadena = "";
            //Existe la semana?
            existesemana = ExisteNomina(ref Nomina, semana);
            if (!existesemana)
            {
                cadena = "\n\t\t La semana no existe";
            }
            else
            {
                copiaNomina = new Nomina[Nomina.Length];


                for (i = 0; i <= copiaNomina.Length - 1; i++, j++)
                {
                    if (i != (semana - 1))
                    {
                        copiaNomina[j] = Nomina[i];
                    }
                }
                //Array dinámico
                Nomina = new Nomina[copiaNomina.Length];
                copiaNomina.CopyTo(Nomina, 0);
                //Ponemos el array de copia en Null para ahorrar memoria
                copiaNomina = null;
                cadena = "\n\t\t Semana eliminada con éxito";
            }
            cadena += "\n\t\tPulse ENTER para continuar\n";
            Interfaz.Continuar(cadena);
        }

        //Cerrar nómina
        public static void CierraNomina(ref Nomina[] Nomina)
        {
            String cadena = null;
            //Calcula nominas semanales
            CalculaParcial(ref Nomina);
            cadena = Interfaz.MostrarNomina(Nomina);
            cadena += Interfaz.CierreMes(Nomina);
            //Confirmacion
            if (Interfaz.Confirmar())
            {
                //Almacena en el fichero
                Ficheros.CerrarNomina(cadena);
                //Eliminar fichero
                Ficheros.BorrarTemporal();//Pasar el dni del trabajador);
            }
            else {
                Interfaz.Continuar();
            }
            
        }//TERMINA
        #endregion
    }
}