using System;
using System.IO;
using System.Xml;

namespace Nominas
{
    class Ficheros
    {
        private static string ruta = @"..\\..\\..\\Nominas\\Nominas_empleados\\trabajador.xml";
        private static string rutaConf = @"..\\..\\..\\Nominas\\Conf.xml";
        #region FICHEROS XML - Francisco Romero
        // CREAR TRABAJADORES
        public static void GuardarTrabajadores(Trabajador[] trb)
        {
            XmlDocument doc = new XmlDocument();
            bool salir = false;
            Format(); // FORMATEA EL ARCHIVO
            do
            {
                if (trb.Length != 0)
                {
                    for (int i = 0; i < trb.Length; i++)
                    {
                        doc.Load(ruta);
                        XmlNode root = doc.DocumentElement;
                        XmlElement nodo = doc.CreateElement("Trabajador");
                        root.AppendChild(nodo);

                        XmlAttribute dni = doc.CreateAttribute("DNI");
                        dni.Value = Encriptacion.Encriptar(trb[i].dni_pre);
                        nodo.Attributes.Append(dni);

                        XmlElement nombre = doc.CreateElement("Nombre");
                        nombre.AppendChild(doc.CreateTextNode(Encriptacion.Encriptar(trb[i].nombre_pre)));
                        nodo.AppendChild(nombre);

                        XmlElement apellidos = doc.CreateElement("Apellidos");
                        apellidos.AppendChild(doc.CreateTextNode(Encriptacion.Encriptar(trb[i].apellidos_pre)));
                        nodo.AppendChild(apellidos);
                        doc.Save(ruta);
                        salir = true;
                    }
                }
                else
                {
                    salir = true;
                }
            } while (!salir);
        }
        // FIN CREAR TRABAJADORES

        // LEER TRABAJADORES
        public static Trabajador[] getTrabajadores()
        {
            Trabajador trb = null;
            Trabajador[] trbArray = null;
            string dni, nombre, apellidos;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(ruta);

                XmlNodeList listaEmpleados = doc.SelectNodes("Plantilla/Trabajador");
                XmlNode unEmpleado;

                trbArray = new Trabajador[listaEmpleados.Count];
                for (int i = 0; i < listaEmpleados.Count; i++)
                {
                    trb = new Trabajador();
                    unEmpleado = listaEmpleados.Item(i);
                    dni = Encriptacion.DesEncriptar(unEmpleado.Attributes.GetNamedItem("DNI").InnerText);
                    trb.dni_pre = dni.ToString();
                    nombre = Encriptacion.DesEncriptar(unEmpleado.SelectSingleNode("Nombre").InnerText);
                    trb.nombre_pre = nombre;
                    apellidos = Encriptacion.DesEncriptar(unEmpleado.SelectSingleNode("Apellidos").InnerText);
                    trb.apellidos_pre = apellidos;
                    trbArray[i] = trb;
                }
            }
            catch (FileNotFoundException)
            {
                throw new Exception("Archivo no encontrado");
            }
            catch (ArgumentException)
            {
                throw new Exception("El archivo al que trata de acceder esta vacio. Por favor inserte minimo un trabajador.");
            }
            catch (XmlException)
            {
                throw new Exception("No se ha podido abrir el archivo, revise el contenido.");
            }
            return trbArray;
        }

        public static void Format() // FORMATEA EL ARCHIVO XML ANTES DE INGRESAR LOS DATOS DE LOS TRABAJADORES
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode root = doc.DocumentElement;
            root.RemoveAll();
            doc.Save(ruta);
        }

        public static void ExistOrEmpty()
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(ruta) || new FileInfo(ruta).Length == 0)
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "Plantilla", string.Empty);
                doc.AppendChild(element1);
                doc.Save(ruta);
            }
        }

        public static void setConfig()
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(rutaConf))
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);
                XmlElement nodo = doc.CreateElement("Configuracion");
                root.AppendChild(nodo);

                XmlElement jornada = doc.CreateElement("Jornada");
                jornada.AppendChild(doc.CreateTextNode("40"));
                nodo.AppendChild(jornada);

                XmlElement Hextras = doc.CreateElement("Horas_Extras");
                Hextras.AppendChild(doc.CreateTextNode("1,5"));
                nodo.AppendChild(Hextras);

                XmlElement retencion = doc.CreateElement("Retencion");
                retencion.AppendChild(doc.CreateTextNode("0,16"));
                nodo.AppendChild(retencion);
                doc.Save(ruta);
            }
        }

        public static void getConfig(ref int jornada, ref float Hextras, ref float retencion)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(rutaConf);
                XmlNodeList raiz = doc.SelectNodes("Configuracion");
                XmlNode configuracion;
                configuracion = raiz.Item(0);
                jornada =  Int32.Parse(configuracion.SelectSingleNode("Jornada").InnerText);
                Hextras = Int64.Parse(configuracion.SelectSingleNode("Horas_Extras").InnerText);
                retencion = Int64.Parse(configuracion.SelectSingleNode("Retenecion").InnerText);
            }
            catch (FileNotFoundException)
            {
                throw new Exception("Archivo no encontrado.");
    }
            catch (ArgumentException)
            {
                throw new Exception("El archivo al que trata de acceder esta vacio. Por favor inserte minimo un trabajador.");
}
            catch (XmlException)
            {
                throw new Exception("No se ha podido abrir el archivo, revise el contenido.");
            }

        }
        #endregion FIN XML - Francisco Romero

        #region FICHEROS TXT - Francisco Romero
        // CREAR TXT
        public static void CrearTxtNomina(string cadena)
{
    bool salir = false;
    string fic = @"..\\..\\..\\Nominas\\Nominas_empleados\\nomina_empleado.txt";
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
                sw.WriteLine(cadena);
                sw.Close();
                salir = true;
            }
        } while (!salir);
    }
    catch (FileNotFoundException)
    {
        throw new Exception("Archivo no encontrado.");
    }
    catch (FileLoadException)
    {
        throw new Exception("Fallo al cargar el archivo.");
    }
}
public static void getNomina(string dni)
{
    try
    {
        StreamReader sr = new StreamReader(@"..\\..\\..\\Nominas\\Nominas_empleados\\nomina_empleado.txt", false);
        string linea;
        while ((linea = sr.ReadLine()) != null)
        {
            Console.WriteLine(linea);
        }
    }
    catch (Exception)
    {
        Console.WriteLine("El archivo no se puede leer");
    }
}
        // FIN CREAR TXT
        #endregion FIN TXT
    }
}
