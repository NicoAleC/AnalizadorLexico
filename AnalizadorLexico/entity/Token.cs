﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico.entity
{
    class Token
    {
        private string token { get; set; }//describe si es palabra reservada u otro
        private string lexema { get; set; }//lo que es
        private int linea { get; set; }
        private int columna { get; set; }
        private string descripcion { get; set; }//en caso de necesitar una descripción más detallada

        public Token()
        {
            this.token = "";
            this.lexema = "";
            this.linea = 0;
            this.columna = 0;
            this.descripcion = "";
        }

        public Token(string token, string lexema, int linea, int columna)
        {
            this.token = token;
            this.lexema = lexema;
            this.linea = linea;
            this.columna = columna;
            this.descripcion = "";
        }
    }
}
