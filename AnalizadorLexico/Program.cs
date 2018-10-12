using AnalizadorLexico.control;
using AnalizadorLexico.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico
{
    class Program
    {
        static void Main(string[] args)
        {
            Evaluator ev = new Evaluator();
            Dictionary<string,string> posibilities = ev.doDivition("i P is\nM\ndone","getNombre a,b is\ngiven a > b return a\ndone");
            foreach (KeyValuePair<string, string> kvp in posibilities)
            {
                    Console.WriteLine(string.Format("Key = {0} , Value = {1}", kvp.Key, kvp.Value));
            }
            // Analizador a = new Analizador();
            // string[] codigo = a.LeerArchivo();
            // Token[] tokens = a.Reconocer(codigo);

            // for(int i = 0; i < codigo.Length; i++)
            // {
            //     Console.WriteLine(codigo[i]);
            // }

            // for (int i = 0; i < tokens.Length; i++)
            // {
            //     Console.WriteLine(tokens[i].ToString());
            // }
          

            Console.ReadKey();
        }
    }

    public class Evaluator 
    {
        private char[] terminals = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '+', '-', '*', '/', '^', '(', ')' , ',',
                                    '>' , '<' , '|' , '~' , '{' , '}' , 'i' , 'v'};
        private string[] keyWords = {"kyu#" , "stop" , "return" , "is", "done" , "change" , "changed" , "forevery" , "in",
                                    "forever" , "given" , "otherwise"};

         public Dictionary<string,char> getDivision(string rule) { 
            
            List<string> keyWords = new List<string>();
            Dictionary<string,char> divisors = new Dictionary<string,char>();

            string[] splited = rule.Split(new char[] {' ' , '\n'});

            int index = 0;
            char separator;

            for(int i=0; i<splited.Count(); i++) 
            {
                index = rule.IndexOf(splited[i]);
                if((index + splited[i].Length) >= rule.Length)
                {
                    separator = 'X';
                } 
                else 
                {
                    separator = rule[index + splited[i].Length];
                }
               divisors.Add(splited[i],separator);
            }

            return divisors;
        }

         public Dictionary<string,string> doDivition(string rule, string input)
        {
            Dictionary<string,char> howToDivide = this.getDivision(rule);
            Dictionary<string,string> division = new Dictionary<string, string>();
            foreach (KeyValuePair<string, char> kvp in howToDivide)
            {
                if(kvp.Value != 'X') 
                {
                    string value = input.Substring(0,input.IndexOf(kvp.Value));
                    input = input.Substring(input.IndexOf(kvp.Value) + 1);
                    if(this.keyWords.Contains(kvp.Key) || this.terminals.Contains(kvp.Key[0])) continue;
                    division.Add(kvp.Key,value);
                } 
                else if (kvp.Value == 'X') 
                {
                    if(this.keyWords.Contains(kvp.Key) || this.terminals.Contains(kvp.Key[0])) continue;
                    division.Add(kvp.Key,input);
                }
            }
          return division;
        }
    }
}
