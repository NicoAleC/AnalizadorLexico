using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using System.Threading.Tasks;

namespace AnalizadorLexico.control
{
    class Analizador
    {
        public string[] leerArchivo()
        {
            string[] codigo;
            List<string> lineas = new List<string>();
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


        public bool esIdentificador(string cadena)
        {

            string patron = @"^[^\d].*$";

            Match match = Regex.Match(cadena, patron);

            return match.Success;
        }

        public bool esPalabraReservada(string cadena)
        {

            string patron = @"^change$|^changed$|^given$|^otherwise$|^done$|^return$|^forevery$|^forever$|^done$|^in$|^stop$|^KYU#$|^is$";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool esNumero(string cadena)
        {

            string patron = @"\\d";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool esOperador(string cadena){

            string patron = @"\+|-|\*|\/|\^|=|!|<|>|,";
            Match match = Regex.Match(cadena, patron);
            return match.Success;     
        }

        public bool esPoLoC(string cadena){

            string patron = @"\]|\(|\)|{|}|\[";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool esComilla(string cadena){
            string comilla = "\"";
            string patron = @comilla + "|'";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool esCadena(String cadena){
            string comilla = "\"";
            string patron = comilla + ".\\w." +comilla;
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }






    }
}
