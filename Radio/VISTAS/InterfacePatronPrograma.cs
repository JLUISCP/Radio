using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.VISTAS
{
    public interface InterfacePatronPrograma
    {
        void agregar(int numeroDia, int numeroCanciones, List<Cancion> listaCanciones);
    }
}
