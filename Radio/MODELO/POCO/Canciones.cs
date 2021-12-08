using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.POCO
{
    class Canciones
    {
        private Int64 CAN_ID;
        private String CAN_TITULO;
        private String CAN_CLAVE;
        private String CAN_DIAS;
        private Int64 CAN_PRIORIDAD;
        private DateTime CAN_FECHAALTA;
        private DateTime CAN_FECHAMODIFICACION;
        private int CAN_ESTATUS;
        private DateTime CAN_FECHABLOQUEO;
        private String CAN_COMENTARIOS;
        private String CAN_OBSERVACIONES;
        private Int64 CNT_ID;
        private String CNT_NOMBRE;
        private Int64 CAT_ID;
        private String CAT_NOMBRE;
        private Int64 GNR_ID;
        private String GNR_NOMBRE;

        public long CAN_ID1 { get => CAN_ID; set => CAN_ID = value; }
        public string CAN_TITULO1 { get => CAN_TITULO; set => CAN_TITULO = value; }
        public string CAN_CLAVE1 { get => CAN_CLAVE; set => CAN_CLAVE = value; }
        public string CAN_DIAS1 { get => CAN_DIAS; set => CAN_DIAS = value; }
        public long CAN_PRIORIDAD1 { get => CAN_PRIORIDAD; set => CAN_PRIORIDAD = value; }
        public DateTime CAN_FECHAALTA1 { get => CAN_FECHAALTA; set => CAN_FECHAALTA = value; }
        public DateTime CAN_FECHAMODIFICACION1 { get => CAN_FECHAMODIFICACION; set => CAN_FECHAMODIFICACION = value; }
        public int CAN_ESTATUS1 { get => CAN_ESTATUS; set => CAN_ESTATUS = value; }
        public DateTime CAN_FECHABLOQUEO1 { get => CAN_FECHABLOQUEO; set => CAN_FECHABLOQUEO = value; }
        public string CAN_COMENTARIOS1 { get => CAN_COMENTARIOS; set => CAN_COMENTARIOS = value; }
        public string CAN_OBSERVACIONES1 { get => CAN_OBSERVACIONES; set => CAN_OBSERVACIONES = value; }
        public long CNT_ID1 { get => CNT_ID; set => CNT_ID = value; }
        public string CNT_NOMBRE1 { get => CNT_NOMBRE; set => CNT_NOMBRE = value; }
        public long CAT_ID1 { get => CAT_ID; set => CAT_ID = value; }
        public string CAT_NOMBRE1 { get => CAT_NOMBRE; set => CAT_NOMBRE = value; }
        public long GNR_ID1 { get => GNR_ID; set => GNR_ID = value; }
        public string GNR_NOMBRE1 { get => GNR_NOMBRE; set => GNR_NOMBRE = value; }
    }
}
