using System;
using System.Globalization;
using System.IO;
using System.Xml;
                    /// <summary> INFORME : 
                    /// Esta clase crea, guarda, obtiene, borra los ficheros correspondientes para su tratamiento en Gestión de Nóminas y 
                    /// Gestión de Empleados.
                    /// Hay métodos que Guardan las BASES DE DATOS de empleados y nóminas, otros que crean estos ficheros en caso de que no existan
                    /// al inicio de la aplicación.
                    /// Al inicio de la aplicación se crea el archivo de configuración con los valroes predeterminados en caso de que este no exista.
                    /// Si al inicio de la aplicación, el archivo que contiene los trabajadores de la empresa está corrupto o no existe, crea uno
                    /// predeterminado.
                    /// </summary>
namespace Nominas
{
    class Ficheros
    {
        private static string rutaEMP = @"..\\..\\..\\Nominas\\B.D_Empleados\\trabajador.xml"; // RUTA POR DEFECTO DE LA BASE DE DATOS DE EMPLEADO
        private static string rutaNOM = @"..\\..\\..\\Nominas\\B.D_Nominas\\"; // RUTA POR DEFECTO DE LAS NOMINAS DE LOS TRABAJADORES
        private static string rutaConf = @"..\\..\\..\\Nominas\\Recursos\\Conf.xml"; // RUTA DEL ARCHIVO DE CONFIGURACIÓN
        private static string nominascerradas = @"..\\..\\..\\Nominas\\B.D_Cerradas\\"; // RUTA DEL DIRECTORIO DE NÓMINAS CERRADAS
        private static string dni_glo = null; // DNI DEL TRABAJADOR
        private static XmlDocument doc = null;

