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
        private short horas;
        private short hextras;
        private short precio;
        private float salarioExtra;
        private float salarioBruto;
        private float salarioNeto;
        private float retencion;
        
        public Nomina()
        {
            this.horas = 0;
            this.hextras = 0;
            this.precio = 0;
            this.salarioExtra = 0;
            this.salarioBruto = 0;
            this.salarioNeto = 0;
            this.retencion = 0;
        }


        public short Horas_pre
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
            }
        }

        public short HExtra_pre
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

        public short Precio_pre
        {
            get { return precio; }
            set
            {
                if (value >= 0)
                {
                    precio = value;
                }
                else
                {
                    throw new Exception("El precio no puede ser menor que 0");
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
                    salarioExtra = value;
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
                    salarioBruto = value;
                }
                else
                {
                    throw new Exception("El salario bruto no puede ser menor que 0");
                }
            }
        }

        public float SalRetencion_pre
        {
            get { return retencion; }
            set
            {
                if (value >= 0)
                {
                    retencion = value;
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
                    salarioNeto = value;
                }
                else
                {
                    throw new Exception("El salario neto no puede ser menor que 0");
                }
            }
        }
    }
}
