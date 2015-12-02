using System;

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
            string mensaje2 = null;
            bool existe = false;

            // Entrada de Datos
            do
            {
                // ENTRADA
                // NO COMPRUEBA SI EXISTE EL TRABAJADOR ------------------------->>>>>>
                trabtemp = Interfaz.PlantillaCrearTrabajador();
                existe = ExisteTrabajador(listaTrabajadores, trabtemp.dni_pre);

                if (listaTrabajadores == null)
                {
                    listaTrabajadores = new Trabajador[1];
                }
                if (listaTrabajadores != null)
                {
                    if (existe == false)
                    {
                        copia = new Trabajador[listaTrabajadores.Length];
                        listaTrabajadores.CopyTo(copia, 0);
                        listaTrabajadores = new Trabajador[listaTrabajadores.Length + 1];
                        copia.CopyTo(listaTrabajadores, 0);
                        copia = null;

                        listaTrabajadores[listaTrabajadores.Length - 1] = trabtemp;

                        mensaje = "Trabajador registrado correctamente";
                        mensaje2 = "Desea registrar otro Trabajador (s/n): ";
                        Interfaz.Pregunta(ref mensaje2, ref salida);
                        salida = Interfaz.Continuar(mensaje) ? false : true;
                    }

                    else
                    {
                        salida = false;
                    }

                }

            } while (!salida);

        }

        public static void ModificarTrabajador(ref Trabajador[] listaTrabajadores)
        {
            int posicion = 0;
            string eleccion = null;
            string dni = null;
            bool existe = false;
            bool salir = false;
            bool error = false;
            string pregunta = null;
            string mensaje = null;

            do
            {
                try
                {
                    if(error == false) { 
                    dni = Interfaz.PlantillaPedirDni();
                    existe = ExisteTrabajador(listaTrabajadores, dni, ref posicion);
                    }
                    if (existe)
                    {
                        eleccion = Interfaz.PlantillaEleccionModificar();

                        switch (eleccion)
                        {
                            case "1":
                                listaTrabajadores[posicion].dni_pre = Interfaz.ElementoModificar(eleccion);
                                break;
                            case "2":
                                listaTrabajadores[posicion].nombre_pre = Interfaz.ElementoModificar(eleccion);
                                break;
                            case "3":
                                listaTrabajadores[posicion].apellidos_pre = Interfaz.ElementoModificar(eleccion);
                                break;
                        }
                        error = false;
                        mensaje = "Operación realizada con éxito.";
                        pregunta = "¿Quieres modificar otro usuario? s/n >> ";
                        Interfaz.Continuar(mensaje);
                        Interfaz.Pregunta(ref pregunta, ref salir);
                    }

                    else
                    {
                        mensaje = "El Trabajador no existe";
                        salir = false;
                        error = true;
                    }
                }
                catch (Exception ex)
                {
                    Interfaz.Error(ex.Message);
                    Interfaz.Continuar("Pulse una tecla para continuar...");
                    salir = false;
                    error = true;
                }
            } while (!salir);
        }

        //Método para borrar un trabajador
        public static void BorrarTrabajador(ref Trabajador[] listaTrabajadores)
        {
            Trabajador[] copia;
            int posicion = 0;
            string dni = null;
            bool existe = false;
            //bool correcto = false;
            string mensaje;
            int j = 0;

            //En primer lugar pedimos la contraseña para realizar cambios
            /*correcto = GestionContraseña();*/

            //Vamos a pedir el DNI para buscar a la persona que vamos a borrar
            dni = Interfaz.PlantillaPedirDni();

            //En segundo lugar buscamos si existe dicho DNI introducido.
            existe = ExisteTrabajador(listaTrabajadores, dni, ref posicion);

            // Condición para conocer si la cuenta se encuentra sin saldo y además si existe.
            if (existe == true)
            {

                // Creamos un array de Copia para volcar los datos, con longitud de los trabajadores -1
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
                mensaje = "Trabajador borrado con éxito, Pulse ENTER para continuar";
                Interfaz.Continuar(mensaje);
            }
            else
            {
                mensaje = "El trabajador que está intentando borrar no existe.\n";
                Interfaz.Error(mensaje);
                Interfaz.Continuar();
            }
        }

        public static void ListarTrabajadores(Trabajador[] arr)
        {
            arr = Ficheros.getTrabajadores();
            Interfaz.Header();
            for (int i = 0; i < arr.Length; i++)
            {
                Interfaz.FormatoLeerXML(arr[i].dni_pre, arr[i].nombre_pre, arr[i].apellidos_pre);
            }
            Interfaz.Continuar("Pulsa una tecla para continuar");
        }
        // Método para mostrar a todos los trabajadores que hay en la nómina
        public static void ComprobarListaTrabajadores()
        {

        }
        public static bool ComprobarDni(string dni)
        {
            Trabajador[] temp = null;
            bool correcto = false;
            int i = 0;
            temp = Ficheros.getTrabajadores();
            for (i = 0; i < temp.Length; i++)
            {
                if (dni.Equals(temp[i].dni_pre))
                {
                    correcto = true;
                    return correcto;
                }
                else
                {
                    correcto = false;
                }
            }
            return correcto;
        }
        #endregion
    }
}