        #region FICHEROS XML EMPLEADOS - Francisco Romero
        // CREAR TRABAJADORES
        public static void GuardarTrabajadores(Trabajador[] trb) // GUARDAR LOS TRABAJADORES EN EL ARCHIVO XML
        {
            doc = new XmlDocument();
            bool salir = false;
            Format(); // FORMATEA EL ARCHIVO ANTES DE GUARDAR
            do
            {
                if (trb.Length != 0)
                {
                    for (int i = 0; i < trb.Length; i++)
                    {
                        doc.Load(rutaEMP);
                        XmlNode root = doc.DocumentElement;
                        XmlElement nodo = doc.CreateElement("Trabajador"); // INTRODUCE EL NODO TRABAJADOR JUNTO CON SUS ELEMENTOS HIJOS
                        root.AppendChild(nodo);

                        XmlAttribute dni = doc.CreateAttribute("DNI"); // ELEMENTO DNI ENCRIPTADO
                        dni.Value = Encriptacion.Encriptar(trb[i].dni_pre);
                        nodo.Attributes.Append(dni);

                        XmlElement nombre = doc.CreateElement("Nombre"); // ELEMENTO NOMBRE ENCRIPTADO
                        nombre.AppendChild(doc.CreateTextNode(Encriptacion.Encriptar(trb[i].nombre_pre)));
                        nodo.AppendChild(nombre);

                        XmlElement apellidos = doc.CreateElement("Apellidos"); // ELEMENTO APELLIDOS ENCRIPTADO
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

        public static Trabajador GetDatosTrabajador(string dni_trb) // DEVUELVE LOS DATOS DE UN TRABAJADOR EN PARTICULAR (PARA MOSTRAROS EN LA NÓMINA)
        {
            doc = new XmlDocument();
            Trabajador trb = new Trabajador();
            string dni = null, nombre = null, apellidos = null;
            try
            {
                doc.Load(rutaEMP);
                XmlNodeList listaEmpleados = doc.SelectNodes("Plantilla/Trabajador"); // OBTIENE LOS NODOS DE TRABAJADOR
                XmlNode empleado = null;
                trb = new Trabajador();
                for (int i = 0; i < listaEmpleados.Count; i++)
                {
                    empleado = listaEmpleados.Item(i);
                    dni = Encriptacion.DesEncriptar(empleado.Attributes.GetNamedItem("DNI").InnerText); 
                    if (dni.Equals(dni_trb))                                        // SI EL DNI DEL TRABAJADOR COINCIDE CON EL PASADO POR PARAMETRO, DEVUELVE LOS DATOS DE ESTE.
                    {
                        trb.dni_pre = dni_trb;
                        nombre = Encriptacion.DesEncriptar(empleado.SelectSingleNode("Nombre").InnerText); // OBTIENE EL NOMBRE
                        trb.nombre_pre = nombre;
                        apellidos = Encriptacion.DesEncriptar(empleado.SelectSingleNode("Apellidos").InnerText); // OBTIENE EL/LOS APELLIDOS
                        trb.apellidos_pre = apellidos;
                    }
                }
                return trb; // DEVUELVE UN OBJETO TRABAJADOR
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
        public static Trabajador[] getTrabajadores() // OBTIENE LA BASE DE DATOS DE TRABAJADORES, DEVUELVE UN ARRAY DE TRABAJADORES DE LA EMPRESA
        {
            doc = new XmlDocument();
            Trabajador trb = null;
            Trabajador[] trbArray = null;
            string dni, nombre, apellidos;
            try
            {
                doc.Load(rutaEMP);
                XmlNodeList listaEmpleados = doc.SelectNodes("Plantilla/Trabajador"); // SELECCIONA LOS NODOS TRABAJADOR DENTRO DEL NODO RAIZ
                XmlNode unEmpleado;

                trbArray = new Trabajador[listaEmpleados.Count];
                for (int i = 0; i < listaEmpleados.Count; i++)
                {
                    trb = new Trabajador();
                    unEmpleado = listaEmpleados.Item(i);
                    dni = Encriptacion.DesEncriptar(unEmpleado.Attributes.GetNamedItem("DNI").InnerText); // OBTIENE EL DNI
                    trb.dni_pre = dni.ToString();
                    nombre = Encriptacion.DesEncriptar(unEmpleado.SelectSingleNode("Nombre").InnerText); // OBTIENE EL NOMBRE
                    trb.nombre_pre = nombre;
                    apellidos = Encriptacion.DesEncriptar(unEmpleado.SelectSingleNode("Apellidos").InnerText); // OBTIENE EL/LOS APELLIDOS
                    trb.apellidos_pre = apellidos;
                    trbArray[i] = trb; // GUARDA EL TRABAJADOR EN EL ARRAY
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
            doc = new XmlDocument();
            doc.Load(rutaEMP); // CARGA LA RUTA DEL ARCHIVO
            XmlNode root = doc.DocumentElement; // OBTIENE EL NODO RAÍZ
            root.RemoveAll(); // BORRA TODOS LOS NODOS HIJOS
            doc.Save(rutaEMP); // GUARDA EL ARCHIVO SOLO CON EL NODO RAÍZ
        }

        public static void ExistOrEmptyEMP() // COMPRUEBA SI EXISTE LA BASE DE DATOS DE TRABAJADOR O SI NO TIENE NODOS HIJOS. SI ES ASÍ, CREA UNO POR DEFECTO.
        {
            doc = new XmlDocument();
            if (!File.Exists(rutaEMP) || doc.ChildNodes.Count == 0) // SI ESTÁ EL ARCHIVO O SI NO TIENE NODOS HIJOS
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null); // CREA LA CABECERA
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "Plantilla", string.Empty); // CREA EL NODO RAÍZ
                doc.AppendChild(element1);
                doc.Save(rutaEMP);
            }
        }
        #endregion FIN XML EMPLEADOS

        #region FICHEROS XML NOMINAS - Francisco Romero

        public static void GuardarNominaTemporal(ref Nomina[] nomina) // GUARDA LA NÓMINA TEMPORAL (LA QUE NO TIENE LOS CÁLCULOS FINALES) EN UN ARCHIVO XML
        {
            doc = new XmlDocument();
            bool salir = false;
            FormatNomina();
            do
            {
                for (int i = 0; i < nomina.Length; i++)
                {
                    if (nomina[i] != null)
                    {
                        doc.Load(rutaNOM + "\\" + dni_glo + ".xml"); // EL NOMBRE DEL FICHERO DEBE SER EL DNI DEL TRABAJADOR
                        XmlNode root = doc.DocumentElement;

                        XmlElement SEMANA = doc.CreateElement("Semana"); //´CREA EL NODO SEMANA
                        root.AppendChild(SEMANA);

                        XmlAttribute ID = doc.CreateAttribute("ID");  // CREA COMO ATRIBUTO EL NÚMERO DE SEMANA
                        ID.Value = nomina[i].ID_pre.ToString();
                        SEMANA.Attributes.Append(ID);

                        XmlElement horas = doc.CreateElement("Horas_Totales"); // CREA EL NODO HORAS TOTALES
                        horas.AppendChild(doc.CreateTextNode(nomina[i].Horas_pre.ToString()));
                        SEMANA.AppendChild(horas);

                        XmlElement ValPHora = doc.CreateElement("ValPrecio_Hora_Pre"); // CREA COMO NODO EL VALOR DE LAS HORAS ORDINALES 
                        ValPHora.AppendChild(doc.CreateTextNode(nomina[i].PrecioPre.ToString()));
                        SEMANA.AppendChild(ValPHora);

                        XmlElement Jorpre = doc.CreateElement("Jornada_Pre"); // CREA COMO NODO LA JORNADA PREDETERMINADA
                        Jorpre.AppendChild(doc.CreateTextNode(nomina[i].JornadaPre.ToString()));
                        SEMANA.AppendChild(Jorpre);

                        XmlElement ValHExtrasPre = doc.CreateElement("ValHExtras_Pre"); // CREA COMO NODO EL VALOR DE LAS HORAS EXTRAS
                        ValHExtrasPre.AppendChild(doc.CreateTextNode(nomina[i].HextrasPre.ToString()));
                        SEMANA.AppendChild(ValHExtrasPre);

                        XmlElement ValRet = doc.CreateElement("ValRetencion_Pre"); // CREA COMO NODO EL VALOR DE LA RETENCIÓN DE ESA SEMANA
                        ValRet.AppendChild(doc.CreateTextNode(nomina[i].RetencionPre.ToString()));
                        SEMANA.AppendChild(ValRet);

                        doc.Save(rutaNOM + "\\" + dni_glo + ".xml"); // GUARDA EL ARCHIVO CON EL DNI DEL TRABAJADOR
                        salir = true;
                    }
                    else
                    {
                        salir = true;
                    }
                }
            } while (!salir);
        }

        public static void FormatNomina() // BORRA EL CONTENIDO DE LA NÓMINA DE UN TRABAJADOR EN PARTICULAR 
        {
            doc = new XmlDocument();
            doc.Load(rutaNOM + "\\" + dni_glo + ".xml");
            XmlNode root = doc.DocumentElement;
            root.RemoveAll();
            doc.Save(rutaNOM + "\\" + dni_glo + ".xml");
        }

        public static string BuscarNombre(string dni) // BUSCA EL FICHERO XML RELACIONADO A UN TRABAJADOR Y DEVUELVE LA RUTA
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

        public static Nomina[] GetNomina(string dni) // OBTIENE TODAS LAS SEMANAS QUE TIENE ACTUALMENTE EL EMPLEADO
        {
            string ruta = null;
            int id = 0, horas = 0;
            int jornadapre = 0;
            float hextraspre = 0.0F, retencionespre = 0.0F, preciopre = 0.0F;
            dni_glo = dni;
            ruta = BuscarNombre(dni);
            Nomina Nom = null;
            Nomina[] ArraySemanas = null;
            doc = new XmlDocument();

            try
            {
                doc.Load(ruta);
                XmlNodeList ListaSemanas = doc.SelectNodes("Nomina/Semana"); // ACCEDE AL NODO SEMANA DENTRO DE NOMINA
                XmlNode UnaSemana = null;

                ArraySemanas = new Nomina[ListaSemanas.Count];
                for (int i = 0; i < ListaSemanas.Count; i++)
                {
                    Nom = new Nomina();
                    UnaSemana = ListaSemanas.Item(i); // RECORRE CADA SEMANA Y OBTIENE SUS PROPIEDADES

                    id = Int32.Parse(UnaSemana.Attributes.GetNamedItem("ID").InnerText); // OBTIENE EL ID
                    Nom.ID_pre = id;
                    horas = Int32.Parse(UnaSemana.SelectSingleNode("Horas_Totales").InnerText); // OBTIENE LAS HORAS TOTALES DE LA SEMANA
                    Nom.Horas_pre = horas;
                    // Valores predeterminados CONF
                    jornadapre = Int32.Parse(UnaSemana.SelectSingleNode("Jornada_Pre").InnerText); // OBTIENE LA JORNADA PREDETERMINADA DE ESA SEMANA
                    Nom.JornadaPre = jornadapre;
                    hextraspre = Single.Parse(UnaSemana.SelectSingleNode("ValHExtras_Pre").InnerText); // OBTIENE EL VALOR DE LAS HORAS EXTRAS
                    Nom.HextrasPre = hextraspre;
                    preciopre = Single.Parse(UnaSemana.SelectSingleNode("ValPrecio_Hora_Pre").InnerText); // OBTIENE EL VALOR DE LA HORA ORDINARIA DE ESA SEMANA
                    Nom.PrecioPre = preciopre;
                    retencionespre = Single.Parse(UnaSemana.SelectSingleNode("ValRetencion_Pre").InnerText); // OBTIENE LA RETENCION APLICADA ESA SEMANA
                    Nom.RetencionPre = retencionespre;
                    //

                    ArraySemanas[i] = Nom; // METE EN EL ARRAY EL OBJETO DE LA SEMANA CREADO
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

        public static void BorrarTemporal(string dni) // BORRA LA NÓMINA TEMPORAL UNA VEZ CERRADA.
        {
            FileInfo[] archivos = null;
            string name = null;

            DirectoryInfo d = new DirectoryInfo(rutaNOM);
            archivos = d.GetFiles("*.xml");
            if (archivos != null)
            {                                               // LISTA EL DIRECTORIO, BUSCA EL ARCHIVO POR EL DNI DEL TRABAJADOR Y LO BORRA.
                foreach (FileInfo file in archivos)
                {
                    name = Path.GetFileNameWithoutExtension(file.Name);
                    if (dni.Equals(name))
                    {
                        try
                        {
                            File.Delete(rutaNOM + "\\" + dni + ".xml");
                        }
                        catch (Exception)
                        {
                            throw new Exception("No se ha podido borrar el archivo");
                        }
                        break;
                    }
                }
            }
        }
        public static void ExistOrEmptyNOM(string dni) // COMPRUEBA SI LA NOMINA DE UN TRABAJADOR EXISTE O NO, A LA HORA DE LOGUEARSE
        {
            FileInfo[] archivos = null;
            bool flag = false;

            DirectoryInfo d = new DirectoryInfo(rutaNOM);
            archivos = d.GetFiles("*.xml");

            if (archivos != null)
            {                                                   // ACCEDE AL DIRECTORIO Y LO RECORRE COMPROBANDO EL NOMBRE DE LOS ARCHIVOS
                foreach (FileInfo file in archivos)             // SI COINCIDE CON EL DNI DEL TRABAJADOR ENTONCES EXISTE, SINO NO.
                {
                    if (dni.Equals(Path.GetFileNameWithoutExtension(file.Name)))
                    {
                        flag = true;
                    }
                }
            }

            if (flag == false) // EN EL CASO DE QUE NO EXISTA CREA UNA NÓMINA PREDETERMINADA (SIN SEMANAS).
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
        public static void setConfig() // ESTABLECER LA CONFIGURACIÓN DE INICIO PREDETERMINADA
        {
            doc = new XmlDocument();
            if (!File.Exists(rutaConf))
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null); // CABECERA
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "Configuracion", string.Empty); // NODO PADRE CONFIGURACIÓN
                doc.AppendChild(element1);

                XmlElement jornada = doc.CreateElement("Jornada"); // JORNADA PREDETERMINADA 40 HORAS
                jornada.AppendChild(doc.CreateTextNode("40"));
                element1.AppendChild(jornada);

                XmlElement Hextras = doc.CreateElement("Horas_Extras"); // EL VALOR DE LAS HORAS EXTRAS SIEMPRE ES 1.5
                Hextras.AppendChild(doc.CreateTextNode("1.5"));
                element1.AppendChild(Hextras);

                XmlElement retencion = doc.CreateElement("Retencion"); // RETENCION PREDETERMINADA 16%
                retencion.AppendChild(doc.CreateTextNode("0.16"));
                element1.AppendChild(retencion);
                doc.Save(rutaConf);
            }
        }

        public static void getConfig(ref int jornada, ref float Hextras, ref float retencion) // ESTE METODO SE USA PARA OBTENER LOS VALORES DEL ARCHIVO DE CONFIGURACIÓN
        {
            string hextras_provisional = null;
            string retencion_provisional = null;
            doc = new XmlDocument();
            try
            {
                doc.Load(rutaConf);
                XmlNodeList raiz = doc.SelectNodes("Configuracion");
                foreach (XmlNode conf in raiz)
                {
                    jornada = Int32.Parse(conf.SelectSingleNode("Jornada").InnerText); // VALOR PREDETERMINADO DE JORNADA
                    hextras_provisional = conf.SelectSingleNode("Horas_Extras").InnerText; // VALOR HORAS EXTRAS
                    retencion_provisional = conf.SelectSingleNode("Retencion").InnerText; // VALOR RETENCION
                }
                Hextras = Convert.ToSingle(hextras_provisional.ToString(), CultureInfo.InvariantCulture); // SI NO SE LE ASIGNABA "CULTUREINFO" DABA FALLO EN LA CONVERSIÓN.
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

        public static void ModConfig(int option, float valor) // SE LLAMA A ESTE MÉTODO PARA MODIFICAR LOS DISTINTOS MODULOS DEL ARCHIVO DE CONFIGURACIÓN, MENOS EL VALOR DE LAS HORAS EXTRAS
        {
            doc = new XmlDocument();
            doc.Load(rutaConf);
            XmlNode nodo;
            nodo = doc.DocumentElement;

            foreach (XmlNode node1 in nodo.ChildNodes)
                if (node1.Name.Equals("Jornada") && option == 1) { // NODO JORNADA
                    int newJor = Int32.Parse(valor.ToString());
                    node1.InnerText = newJor.ToString();
                    break;
                }  else if (node1.Name.Equals("Retencion") && option == 2) { // LAS RETENCIONES SE GUARDAN DIVIDIDAS ENTRE 100 PARA SU POSTERIOR CALCULO
                    float newRet = float.Parse(valor.ToString(), CultureInfo.InvariantCulture);
                    newRet = newRet / 100;
                        node1.InnerText = newRet.ToString();
                        break;
                }
            doc.Save(rutaConf);
        }
        #endregion FIN ARCHIVO CONFIGURACIÓN

        #region FICHEROS TXT - Francisco Romero
        // CREAR TXT
        public static void CerrarNomina(string cadena, string fecha) // CREA EL ARCHIVO DE TEXTO CON LA NÓMINA FINAL EN EL DIRECTORIO "B.D_Cerradas"
        {
            bool salir = false;
            try
            {
                do
                {
                    StreamWriter sw = File.CreateText(nominascerradas + fecha + dni_glo + ".txt"); // "CREA EL FICHERO"
                    sw.WriteLine(cadena); // ESCRIBE LA NÓMINA FINAL EN EL FICHERO TXT
                    sw.Close(); // CIERRA EL ARCHIVO
                    salir = true;
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
