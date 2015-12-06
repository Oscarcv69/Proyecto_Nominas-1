using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///<summary> Informe:
/// Esta clase recoge todos los datos de la nómina procedentes de la clase Nomina
/// y los procesa, calculando los diversos capos que van a conformar la nómina definitiva.
/// Asimismo hay métodos que incializan la nómina desde los campos del archivo de configuración.
/// </summary>
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


        #region GESTION NOMINAS - ANTONIO BAENA


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


        /*OrdenaNomina: El método coge un array de semanas y lo ordena, 
        * creando huecos allí donde sea necesario para insertar después 
        * las semanas y completarlo.*/
        public static void OrdenaNomina(ref Nomina[] nomina)
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

            
            nominatemp = new Nomina[max];//Redimensionamos el array para hacer sitio (usando la semana más alta)
            for (int i = 0; i < nomina.Length; i++)//Recorremos el array recolocando las semanas según el ID en el orden correspondiente
            {
                nominatemp[(nomina[i].ID_pre) - 1] = nomina[i];
            }
            //Dehacemos la copia de la nómina y dejamos la nominatemp a null para ahorrar memoria.
            nomina = null;
            nomina = new Nomina[nominatemp.Length];
            nomina = nominatemp;
            nominatemp = null;
        }

        #region Cálculo de las variables - Antonio Baena
        //Cálculo de las horas extra
        private static void CalculoExtra(ref Nomina nomina)
        {
            if (nomina.Horas_pre > nomina.JornadaPre)
            {
                nomina.HExtra_pre = (short)(nomina.Horas_pre - nomina.JornadaPre);
            }
            else { nomina.HExtra_pre = 0; }


        }
        //Calculo del salario Extra
        private static void CalculoSalarioExtra(ref Nomina nomina)
        {
            nomina.SalExtra_pre = nomina.HExtra_pre * nomina.PrecioPre * nomina.HextrasPre;
        }
        //Cálculo del salario bruto
        private static void CalculoSalarioBruto(ref Nomina nomina)
        {
            if (nomina.HExtra_pre > 0)
            {
                nomina.SalBruto_pre = (nomina.JornadaPre * nomina.PrecioPre) + nomina.SalExtra_pre;

            }
            else
            {
                nomina.SalBruto_pre = (nomina.Horas_pre * nomina.PrecioPre);
            }

        }
        //Cálculo de las retenciones
        private static void CalculoRetenciones(ref Nomina nomina)
        {
            nomina.SalRetencion_pre = nomina.SalBruto_pre * nomina.RetencionPre;

        }
        //Cálculo del salario Neto
        private static void CalculoSalarioNeto(ref Nomina nomina)
        {
            nomina.SalNeto_pre = nomina.SalBruto_pre - nomina.SalRetencion_pre;
        }
        //CalculoParcial: Con este método almacenamos en el array Nomina los valores correspondientes a cada semana.
        internal static void CalculaParcial(ref Nomina[] Nomina)
        {
            for (int i = 0; i < Nomina.Length; i++) //Recorremos el array y en aquellas posiciones en las que la 
            {                                       //semana no es nula se almacenan los correspondientes valores.
                if (Nomina[i] != null)
                {
                    CalculoExtra(ref Nomina[i]);
                    CalculoSalarioBruto(ref Nomina[i]);
                    CalculoSalarioExtra(ref Nomina[i]);
                    CalculoRetenciones(ref Nomina[i]);
                    CalculoSalarioNeto(ref Nomina[i]);
                }
            }
        }
        //Calculo de los totales de nomina: En este método se calculan los correspondientes valores totales (o promedio) de la nómina mensual
        internal static float CalculaTotal(Nomina[] Nomina, int v)
        {
            float cadena = 0;
            switch (v) //Para facilitar la ejecución realizamos el cálculo mediante 
            {
                case 1:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        if (Nomina[i] != null)
                        {
                            cadena += Nomina[i].Horas_pre;
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        if (Nomina[i] != null)
                        {
                            cadena += Nomina[i].PrecioPre;
                        }
                    }

                    break;
                case 3:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        if (Nomina[i] != null)
                        {
                            cadena += Nomina[i].HExtra_pre;
                        }
                    }
                    break;
                case 4:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        if (Nomina[i] != null)
                        {
                            cadena += Nomina[i].SalExtra_pre;
                        }
                    }
                    break;
                case 5:

                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        if (Nomina[i] != null)
                        {
                            cadena += Nomina[i].SalBruto_pre;
                        }
                    }
                    break;
                case 6:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        if (Nomina[i] != null)
                        {
                            cadena += Nomina[i].SalRetencion_pre;
                        }
                    }
                    break;
                case 7:
                    for (int i = 0; i < Nomina.Length; i++)
                    {
                        if (Nomina[i] != null)
                        {
                            cadena += Nomina[i].SalNeto_pre;
                        }
                    }
                    break;
            }
            return cadena;
        }
        #endregion

        #region Gestión de las Nóminas - Antonio Baena
        //Método que comprueba si una determinada semana ya existe en la nómina
        public static bool ExisteNomina(ref Nomina[] Nomina, int semana)
        {
            bool existe = false;
            if (Nomina.Length == 0) //Si el array con las nóminas está vacio se crea con tantos espacios como semana queremos comprobar
            {
                Nomina = new Nomina[semana];
                existe = false;
            }
            else if (Nomina.Length < semana) //Si la longitud del Array de Nóminas es menor que la semana que queremos introducir.
            {
                existe = false;//Indica que la semana no existe
            }
            else if (Nomina[semana - 1] != null)//Si el espacio correspondiente a la semana dentro del array no es nulo, entonces existe.
            {
                existe = true;
            }
            return existe;
        }
        //Método que comprueba si las seis semanas (máximo de semanas que tiene un mes) han sido rellenadas
        public static bool LimiteSemanas(Nomina[] Nomina)
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

            else
            {
                limite = false;
            }
            return limite;
        }
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
        //Añadir semanas a la nómina. Pasamos tanto la nómina temporal como un la semana a añadir mediante referencias.
        public static void CreaSemana(ref Nomina[] Nomina, ref Nomina semana)
        {
            String cadena = null;

            Nomina[] nomcop = null;

            if (semana.ID_pre < Nomina.Length) //Si el número de semana es menor que la longitud de la semana, se inserta en su posición.
            {
                Nomina[semana.ID_pre - 1] = semana;
            }
            else //En caso contrario redimensionamos la nómina para hacer sitio a la nueva semana y se inserta en su posición.
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
        //Método para buscar una semana determinada en el array
        private static bool BuscaSemana(Nomina[] nomina, ref int semana)
        {
            bool ctrl = false;
            for (int i = 0; i < nomina.Length; i++)//Recorre el array buscando una semana no nula y que coincida el ID con el valor 
            {                                      //de la semana que se pasa como parámetro.
                if (nomina[i] != null && semana == nomina[i].ID_pre)
                {

                    semana = i; //En este caso se iguala la semana al valor de la posición del array, 
                    ctrl = true; //y se devuelve el true, es decir que la semana está en el array
                }
                
            }
            if (!ctrl)
            {
                Interfaz.Error("Lo siento, no existe la semana");

            }

            return ctrl;
        }
        //Método de modificacion de nómina semanal
        public static void CambiaSemana(ref Nomina[] Nomina)
        {
            int semana = 0;
            int opcion = 0;
            String cadena = null;


            //Entrada de Datos
            semana = Interfaz.QueSemana(Nomina); //Se pide la semana mediante el método de interfaz
            if (BuscaSemana(Nomina, ref semana)) //A continuación se lanza el método Buscasemana 
            {                                    //que nos devuelve la posición del array en la que está la semana y si existe.
                opcion = Interfaz.NominaModificar(Nomina[semana]); //Si existe se lanza la interfaz para seleccionar la opción que vamos a modificar
            }
            else { opcion = 0; } //Si no se encuentra la semana se pasa la opción de abortar la modificación


            //PROCESO
            switch (opcion)
            {
                case 0:
                    cadena = "Modificación cancelada";
                    break;
                //Modificación de los datos
                case 1://Lanza la Interfaz para modificar las horas
                    Nomina[semana].Horas_pre = Interfaz.SolicitarHoras();
                    cadena = "Horas modificadas con éxito.";
                    break;
                case 2://Lanza la Interfaz para modificar el precio de la hora.
                    Nomina[semana].PrecioPre = Interfaz.SolicitarPrecio();
                    cadena = "Precio de la hora de trabajo modificado con éxito.";
                    break;
                case 3://Lanza la Interfaz para modificar el precio de la hora.
                    Nomina[semana].JornadaPre = Interfaz.SolicitarJornada();
                    cadena = "Jornada modificada con éxito.";
                    break;
                case 4://Lanza la Interfaz para modificar el precio de la hora.
                    Nomina[semana].RetencionPre = Interfaz.SolicitarRetencion();
                    cadena = "Retención modificada con éxito.";
                    break;
            }
            Interfaz.Continuar(cadena);
        }
        // Método de eliminacion de toda la nomina
        public static void ProcesoEliminarNomina(ref Nomina[] Nomina)
        {
            for (int i = 0; i < Nomina.Length; i++)//Se recorre el array haciendo nula cada entrada del mismo
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
            else//Si la semana existe
            {
                copiaNomina = new Nomina[Nomina.Length]; //Creamos un Array en el que copiamos la nómina


                for (i = 0; i <= copiaNomina.Length - 1; i++, j++)
                {
                    if (i != (semana - 1)) //En caso que el contador sea distinto a la posición de la seman en el array se copia en copianomina
                    {
                        copiaNomina[j] = Nomina[i];
                    }
                }
                //Array dinámico (Una vez copiado todo, redimensionamos el array eliminando una seman y vaciándolo
                Nomina = new Nomina[copiaNomina.Length];
                copiaNomina.CopyTo(Nomina, 0); //Copiamos el array copia en la nómina
                //Ponemos el array de copia en Null para ahorrar memoria
                copiaNomina = null;
                cadena = "\n\t\t Semana eliminada con éxito";
            }
            cadena += "\n\t\tPulse ENTER para continuar\n";
            Interfaz.Continuar(cadena);
        }
        #endregion
    }
}
#endregion