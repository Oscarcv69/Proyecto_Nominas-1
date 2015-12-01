using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
	//Comentario
    class Nomina
    {
        private int horas;
        private int hextras;
        private float precio;
        private float salarioExtra;
        private float salarioBruto;
        private float salarioNeto;
        private float impuestos;

        // Variables del archivo de configuración
        private int jornada_pre;
        private float hextras_pre;
        private float retencion_pre;
        //

        public Nomina()
        {
            this.horas = 0;
            this.hextras = 0;
            this.salarioExtra = 0;
            this.salarioBruto = 0;
            this.salarioNeto = 0;
            this.impuestos = 0;

            this.jornada_pre = 0;
            this.hextras_pre = 0.0F;
            this.precio = 0;
            this.retencion_pre = 0.0F;
        }


        public int Horas_pre
        {
            get { return horas; }
            set {
                if (value >= 0)
                {
                    horas = value;
                } else
                {
                    throw new Exception("Las horas no pueden ser menores que 0");
                }

                if(value <= 168) {
                    horas = value;
                }
                else {
                    throw new Exception("No puede haber más de 168 horas.");
                }
            }
        }

        public int HExtra_pre
        {
            get { return hextras; }
            set
            {
                if (value >= 0)
                {
                    hextras = value;
                }
                else
                {
                    throw new Exception("Las horas no pueden ser menores que 0");
                }
            }
        }


        public float SalExtra_pre
        {
            get { return salarioExtra; }
            set
            {
                if (value >= 0)
                {
                    salarioExtra = Single.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("El salario extra no pued ser menor que 0");
                }
            }
        }

        public float SalBruto_pre
        {
            get { return salarioBruto; }
            set
            {
                if (value >= 0)
                {
                    salarioBruto = Single.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("El salario bruto no puede ser menor que 0");
                }
            }
        }

        public float SalRetencion_pre
        {
            get { return impuestos; }
            set
            {
                if (value >= 0)
                {
                    impuestos = Single.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("La retención no puede ser menor que 0");
                }
            }
        }

        public float SalNeto_pre
        {
            get { return salarioNeto; }
            set
            {
                if (value >= 0)
                {
                    salarioNeto = Single.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("El salario neto no puede ser menor que 0");
                }
            }
        }

        public int JornadaPre {
            get { return jornada_pre; }
            set
            {
                if (value >= 0 && value <= 40)
                {
                    jornada_pre = Int32.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("La jornada predeterminada no puede ser menor que 0 ni mayor que 40 horas.");
                }

            }

        }

        public float HextrasPre
        {
            get { return hextras_pre; }
            set
            {
                if (value >= 0)
                {
                    hextras_pre = Single.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("El valor de horas extras no puede ser menor que 0");
                }

            }

        }

        public float PrecioPre
        {
            get { return precio; }
            set
            {
                if (value >= 0)
                {
                    precio = Single.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("El precio no puede ser menor que 0");
                }
            }
        }

        public float RetencionPre
        {
            get { return retencion_pre; }
            set
            {
                if (value >= 0)
                {
                    retencion_pre = Single.Parse(value.ToString());
                }
                else
                {
                    throw new Exception("El valor de la retención no puede ser menor que 0");
                }

            }
        }
    }
}
