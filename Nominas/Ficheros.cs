using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Nominas
{
    class Ficheros
    {
        private static string rutaEMP = @"..\\..\\..\\Nominas\\B.D_Empleados\\trabajador.xml"; // RUTA POR DEFECTO DE LA BASE DE DATOS DE EMPLEADO
        private static string rutaNOM = @"..\\..\\..\\Nominas\\B.D_Nominas\\"; // RUTA POR DEFECTO DE LAS NOMINAS DE LOS TRABAJADORES
        private static string rutaConf = @"..\\..\\..\\Nominas\\Recursos\\Conf.xml"; // RUTA DEL ARCHIVO DE CONFIGURACIÓN
        private static string dni_glo = null;
        #region FICHEROS XML EMPLEADOS - Francisco Romero
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
                        doc.Load(rutaEMP);
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
                        doc.Save(rutaEMP);
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

        public static Trabajador GetDatosTrabajador(string dni_trb)
        {
            XmlDocument doc = new XmlDocument();
            Trabajador trb = new Trabajador();
            string dni = null, nombre = null, apellidos = null;
            try
            {
                doc.Load(rutaEMP);
                XmlNodeList listaEmpleados = doc.SelectNodes("Plantilla/Trabajador");
                XmlNode empleado = null;
                trb = new Trabajador();
                for (int i = 0; i < listaEmpleados.Count; i++)
                {
                    empleado = listaEmpleados.Item(i);
                    dni = Encriptacion.DesEncriptar(empleado.Attributes.GetNamedItem("DNI").InnerText);
                    trb.dni_pre = dni.ToString();
                    if (trb.dni_pre.Equals(dni_trb))
                    {
                        nombre = Encriptacion.DesEncriptar(empleado.SelectSingleNode("Nombre").InnerText);
                        trb.nombre_pre = nombre;
                        apellidos = Encriptacion.DesEncriptar(empleado.SelectSingleNode("Apellidos").InnerText);
                        trb.apellidos_pre = apellidos;
                    }
                }
                return trb;
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
        }

        // LEER TRABAJADORES
        public static Trabajador[] getTrabajadores()
        {
            Trabajador trb = null;
            Trabajador[] trbArray = null;
            string dni, nombre, apellidos;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(rutaEMP);
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
            doc.Load(rutaEMP);
            XmlNode root = doc.DocumentElement;
            root.RemoveAll();
            doc.Save(rutaEMP);
        }

        public static void ExistOrEmptyEMP() // COMPRUEBA SI EXISTE LA BASE DE DATOS DE TRABAJADOR O ESTÁ VACIA. SI ES ASÍ, CREA UNO POR DEFECTO.
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(rutaEMP) || new FileInfo(rutaEMP).Length == 0)
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "Plantilla", string.Empty);
                doc.AppendChild(element1);
                doc.Save(rutaEMP);
            }
        }
        #endregion FIN XML EMPLEADOS


        #region FICHEROS XML NOMINAS - Francisco Romero

        public static void GuardarNominaTemporal(ref Nomina[] nomina)
        {
            XmlDocument doc = new XmlDocument();
            bool salir = false;
            FormatNomina();
            do
            {
                for (int i = 0; i < nomina.Length; i++)
                {
                    if (nomina[i] != null)
                    {
                        doc.Load(rutaNOM + "\\" + dni_glo + ".xml");
                        XmlNode root = doc.DocumentElement;

                        XmlElement SEMANA = doc.CreateElement("Semana");
                        root.AppendChild(SEMANA);

                        XmlAttribute ID = doc.CreateAttribute("ID");
                        ID.Value = nomina[i].ID_pre.ToString();
                        SEMANA.Attributes.Append(ID);

                        XmlElement horas = doc.CreateElement("Horas_Totales");
                        horas.AppendChild(doc.CreateTextNode(nomina[i].Horas_pre.ToString()));
                        SEMANA.AppendChild(horas);

                        XmlElement Jorpre = doc.CreateElement("Jornada_Pre");
                        Jorpre.AppendChild(doc.CreateTextNode(nomina[i].JornadaPre.ToString()));
                        SEMANA.AppendChild(Jorpre);

                        XmlElement ValHExtrasPre = doc.CreateElement("ValHExtras_Pre");
                        ValHExtrasPre.AppendChild(doc.CreateTextNode(nomina[i].HextrasPre.ToString()));
                        SEMANA.AppendChild(ValHExtrasPre);

                        XmlElement ValPHora = doc.CreateElement("ValPrecio_Hora_Pre");
                        ValPHora.AppendChild(doc.CreateTextNode(nomina[i].PrecioPre.ToString()));
                        SEMANA.AppendChild(ValPHora);

                        XmlElement ValRet = doc.CreateElement("ValRetencion_Pre");
                        ValRet.AppendChild(doc.CreateTextNode(nomina[i].RetencionPre.ToString()));
                        SEMANA.AppendChild(ValRet);

                        doc.Save(rutaNOM + "\\" + dni_glo + ".xml");
                        salir = true;
                    }
                    else
                    {
                        salir = true;
                    }
                }
            } while (!salir);
        }

        public static void FormatNomina()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaNOM + "\\" + dni_glo + ".xml");
            XmlNode root = doc.DocumentElement;
            root.RemoveAll();
            doc.Save(rutaNOM + "\\" + dni_glo + ".xml");
        }

        public static string BuscarNombre(string dni)
        {
            FileInfo[] archivos = null;
            string name = null;
            string result = null;


            DirectoryInfo d = new DirectoryInfo(rutaNOM);
            archivos = d.GetFiles("*.xml");

            if (archivos != null)
            {
                foreach (FileInfo file in archivos)
                {
                    name = Path.GetFileNameWithoutExtension(file.Name);
                    if (dni.Equals(name))
                    {
                        result = rutaNOM + "\\" + name + ".xml";
                        break;
                    }
                }
            }
            return result;
        }

        public static Nomina[] GetNomina(string dni)
        {
            string ruta = null;
            int id = 0, horas = 0, hextras = 0;
            float salarioExtra = 0.0F, salarioBruto = 0.0F, salarioNeto = 0.0F, impuestos = 0.0F;
            int jornadapre = 0;
            float hextraspre = 0.0F, retencionespre = 0.0F, preciopre = 0.0F;
            dni_glo = dni;
            ruta = BuscarNombre(dni);
            Nomina Nom = null;
            Nomina[] ArraySemanas = null;
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(ruta);
                XmlNodeList ListaSemanas = doc.SelectNodes("Nomina/Semana");
                XmlNode UnaSemana = null;

                ArraySemanas = new Nomina[ListaSemanas.Count];
                for (int i = 0; i < ListaSemanas.Count; i++)
                {
                    Nom = new Nomina();
                    UnaSemana = ListaSemanas.Item(i);

                    id = Int32.Parse(UnaSemana.Attributes.GetNamedItem("ID").InnerText);
                    Nom.ID_pre = id;
                    horas = Int32.Parse(UnaSemana.SelectSingleNode("Horas_Totales").InnerText);
                    Nom.Horas_pre = horas;
                    // Valores predeterminados CONF
                    jornadapre = Int32.Parse(UnaSemana.SelectSingleNode("Jornada_Pre").InnerText);
                    Nom.JornadaPre = jornadapre;
                    hextraspre = Single.Parse(UnaSemana.SelectSingleNode("ValHExtras_Pre").InnerText);
                    Nom.HextrasPre = hextraspre;
                    preciopre = Single.Parse(UnaSemana.SelectSingleNode("ValPrecio_Hora_Pre").InnerText);
                    Nom.PrecioPre = preciopre;
                    retencionespre = Single.Parse(UnaSemana.SelectSingleNode("ValRetencion_Pre").InnerText);
                    Nom.RetencionPre = retencionespre;
                    //

                    ArraySemanas[i] = Nom;
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
            catch (NullReferenceException)
            {
                throw new Exception("Falta un elemento en el archivo, revíse el contenido.");
            }
            return ArraySemanas;

        }

        public static void ExistOrEmptyNOM(string dni) // COMPRUEBA SI 
        {
            FileInfo[] archivos = null;
            bool flag = false;

            DirectoryInfo d = new DirectoryInfo(rutaNOM);
            archivos = d.GetFiles("*.xml");

            if (archivos != null)
            {
                foreach (FileInfo file in archivos)
                {
                    if (dni.Equals(Path.GetFileNameWithoutExtension(file.Name)))
                    {
                        flag = true;
                    }
                }
            }

            if (flag == false)
            {
                XmlDocument doc = new XmlDocument();
                if (!File.Exists(rutaNOM) || new FileInfo(rutaEMP).Length == 0)
                {
                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);

                    XmlElement element1 = doc.CreateElement(string.Empty, "Nomina", string.Empty);
                    doc.AppendChild(element1);
                    doc.Save(rutaNOM + dni + ".xml");
                }
            }
        }

        #endregion

        #region ARCHIVO DE CONFIGURACIÓN
        public static void setConfig()
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(rutaConf))
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "Configuracion", string.Empty);
                doc.AppendChild(element1);

                XmlElement jornada = doc.CreateElement("Jornada");
                jornada.AppendChild(doc.CreateTextNode("40"));
                element1.AppendChild(jornada);

                XmlElement Hextras = doc.CreateElement("Horas_Extras");
                Hextras.AppendChild(doc.CreateTextNode("1.5"));

                element1.AppendChild(Hextras);

                XmlElement retencion = doc.CreateElement("Retencion");
                retencion.AppendChild(doc.CreateTextNode("0.16"));
                element1.AppendChild(retencion);
                doc.Save(rutaConf);
            }
        }

        public static void getConfig(ref int jornada, ref float Hextras, ref float retencion)
        {
            string hextras_provisional = null;
            string retencion_provisional = null;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(rutaConf);
                XmlNodeList raiz = doc.SelectNodes("Configuracion");
                foreach (XmlNode conf in raiz)
                {
                    jornada = Int32.Parse(conf.SelectSingleNode("Jornada").InnerText);
                    hextras_provisional = conf.SelectSingleNode("Horas_Extras").InnerText;
                    retencion_provisional = conf.SelectSingleNode("Retencion").InnerText;
                }
                Hextras = Convert.ToSingle(hextras_provisional.ToString(), CultureInfo.InvariantCulture);
                retencion = Convert.ToSingle(retencion_provisional.ToString(), CultureInfo.InvariantCulture);
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

        public static void ModConfig(string name, string valor)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaConf);
            XmlNode nodo;
            nodo = doc.DocumentElement;

            foreach (XmlNode node1 in nodo.ChildNodes)
                switch (name)
                {
                    case "Jornada":
                        int newJor = Int32.Parse(valor);
                        node1.InnerText = newJor.ToString();
                        break;
                    case "Horas_Extras":
                        float newHoras = float.Parse(valor, CultureInfo.InvariantCulture);
                        node1.InnerText = newHoras.ToString();
                        break;
                    case "Retenciones":
                        float newRet = float.Parse(valor, CultureInfo.InvariantCulture);
                        node1.InnerText = newRet.ToString();
                        break;
                }
            doc.Save(rutaConf);
        }
        #endregion FIN ARCHIVO CONFIGURACIÓN

        #region FICHEROS TXT - Francisco Romero
        // CREAR TXT
        public static void CerrarNomina(string cadena)
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
        // FIN CREAR TXT
        #endregion FIN TXT
    }
}
