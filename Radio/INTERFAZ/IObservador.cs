using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.INTERFAZ
{
    public interface IObservador
    {
        void actualizarInformacion(String resultadoDeVentanaOrigen, int ventanaOrigen);
    }
}
