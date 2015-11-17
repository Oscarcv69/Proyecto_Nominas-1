using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Nomina
    {
        private int horas = 0;
        private int jornada = 40;
        private int extra = 0;
        private float precio = 0.0F;
        private float retenciones = 0.16F;
        private float bruto =0.0F;
        private float neto=0.0F;
        private float salarioExtra = 0.0F;

        //METODOS

        //Cálculo de las horas extras semanales
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
                bruto = jornada+salarioExtra*precio;
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
        public float CalculoSalarioNeto (float bruto, float retenciones)
        {
            retenciones = CalculoRetenciones(bruto, retenciones);
            neto = bruto -retenciones;
            return neto;
        }

        //Cálculo de las retenciones
        public float CalculoRetenciones(float bruto, float retenciones)
        {
            retenciones = bruto * retenciones;
            return retenciones;
        }



    }
}
