using System;
using System.Collections.Generic;
using System.Text;

namespace Poc_Redis_Docker
{
    public class Agencia
    {
        public string agencia { get; set; }
        public string grupo { get; set; }
        public string evento { get; set; }
        public string chave_cache { get; private set; }
        //public string chave_cache { get; set; }

        public Agencia()
        //public Agencia(string agenciaC, string grupoC, string eventoC)
        {
            chave_cache = "AG:"+ Guid.NewGuid();
            //chave_cache = agenciaC + ":" + grupoC + ":" + eventoC;
            //agencia = agenciaC;
            //grupo = grupoC;
            //evento = eventoC;
        }
    }
}
