﻿using System;
/// <summary> INFORME : 
/// Clase que contiene las métodos correspondientes para la creación, modificación y borrado de trabajadores.
/// También están los metodos para comprobar la existencia de los trabajadores consultados en la Base de datos
/// y un método para mostrar por pantalla dichos trabajadores contenidos en la base de Datos
/// </summary>

namespace Nominas
{
    class Gestion_Empleado
    {
        #region GESTION TRABAJADOR - OSCAR

        //Método para comprobar si existe el trabajador en la base de datos
        public static bool ExisteTrabajador(Trabajador[] listaTrabajadores, string dni)
        {
            bool existe = false; //Inicialización de variables
            int indice = 0;

            if (listaTrabajadores != null)
            {
                for (indice = 0; (indice < listaTrabajadores.Length) && !existe; indice++)
                {
                    if (listaTrabajadores[indice].dni_pre.Equals(dni)) existe = true; //Si lo encontramos, asignamos true
                }
            }

            return existe; //Devolvemos el resultado de la búsqueda, falso/verdadero
        }

        //Método para comprobar si existe el Trabajador, pero además nos trae su posición
        private static bool ExisteTrabajador(Trabajador[] listaTrabajadores, string dni, ref int posicion)
        {
            bool existe = false;    // Inicialización de variables
            int indice = 0;

            if (listaTrabajadores != null)
            {
                for (indice = 0; (indice < listaTrabajadores.Length) && !existe; indice++)
                {
                    if (listaTrabajadores[indice].dni_pre.Equals(dni))
                    {
                        existe = true;  // Si existe, asignamos true a la variable
                        posicion = indice; // Asignamos a la posición el índice donde lo hemos encontramos
                    }
                }
            }

            return existe; //Devolvemos si existe o no
        }

        //Método utilizado para la creación de nuevos trabajadores
        public static void NuevoTrabajador(ref Trabajador[] listaTrabajadores, int mode)
        {
            Trabajador trabtemp;
            Trabajador[] copia = null;
            bool salida = false; //Inicialización de variables
            string mensaje = null;
            string mensaje2 = null;
            bool existe = false;

            // Entrada de Datos
            do
            {
                // ENTRADA
                trabtemp = Interfaz.PlantillaCrearTrabajador();
                existe = ExisteTrabajador(listaTrabajadores, trabtemp.dni_pre); //Comprueba si existe o no el trabajador

                if (listaTrabajadores == null) //Si la lista de trabajadores está vacía, le asigna una posición
                {
                    listaTrabajadores = new Trabajador[1];
                }
                if (listaTrabajadores != null) //Como no es nula, debido a la anterior creación de la posición
                {
                    if (existe == false)
                    {
                        //Gestión dinámica de la memoria
                        copia = new Trabajador[listaTrabajadores.Length]; //Copiamos el tamaño del array de trabajadores
                        listaTrabajadores.CopyTo(copia, 0); //Volcamos el contenido del array de trabajadores en copia
                        listaTrabajadores = new Trabajador[listaTrabajadores.Length + 1]; //Asignamos una posición más
                        copia.CopyTo(listaTrabajadores, 0); //Copiamos todo de nuevo al array de trabajadores
                        copia = null;//Volvemos copia a null para ahorrar memoria

                        listaTrabajadores[listaTrabajadores.Length - 1] = trabtemp; //Asignamos el nuevo trabajador al array

                        mensaje = "Trabajador registrado correctamente";
                        if (mode == 1)// Modo 1: Creamos empleados por NuevoTrabajadors
                        {
                            mensaje2 = "Desea registrar otro Trabajador (s/n): ";
                            salida = Interfaz.Continuar(mensaje);
                            Interfaz.Pregunta(ref mensaje2, ref salida);
                            Ficheros.GuardarTrabajadores(listaTrabajadores);
                        }
                        else //Creamos empleados desde la nómina.
                        {
                            salida = true;
                        }
                    }

                    else //Si existe el trabajador, asignamos que salga del bucle
                    {
                        salida = true;
                    }

                }

            } while (!salida);
        }
        //Método para la Modificación de los diferentes aspectos de un trabajador en concreto
        public static void ModificarTrabajador(ref Trabajador[] listaTrabajadores)
        {
            int posicion = 0;
            string eleccion = null;
            string dni = null;
            bool salir = false, existe = false;
            bool error = false;
            string pregunta = null;
            string mensaje = null;

            if (error == false)
            {
                dni = Interfaz.PlantillaPedirDni(); //Pedimos el DNI a modificar con la plantilla de la intefaz
                existe = ExisteTrabajador(listaTrabajadores, dni, ref posicion);
            }
            if (existe)
            {
                do
                {
                    try
                    {
                        //Llamamos a la plantilla para ver la elección escogida
                        eleccion = Interfaz.PlantillaEleccionModificar();

                        switch (eleccion) //Según la elección escogida realiza una acción u otra
                        {
                            //Caso 1: Modificación del DNI
                            case "1":
                                listaTrabajadores[posicion].dni_pre = Interfaz.ElementoModificar(eleccion);
                                break;
                            //Caso 2: Modificación del NOMBRE
                            case "2":
                                listaTrabajadores[posicion].nombre_pre = Interfaz.ElementoModificar(eleccion);
                                break;
                            //Caso 1: Modificación de los APELLIDOS
                            case "3":
                                listaTrabajadores[posicion].apellidos_pre = Interfaz.ElementoModificar(eleccion);
                                break;
                        }
                        error = false;
                        mensaje = "Operación realizada con éxito.";
                        pregunta = "¿Quieres modificar otro aspecto? s/n >> ";
                        Interfaz.Continuar(mensaje);
                        Interfaz.Pregunta(ref pregunta, ref salir);
                        Ficheros.GuardarTrabajadores(listaTrabajadores);
                        // Si todo ha sido validado, lo guardamos en el fichero

                    }
                    catch (Exception ex)
                    {
                        Interfaz.Error(ex.Message);
                        Interfaz.Continuar();
                        salir = false;
                        error = true;
                    }
                } while (!salir);
            }

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
                mensaje = "Trabajador borrado con éxito";
                Interfaz.Continuar(mensaje);
                Interfaz.Continuar();
            }
            else
            {
                mensaje = "El trabajador que está intentando borrar no existe.\n";
                Interfaz.Error(mensaje);
                Interfaz.Continuar();
            }
        }
        // Método para mostrar a todos los trabajadores que hay en la nómina

        public static void ListarTrabajadores(Trabajador[] arr)
        {
            arr = Ficheros.getTrabajadores(); //Cogemos los trabajadores del fichero
            Interfaz.Header();
            for (int i = 0; i < arr.Length; i++)
            {
                Interfaz.FormatoLeerXML(arr[i].dni_pre, arr[i].nombre_pre, arr[i].apellidos_pre);
            }
            Interfaz.Continuar();
        }

        //Método para comprobar si el DNI existe ya en la Base de datos de empleados.
        public static bool ComprobarDni(string dni)
        {
            Trabajador[] temp = null;
            bool correcto = false;
            int i = 0;                          //Inicialización de variables
            temp = Ficheros.getTrabajadores(); //Cogemos los trabajadores del fichero
            for (i = 0; i < temp.Length; i++) //Recorremos los DNI de los trabajadores buscando el solicitado
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
            return correcto; //Devolvemos si ha sido encontrado o no
        }
        #endregion
    }
}
