using System;
using System.Text.RegularExpressions;

namespace Nominas {
    //Comentario
    class Nomina {
        private int ID;
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

        public Nomina() {
            this.ID = 1;
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
        private Regex regexfloat = new Regex(@"\d*\.?\d+?"); // regex float.
        Regex regex = new Regex("^[0-9]*$"); // regex only numbers


        public int ID_pre {
            get { return ID; }
            set {
                if(value < 1 || value >= 7) {
                    throw new Exception("El mes no puede tener menos de una semana o más de seis");
                }

                else if(!regex.IsMatch(ID.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    ID = value;
                }

                if(String.IsNullOrEmpty(ID.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    ID = value;
                }
            }
        }


        public int Horas_pre {
            get { return horas; }
            set {
                if(value >= 0) {
                    horas = value;
                }
                else {
                    throw new Exception("Las horas no pueden ser menores que 0");
                }

                if(value <= 168) {
                    horas = value;
                }
                else {
                    throw new Exception("No puede haber más de 168 horas.");
                }

                if(!regex.IsMatch(horas.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    horas = value;
                }
                if(String.IsNullOrEmpty(horas.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    horas = value;
                }

            }
        }

        public int HExtra_pre {
            get { return hextras; }
            set {
                if(value >= 0) {
                    hextras = value;
                }
                else {
                    throw new Exception("Las horas no pueden ser menores que 0");
                }

                if(!regexfloat.IsMatch(hextras.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    hextras = value;
                }

                if(String.IsNullOrEmpty(hextras.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    hextras = value;
                }
            }
        }


        public float SalExtra_pre {
            get { return salarioExtra; }
            set {
                if(value >= 0) {
                    salarioExtra = Single.Parse(value.ToString());
                }
                else {
                    throw new Exception("El salario extra no pued ser menor que 0");
                }

                if(!regexfloat.IsMatch(salarioExtra.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    salarioExtra = value;
                }
                if(String.IsNullOrEmpty(salarioExtra.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    salarioExtra = value;
                }
            }
        }

        public float SalBruto_pre {
            get { return salarioBruto; }
            set {
                if(value >= 0) {
                    salarioBruto = Single.Parse(value.ToString());
                }
                else {
                    throw new Exception("El salario bruto no puede ser menor que 0");
                }

                if(!regexfloat.IsMatch(salarioBruto.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    salarioBruto = value;
                }

                if(String.IsNullOrEmpty(salarioBruto.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    salarioBruto = value;
                }
            }
        }

        public float SalRetencion_pre {
            get { return impuestos; }
            set {
                if(value >= 0) {
                    impuestos = Single.Parse(value.ToString());
                }
                else {
                    throw new Exception("La retención no puede ser menor que 0");
                }

                if(!regexfloat.IsMatch(impuestos.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    impuestos = value;
                }

                if(String.IsNullOrEmpty(impuestos.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    impuestos = value;
                }


            }
        }

        public float SalNeto_pre {
            get { return salarioNeto; }
            set {
                if(value >= 0) {
                    salarioNeto = Single.Parse(value.ToString());
                }
                else {
                    throw new Exception("El salario neto no puede ser menor que 0");
                }
                if(!regexfloat.IsMatch(salarioNeto.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    salarioNeto = value;
                }

                if(String.IsNullOrEmpty(salarioNeto.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    salarioNeto = value;
                }
            }
        }

        public int JornadaPre {
            get { return jornada_pre; }
            set {
                if(value >= 0 && value <= 40) {
                    jornada_pre = Int32.Parse(value.ToString());
                }
                else {
                    throw new Exception("La jornada predeterminada no puede ser menor que 0 ni mayor que 40 horas.");
                }

                if(!regex.IsMatch(jornada_pre.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    jornada_pre = value;
                }

                if(String.IsNullOrEmpty(jornada_pre.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    jornada_pre = value;
                }

            }

        }

        public float HextrasPre {
            get { return hextras_pre; }
            set {
                if(value >= 0) {
                    hextras_pre = float.Parse(value.ToString());
                }
                else {
                    throw new Exception("El valor de horas extras no puede ser menor que 0");
                }

                if(!regexfloat.IsMatch(hextras_pre.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    hextras_pre = value;
                }
                if(String.IsNullOrEmpty(hextras_pre.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    hextras_pre = value;
                }

            }

        }

        public float PrecioPre {
            get { return precio; }
            set {
                if(value >= 0) {
                    precio = Single.Parse(value.ToString());
                }
                else {
                    throw new Exception("El precio no puede ser menor que 0");
                }

                if(!regexfloat.IsMatch(precio.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    precio = value;
                }

                if(String.IsNullOrEmpty(precio.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    precio = value;
                }
            }
        }

        public float RetencionPre {
            get { return retencion_pre; }
            set {
                if(value >= 0) {
                    retencion_pre = Single.Parse(value.ToString());
                }
                else {
                    throw new Exception("El valor de la retención no puede ser menor que 0");
                }

                if(!regexfloat.IsMatch(retencion_pre.ToString())) {

                    throw new Exception("Introduce sólo valores numéricos");

                }
                else {
                    retencion_pre = value;
                }

                if(String.IsNullOrEmpty(retencion_pre.ToString())) {
                    throw new Exception("La cadena está vacía o es Null.");
                }

                else {
                    retencion_pre = value;
                }

            }
        }
    }
}
