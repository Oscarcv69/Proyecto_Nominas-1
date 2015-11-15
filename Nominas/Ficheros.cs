using System;
using System.IO;
using System.Xml;

namespace Nominas
{
    class Ficheros
    {

        #region FICHEROS XML - Francisco Romero
        // CREAR TRABAJADORES
        public static void crearTrabajadores(string _DNI, string _nombre, string _apellidos)
        {
            string ruta = "..\\..\\..\\Nominas\\Nominas_empleados\\trabajador.xml";
            XmlDocument doc = new XmlDocument();
            bool salir = false;

            do
            {
                if (File.Exists(ruta)) // COMPROBAR SI EXISTE -> COMPLETADO
                {
                    doc.Load(ruta);
                    XmlNode root = doc.DocumentElement;
                    XmlElement nodo = doc.CreateElement("Trabajador");
                    root.AppendChild(nodo);

                    XmlAttribute dni = doc.CreateAttribute("DNI");
                    dni.Value = Encriptacion.Encriptar(_DNI);
                    nodo.Attributes.Append(dni);

                    XmlElement nombre = doc.CreateElement("Nombre");
                    nombre.AppendChild(doc.CreateTextNode(Encriptacion.Encriptar(_nombre)));
                    nodo.AppendChild(nombre);

                    XmlElement apellidos = doc.CreateElement("Apellidos");
                    apellidos.AppendChild(doc.CreateTextNode(Encriptacion.Encriptar(_apellidos)));
                    nodo.AppendChild(apellidos);
                    doc.Save(ruta);
                    salir = true;
                }
                else
                {
                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);

                    XmlElement element1 = doc.CreateElement(string.Empty, "Plantilla", string.Empty);
                    doc.AppendChild(element1);
                    doc.Save(ruta);
                    salir = false;
                }
            } while (!salir);
        }
        // FIN CREAR TRABAJADORES

        // LEER TRABAJADORES
        public static void getTrabajadores()
        {
            string dni, nombre, apellidos;
            XmlDocument doc = new XmlDocument();
            doc.Load("..\\..\\..\\Nominas\\Nominas_empleados\\trabajador.xml");

            //Obtenemos una colección con todos los empleados.
            XmlNodeList listaEmpleados = doc.SelectNodes("Plantilla/Trabajador");

            //Creamos un único empleado.
            XmlNode unEmpleado;
            Interfaz.Header();
            Interfaz.HeaderVerTrabajadores();

            //Recorremos toda la lista de empleados.
            for (int i = 0; i < listaEmpleados.Count; i++)
            {
                unEmpleado = listaEmpleados.Item(i);
                dni = Encriptacion.DesEncriptar(unEmpleado.Attributes.GetNamedItem("DNI").InnerText);
                nombre = Encriptacion.DesEncriptar(unEmpleado.SelectSingleNode("Nombre").InnerText);
                apellidos = Encriptacion.DesEncriptar(unEmpleado.SelectSingleNode("Apellidos").InnerText);
                Interfaz.FormatoLeerXML(dni, nombre, apellidos);
            }
        }
        #endregion FIN XML - Francisco Romero

        #region FICHEROS TXT - Francisco Romero
        // CREAR TXT
        public static void CrearTxtNomina()
        {
            bool salir = false;
            string fic = @"..\\..\\..\\Nominas\\Nominas_empleados\\nomina_empleado.txt";
            string formato = "Prueba escribir";
            try
            {
                do
                {
                    if (!File.Exists(fic)) // ARCHIVO EXISTE -> COMPROBADO
                    {
                        StreamWriter writer = File.CreateText(fic);
                        salir = false;
                        writer.Close();
                    }
                    else
                    {
                        StreamWriter sw = new StreamWriter(fic, true);
                        sw.WriteLine(formato);
                        sw.Close();
                        salir = true;
                    }
                } while (!salir);
            }
            catch (FileNotFoundException e)
            {
                e.ToString();
            }
            catch (FileLoadException ex)
            {
                ex.ToString();
            }
        }
        // FIN CREAR TXT
        #endregion FIN TXT
    }
}
