using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    public class Encriptacion
    {
        public static string Encriptar(string cadenaAencriptar) // Encriptación datos
        {
            string result = null;
            try
            {
                byte[] encryted = Encoding.Unicode.GetBytes(cadenaAencriptar);
                result = Convert.ToBase64String(encryted);
            }
            catch (InvalidCastException e)
            {
                e.ToString();
            }
            return result;
        }

        /// DESENCRIPTACIÓN DE DATOS
        public static string DesEncriptar(string cadenaAdesencriptar)
        {

            string result = string.Empty;
            try
            {
                byte[] decryted = Convert.FromBase64String(cadenaAdesencriptar);
                result = Encoding.Unicode.GetString(decryted);
            }
            catch (InvalidCastException e)
            {
                e.ToString();
            }
            return result;
        }
    }
}
