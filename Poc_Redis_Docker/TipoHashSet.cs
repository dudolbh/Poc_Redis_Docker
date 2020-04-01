using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Poc_Redis_Docker
{
    class TipoHashSet
    {
        public string LeDadoColuna(string key, string coluna)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            var valor = cache.HashGet(key, coluna);
            return valor.ToString();
        }

        public List<HashEntry> LeDadoCompleto(string key)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            List<HashEntry> lista = new List<HashEntry>();
            var valores = cache.HashGetAll(key);
            
            foreach (var valor in valores)
            {
                lista.Add(valor);
            }
            return lista;
        }

        /*public List<Task<HashEntry[]>> LeDadoCompletoAsync(string mascara)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            List<Task<HashEntry[]>> lista = new List<Task<HashEntry[]>>();

            foreach (var hash in ListaChave(mascara))
            {
                var valor = cache.HashGetAllAsync(hash);
                lista.Add(valor);
            }
            var valore = await Task.WhenAll(lista);
            return lista;
        }*/

        public void GravaDado(List<Agencia> agenciaLista)
        {
            var cache = RedisConexao.Connection.GetDatabase();

            foreach (var agencia in agenciaLista)
            {
                cache.HashSet(agencia.chave_cache, new HashEntry[] {
                    new HashEntry("agencia", agencia.agencia),
                    new HashEntry("grupo", agencia.grupo),
                    new HashEntry("evento", agencia.evento)}
                );
            }
        }

        public string DictToString<T, V>(IEnumerable<KeyValuePair<T, V>> items, string format)
        {
            format = String.IsNullOrEmpty(format) ? "{0}='{1}' " : format;

            StringBuilder itemString = new StringBuilder();
            foreach (var item in items)
                itemString.AppendFormat(format, item.Key, item.Value);

            return itemString.ToString();
        }
    }
}
