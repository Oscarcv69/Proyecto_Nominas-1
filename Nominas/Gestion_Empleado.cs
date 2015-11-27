using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nominas
{
    class Gestion_Empleado
    {
        #region GESTION TRABAJADOR - OSCAR
        public static bool ExisteTrabajador(Trabajador[] listaTrabajadores, string dni)
        {
            bool existe = false;
            int indice = 0;

            if (listaTrabajadores != null)
            {
                for (indice = 0; (indice < listaTrabajadores.Length) && !existe; indice++)
                {
                    if (listaTrabajadores[indice].dni_pre.Equals(dni)) existe = true;
                }
            }

            return existe;
        }
        private static bool ExisteTrabajador(Trabajador[] listaTrabajadores, string dni, ref int posicion)
        {
            bool existe = false;    // Control de existencia
            int indice = 0;

            if (listaTrabajadores != null)
            {
                for (indice = 0; (indice < listaTrabajadores.Length) && !existe; indice++)
                {
                    if (listaTrabajadores[indice].dni_pre.Equals(dni))
                    {
                        existe = true;
                        posicion = indice;
                    }
                }
            }

            return existe;
        }
        public static void NuevoTrabajador(ref Trabajador[] listaTrabajadores)
        {
            Trabajador trabtemp;
            Trabajador[] copia = null;
            bool salida = false;
            string mensaje = null;

            // Entrada de Datos
            do
            {
                // ENTRADA

                trabtemp = Interfaz.PlantillaCrearTrabajador();

                if (!ExisteTrabajador(listaTrabajadores, trabtemp.dni_pre))
                {

                    if (listaTrabajadores == null)
                    {
                        listaTrabajadores = new Trabajador[1];
                    }
                    else
                    {
                        copia = new Trabajador[listaTrabajadores.Length];
                        listaTrabajadores.CopyTo(copia, 0);
                        listaTrabajadores = new Trabajador[listaTrabajadores.Length + 1];
                        copia.CopyTo(listaTrabajadores, 0);
                        copia = null;
                    }

                    listaTrabajadores[listaTrabajadores.Length - 1] = trabtemp;

                    mensaje = "\n\t Trabajador registrado correctamente";
                }
                else
                {
                    mensaje = "\n\t El Trabajador se encuentra registrado";
                }

                // SALIDA
                mensaje = mensaje + "\n\n\t Desea registrar otro Trabajador (s/n): ";


                salida = Interfaz.Continuar(mensaje) ? false : true;

            } while (!salida);

        }
        // Método para mostrar a todos los trabajadores que hay en la nómina
        public static void ListarTrabajadores(Trabajador[] listaTrabajadores)
        {

            if (listaTrabajadores != null)
            {
                Interfaz.MostrarLista(listaTrabajadores);
                Interfaz.Continuar("\n\tPulse ENTER para continuar");
            }
            else
            {
                Interfaz.Continuar("\n\tNo hay trabajadores a mostrar. \n\tPulse ENTER para continuar");
            }
        }
        //Método para borrar un trabajador
        public static void BorrarTrabajador(ref Trabajador[] listaTrabajadores)
        {
            Trabajador[] copia;
            int posicion = 0;
            string dni = null;
            bool existe = false;
            bool correcto = false;
            string mensaje;
            int j = 0;

            //En primer lugar pedimos la contraseña para realizar cambios
            correcto = GestionContraseña();

            //Vamos a pedir el DNI para buscar a la persona que vamos a borrar
            dni = Interfaz.PlantillaBorrarUsuario();

            //En segundo lugar buscamos si existe dicho DNI introducido.
            existe = ExisteTrabajador(listaTrabajadores, dni, ref posicion);

            // Condición para conocer si la cuenta se encuentra sin saldo y además si existe.
            if (correcto == true && existe == true)
            {

                // Creamos un array de Copia para volcar los datos, con longitud de los clientes -1
                copia = new Trabajador[listaTrabajadores.Length - 1];

                /* For para recorrer el array, si encotnramos dicho DNI, borramos dicha posición y datos.*/


                for (int i = 0; i < listaTrabajadores.Length; i++, j++)
                {
                    if (i != posicion)
                    {
                        copia[j] = listaTrabajadores[i];
                    }
                    else j -= 1;
                }
                //Array dinámico
                listaTrabajadores = new Trabajador[copia.Length];
                copia.CopyTo(listaTrabajadores, 0);
                //Ponemos el array de copia en Null para ahorrar memoria
                copia = null;
                mensaje = "\n\t Cliente borrado con éxito, Pulse ENTER para continuar\n";
            }
            else
            {

                mensaje = "\n\t ERROR: Asegurese que el DNI existe o que no tienes fondos en su cuenta\n";
            }

            Interfaz.Continuar(mensaje);

        }
        #endregion
    }
}
