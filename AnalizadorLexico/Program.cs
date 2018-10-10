using AnalizadorLexico.control;
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
            string[] codigo = a.leerArchivo();

            for (int i = 0; i < codigo.Length; i++)
            {
                Console.WriteLine(codigo[i]);
            }

            Console.ReadKey();
        }
    }
}
