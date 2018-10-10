using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.control
{
    class Analizador
    {
        public string[] leerArchivo()
        {
            string[] codigo;
            List<String> lineas = new List<String>();
            StreamReader reader;
            try
            {
                reader = new StreamReader("Ejemplo.txt");
                string linea = "";
                while (linea != null)
                {
                    linea = reader.ReadLine();
                    if (linea != null)
                    {
                        lineas.Add(linea);
                    }
                }
                codigo = new string[lineas.Count];
                for (int i = 0; i < codigo.Length; i++)
                {
                    codigo[i] = lineas[i];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                codigo = new string[0];
            }

            return codigo;
        }
    }
}
