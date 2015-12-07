using System;
using System.Text.RegularExpressions;
/// <summary> INFORME : 
/// Es una clase para validar la entrada dni, nombre y apellidos.
/// Declaramos las varibles que vamos a utilizar como en la clase trbajadores como son dni, nombre y apellidos.
/// Con un metodo trabajadores hacemos que estas variables se conviertan en null.
/// Con los metodos dni_pre, nombre_pre y apellidos_pre controlamos las excepciones de estos campos
///     En el metodo dni_pre tenemos un get que devuelve el dni introducido y un set en el cual se validan las entradas de dni por teclado
///     en el set comprobamos que no tenemos un dni que su largo sea inferior o mayor a 9 caracteres ya sean letras o numeros, en el cual acontinuacion 
///     tenemos un if el cual lee cada caracter de la cadena dni y comprueba que los 8 primero caracteres son numeros y el ultimo caracter de la cadena es 
///     una letra, tras pasado por este if, llegamos al ultimo if el cual comprueba que la letra del dni es correcta, si todo es correcto devuelve dni para
///     guardarlo.
///     En los metodos nombre_pre y apellidos_pre tenemos un get que nos devuelve el nombre en nombre_pre y los apellidos en apellidos_pre.
///     El set de estos metodos son iguales porque tenemos que conprobar lo mismo en los dos asi que tenemos un if con un regex con todos los 
///     caracteres que podemos introducir, si es todo correcto devuelve nombre y apellido, sino lo son saltara una execión.
/// </summary>

namespace Nominas
{

    public class Trabajador
    {
        private string dni, nombre, apellidos;
        public Trabajador()
        {
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
                if (Regex.IsMatch(value.ToString(), @"^[a-zA-ZñáéíóúÑÁÉÍÓÚ\s]+$") && !(value == " "))
                {
                    value = Regex.Replace(value, @"\s+", " ");
                    nombre = value.Trim();
                }
                //EXEPCIÓN DE NOMBRE INTRODUCIDO INCORRECTO
                else
                {
                    throw new Exception("El nombre introducido no tiene el formato correcto");
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
                if (Regex.IsMatch(value.ToString(), @"^[a-zA-ZñáéíóúÑÁÉÍÓÚ\s]+$"))
                {
                    value = Regex.Replace(value, @"\s+", " ");
                    apellidos = value.Trim();
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
