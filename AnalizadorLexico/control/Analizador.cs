﻿using AnalizadorLexico.entity;
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
        public bool Analizar(string filePath) {
            string[] codigo = this.LeerArchivo(filePath);
            Token[] tokens = this.Reconocer(codigo);

            for (int i = 0; i < codigo.Length; i++)
            {
                Console.WriteLine(codigo[i]);
            }

            for (int i = 0; i < tokens.Length; i++)
            {
                Console.WriteLine(tokens[i].ToString());
            }
            return false;
        }
        public string[] LeerArchivo(string filePath)
        {
            string[] codigo;
            List<string> lineas = new List<string>();
            StreamReader reader;
            try
            {
                reader = new StreamReader(filePath);
                string linea = "";
                while (linea != null)
                {
                    linea = reader.ReadLine();
                    if (linea != null)
                    {
                        linea = linea.Trim();
                        if (linea != "") {
                            linea = Regex.Replace(linea, @"\s+", " ");
                            linea = linea.Split(';')[0];
                            lineas.Add(linea);
                        }
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


        public bool EsIdentificador(string cadena)
        {

            string patron = "^[^\\d].*$";

            Match match = Regex.Match(cadena, patron);

            return match.Success;
        }

        public bool EsPalabraReservada(string cadena)
        {

            string patron = @"^change$|^changed$|^given$|^otherwise$|^done$|^return$|^forevery$|^forever$|^done$|^in$|^stop$|^kyu#$|^is$";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool EsNumero(string cadena)
        {

            string patron = @"^~?[0-9]+.?[0-9]+$";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool EsOperadorA(string cadena){

            string patron = "\\+|-|\\*|/|\\^|!";
            Match match = Regex.Match(cadena, patron);
            return match.Success;     
        }

        public bool EsOperadorL(string cadena)
        {
            string patron = "&|&&|\\||\\|\\||~";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool EsComparador(string cadena)
        {
            string patron = "<|>|==|~=|>=|<=";
            Match match = Regex.Match(cadena, patron);
            return match.Success;
        }

        public bool EsSimboloA(string cadena){

            string patron = "\\(|\\)|\\[|\\]|{|}";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool EsComilla(string cadena){
            string patron = "\"|'";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool EsCadena(string cadena){
            
            string patron = "^\"[\\w|\\s|\\W]*\"$";
            Match match = Regex.Match(cadena, patron);
            return match.Success; 
        }

        public bool EsSimbolo(string cadena)
        {
            string patron = ":|\\(|\\)|=|\\[|\\]|{|}|-|\\+|>|<|,|\\*|/|!|\"|'|&|\\|\\||\\||~";
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
                //no borrar estas 2 líneas por favor, son para evitar excepciones
                caracteres = new string[codigo[i].Length + 1];
                caracteres[codigo[i].Length] = "";

                //no quitar el -1
                for (int j = 0; j < caracteres.Length - 1; j++)
                {
                    caracteres[j] = codigo[i].Substring(j, 1);
                }

                if (!aux.Equals("") && !aux.Equals(" "))
                {
                    auxTokens.Add(new Token(aux, i, 0));
                    aux = "";
                }

                //no quitar el -1
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
                    }
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
                        if (!aux.Equals(""))
                        {
                            auxTokens.Add(new Token(aux, i, j));
                        }
                        aux = "";
                    }
                    else if (EsSimbolo(caracteres[j]))
                    {
                        if (EsSimbolo(caracteres[j]) && caracteres[j + 1].Equals("="))
                        {
                            if (!EsSimboloA(caracteres[j]))
                            {
                                auxTokens.Add(new Token(caracteres[j] + caracteres[j + 1], i, j));
                                j++;
                            }
                            else
                            {
                                auxTokens.Add(new Token(caracteres[j], i, j));
                            }
                            aux = "";
                        }
                    }
                    else if (EsSimbolo(caracteres[j]))
                    {
                        auxTokens.Add(new Token(aux, i, j));
                        auxTokens.Add(new Token(caracteres[j], i, j));
                        aux = "";
                    }
                }

                if (i == codigo.Length - 1 && !aux.Equals(" ") && !aux.Equals(""))
                {
                    auxTokens.Add(new Token(aux, i, codigo.Length - 1));
                }

            }
            
            tokens = new Token[auxTokens.Count];
            for (int i = 0; i < auxTokens.Count; i++)
            {
                tokens[i] = auxTokens[i];
            }

            for (int i = 0; i < auxTokens.Count; i++)
            {
                if (EsCadena(tokens[i].lexema))
                {
                    tokens[i].token = "Cadena";
                }
                else if (EsPalabraReservada(tokens[i].lexema))
                {
                    tokens[i].token = "Palabra Reservada";
                }
                else if (EsSimboloA(tokens[i].lexema))
                {
                    tokens[i].token = "Agrupación";
                }
                else if (EsNumero(tokens[i].lexema))
                {
                    tokens[i].token = "Número";
                }
                else if (EsOperadorA(tokens[i].lexema))
                {
                    tokens[i].token = "Operador Aritmético";
                }
                else if (EsOperadorL(tokens[i].lexema))
                {
                    tokens[i].token = "Operador Lógico";
                }
                else if (EsComparador(tokens[i].lexema))
                {
                    tokens[i].token = "Comparador";
                }
                else if (tokens[i].lexema.Equals(","))
                {
                    tokens[i].token = "Separador";
                }
                else if (EsIdentificador(tokens[i].lexema))
                {
                    tokens[i].token = "Identificador";
                }
                else
                {
                    tokens[i].token = "No definido";
                }
                
            }
            return tokens;
        }

        


    }
}
