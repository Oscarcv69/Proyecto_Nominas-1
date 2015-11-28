using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nominas {
    class Nomina {

        private short horas;
        private short horasE; //Horas Extra
        private float impuestos;

        private float salario;
        private float salarioE; // Extra
        private float salarioB; // Bruto
        private float salarioN; // Neto

        Regex regex = new Regex("`[0-9]*$"); // regex only numbers

        public Nomina() {

            this.horas = 0;
            this.horasE = 0;
            this.impuestos = 0;
            this.salario = 0;
            this.salarioE = 0;
            this.salarioB = 0;
            this.salarioN = 0;
        }

        #region PROPIEDADES

        public short Horas {

            set {
                this.horas = value;
            }

            get {
                if(!regex.IsMatch(horas.ToString())) {
                    Console.WriteLine("Debes introducir valores numéricos");
                }

                return horas;
            }
        }

        public short HorasE {
            set {
                this.horasE = value;
            }

            get {
                if(!regex.IsMatch(horasE.ToString())) {
                    Console.WriteLine("Debes introducir valores numéricos");
                }

                return horasE;
            }
        }
    }


    #endregion
}

