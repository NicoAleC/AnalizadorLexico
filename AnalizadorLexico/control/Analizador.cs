using AnalizadorLexico.entity;
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
        public string[] LeerArchivo()
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

                Console.WriteLine("lineas leídas: " + lineas.Count);
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


        public bool EsIdentificador(string cadena)
        {

            string patron = @"^[^\\d].*$";

            Match match = Regex.Match(cadena, patron);

            return match.Success;
        }

        public bool EsPalabraReservada(string cadena)
        {

            string patron = @"^change$|^changed$|^given$|^otherwise$|^done$|^return$|^forevery$|^forever$|^done$|^in$|^stop$|^KYU#$|^is$";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool EsNumero(string cadena)
        {

            string patron = "^~?[0-9|.]+";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool EsOperador(string cadena){

            string patron = "\\+|-|\\*|/|\\^|!|<|>";
            Match match = Regex.Match(cadena, patron);
            return match.Success;     
        }

        public bool EsPoLoC(string cadena){

            string patron = "\\(|\\)|\\[|\\]|{|}";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool EsComilla(string cadena){
            //string comilla = "\"";
            //string patron = @"" + comilla + "|'";
            string patron = "\"|'";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool EsCadena(string cadena){
            
            string patron = "^\"[\\w|\\s|\\W]*";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool EsComentario(string cadena)
        {
            string patron = "^#[\\w|\\s]*";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool EsSimbolo(string cadena)
        {
            string patron = ":|\\(|\\)|=|\\[|\\]|{|}|-|\\+|>|<|,|\\*|/|!|\"|'|-=|\\+=|!=|&&|&|\\|\\||\\||<=|>=";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public Token[] Reconocer(string[] codigo)
        {
            List<Token> auxTokens = new List<Token>();
            Token[] tokens;
            string[] caracteres;
            string aux = "";

            for (int i = 0; i < codigo.Length; i++)
            {
                caracteres = new string[codigo[i].Length + 1];
                caracteres[codigo[i].Length] = "";

                for (int j = 0; j < caracteres.Length - 1; j++)
                {
                    caracteres[j] = codigo[i].Substring(j, 1);
                }

                if (!aux.Equals(""))
                {
                    auxTokens.Add(new Token(aux, i, 0));
                    aux = "";
                }

                for (int j = 0; j < caracteres.Length - 1; j++)
                {
                    if (!caracteres[j].Equals(" ") && !caracteres[j].Equals("\t") && !EsSimbolo(caracteres[j]))
                    {
                        aux += caracteres[j];
                    }//si es una cadena
                    else if (EsComilla(caracteres[j]))
                    {
                        aux = caracteres[j];
                        j++;
                        while (!EsComilla(caracteres[j]))
                        {
                            aux += caracteres[j];
                            j++;
                        }
                        aux += caracteres[j];
                        auxTokens.Add(new Token(aux, i, j));
                        aux = "";
                    }//si es un comentario
                    else if (caracteres[j].Equals("_"))
                    {
                        aux = caracteres[j];
                        j++;
                        while (j < caracteres.Length - 1)
                        {
                            aux += caracteres[j];
                            j++;
                        }
                        aux += caracteres[j];
                        auxTokens.Add(new Token(aux, i, j));
                        aux = "";
                    }//si es numero
                    else if (caracteres[j].Equals("~") && EsNumero(caracteres[j + 1]))
                    {
                        aux = caracteres[j];
                        j++;
                        while ((!caracteres[j].Equals("\t") || !caracteres[j].Equals(" ")) && j < caracteres.Length - 1)
                        {
                            aux += caracteres[j];
                            j++;
                        }
                        aux += caracteres[j];
                        auxTokens.Add(new Token(aux, i, j));
                        aux = "";
                    }
                    else if (caracteres[j].Equals("\t") || caracteres[j].Equals(" "))
                    {
                        if (!aux.Equals("") )
                        {
                            auxTokens.Add(new Token(aux, i, j));
                        }
                        aux = "";
                    }
                    else if (EsSimbolo(caracteres[j]))
                    {
                        auxTokens.Add(new Token(aux, i, j));
                        auxTokens.Add(new Token(caracteres[j], i, j));
                        aux = "";
                    }
                }

                if (i == codigo.Length - 1)
                {
                    auxTokens.Add(new Token(aux, i, codigo.Length - 1));
                }

            }

            Console.WriteLine("array list de tokens :" + auxTokens.Count);
            tokens = new Token[auxTokens.Count];
            for (int i = 0; i < auxTokens.Count; i++)
            {
                tokens[i] = auxTokens[i];
            }

            for (int i = 0; i < auxTokens.Count; i++)
            {
                if(EsCadena(tokens[i].lexema))
                {
                    tokens[i].token = "Cadena";
                } 
                else if(EsPalabraReservada(tokens[i].lexema))
                {
                    tokens[i].token = "Palabra Reservada";
                }
                else if (EsPoLoC(tokens[i].lexema))
                {
                    tokens[i].token = "Agrupación";
                }
                else if (EsNumero(tokens[i].lexema))
                {
                    tokens[i].token = "Número";
                }
                else if (EsOperador(tokens[i].lexema))
                {
                    tokens[i].token = "Operador";
                }
                else if (EsComentario(tokens[i].lexema))
                {
                    tokens[i].token = "Comentario";
                }
                else if (tokens[i].lexema.Equals(","))
                {
                    tokens[i].token = "separador";
                }
                else if (EsIdentificador(tokens[i].lexema))
                {
                    tokens[i].token = "Identificador";
                }
                
            }
            Console.WriteLine("arreglo de tokens: " + tokens.Length);
            return tokens;
        }

        


    }
}
