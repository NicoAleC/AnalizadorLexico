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
            Analizador a = new Analizador();
            string[] codigo = a.LeerArchivo();
            Token[] tokens = a.Reconocer(codigo);

            for(int i = 0; i < codigo.Length; i++)
            {
                Console.WriteLine(codigo[i]);
            }

            for (int i = 0; i < tokens.Length; i++)
            {
                Console.WriteLine(tokens[i].ToString());
            }
          

            Console.ReadKey();
        }
    }
}
