using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nominas
{
    public class Trabajador
    {
        private string dni, nombre, apellidos;
        public Trabajador() {
            this.dni = null;
            this.nombre = null;
            this.apellidos = null;
        }
        public string dni_pre
        {
            get
            {
                return dni;
            }
            set
            {
                int numero = 0;
                if (value.Length < 9)
                {
                    throw new Exception("La longitud debe ser 9");
                }
                else if (!Int32.TryParse(value.Substring(0, 8), out numero) || (Int32.TryParse(value[8].ToString(), out numero)))
                {
                    throw new Exception("DNI Incorrecto: Formato erróneo (12345678A)");
                }
                else
                {
                    dni = value.ToUpper();
                }
            }
        }
        public string nombre_pre
        {
            get
            {
                return nombre;
            }
            set
            {
                if (Regex.IsMatch(value.ToString(), @"^[a-zA-Z ]+$"))
                {
                    nombre = value;
                }
                else
                {
                    throw new Exception("El nombre introducido no es correcto");
                }
            }
        }
        public string apellidos_pre
        {
            get
            {
                return apellidos;
            }
            set
            {
                if (Regex.IsMatch(value.ToString(), @"^[a-zA-Z ]+$"))
                {
                    apellidos = value;
                }
                else
                {
                    throw new Exception("El apellido introducido no es correcto");
                }
            }
        }
    }
}
