using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class Cantantes
    {
        private Int64 CNT_ID;
        private String CNT_NOMBRE;
        private String CNT_TIPO;
        private DateTime CNT_FECHAMODIFICACION;
        private DateTime CNT_FECHAALTA;
        private String CNT_ESTATUS;

        public long CNT_ID1 { get => CNT_ID; set => CNT_ID = value; }
        public string CNT_NOMBRE1 { get => CNT_NOMBRE; set => CNT_NOMBRE = value; }
        public string CNT_TIPO1 { get => CNT_TIPO; set => CNT_TIPO = value; }
        public DateTime CNT_FECHAMODIFICACION1 { get => CNT_FECHAMODIFICACION; set => CNT_FECHAMODIFICACION = value; }
        public DateTime CNT_FECHAALTA1 { get => CNT_FECHAALTA; set => CNT_FECHAALTA = value; }
        public string CNT_ESTATUS1 { get => CNT_ESTATUS; set => CNT_ESTATUS = value; }

        public override string ToString()
        {
            return CNT_NOMBRE;
        }
    }
}
