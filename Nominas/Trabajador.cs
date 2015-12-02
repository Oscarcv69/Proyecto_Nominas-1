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
                char[] comprobacion = { 'T', 'R', 'W', 'A', 'G', 'M', 'Y', 'F', 'P', 'D', 'X', 'B', 'N', 'J', 'Z', 'S', 'Q', 'V', 'H', 'L', 'C', 'K', 'E' };
                int resto = 0;
                char letra;
                int numero = 0;
                //VALIDACION DE DNI POR LA LONGITUD DEL MISMO
                if (value.Length < 9)
                {
                    throw new Exception("La longitud debe ser 9");
                }
                //VALIDACION DE DNI PARA QUE NUMEROS SEAN NUMEROS Y LA LETRA SEA UNA LETRA
                else if (!Int32.TryParse(value.Substring(0, 8), out numero) || (Int32.TryParse(value[8].ToString(), out numero)))
                {
                    throw new Exception("DNI Incorrecto: Formato erróneo (12345678A)");
                }
                //VALIDACION DE DNI PARA QUE SI EL FORMATO DE LOS NUMEROS Y LA LETRA SON CORRECTOS, COMPROBAR SI LA LETRA PERTENECE A ESE NUMERO
                else if (Int32.TryParse(value.Substring(0, 8), out numero) || (Int32.TryParse(value[8].ToString(), out numero)))
                {
                    letra = value[8];
                    resto = (numero % 23);
                    //SI NO PERTENECE DEVUELVE UNA EXCEPCION
                    if (letra != comprobacion[resto])
                    {
                        throw new Exception("DNI Incorrecto: Letra del DNI es incorrecta");
                    }
                    //SI PERTENECE DEVUELVE DNI
                    else
                    {
                        dni = value.ToUpper();
                    }
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
                //COMPROBACIÓN DE QUE NOMBRE ESTE COMPUESTO SOLO POR LETRAS
                if (Regex.IsMatch(value.ToString(), @"^[a-zA-ZñÑ\s]+$"))
                {
                    nombre = value;
                }
                //EXEPCIÓN DE NOMBRE INTRODUCIDO INCORRECTO
                else
                {
                    throw new Exception("El apellido introducido no tiene el formato correcto");
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
                //COMPROBACIÓN DE QUE NOMBRE ESTE COMPUESTO SOLO POR LETRAS
                if (Regex.IsMatch(value.ToString(), @"^[a-zA-ZñÑ\s]+$"))
                {
                    apellidos = value;
                }
                //EXCEPCIÓN EN EL APELLIDO
                else
                {
                    throw new Exception("El apellido introducido no tiene el formato correcto");
                }
            }
        }
    }
}
