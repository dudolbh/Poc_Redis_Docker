using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace Poc_Redis_Docker
{
    class TipoString
    {
        public void GravaDado(Agencia agencia)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            var key = agencia.chave_cache;
            var value = JsonConvert.SerializeObject(agencia);
            cache.StringSet(key, value);
        }

        public void GravaDadoExpira(List<Agencia> agenciaLista, TimeSpan exipracao)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            foreach (var agencia in agenciaLista)
            {
                var key = agencia.chave_cache;
                var value = JsonConvert.SerializeObject(agencia);
                cache.StringSet(key, value, exipracao);
            }
        }

        public void GravaDadoBatch(List<Agencia> agenciaLista)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            const int BatchSize = 100;
            var batch = new List<KeyValuePair<RedisKey, RedisValue>>(BatchSize);
            // Faz lote de 100 registros e manda para o banco inserir via MSET.
            foreach(var agencia in agenciaLista)
            {
                var key = agencia.chave_cache;
                var value = JsonConvert.SerializeObject(agencia);
                batch.Add(new KeyValuePair<RedisKey, RedisValue>(key, value));

                if (batch.Count == BatchSize)
                {
                    cache.StringSet(batch.ToArray());
                    //limpa o a batch para adicionar novos registros.
                    batch.Clear();
                }
            }
            if (batch.Count != 0) // batch final insere os registros restantes.
                cache.StringSet(batch.ToArray());

            //Fonte: https://github.com/StackExchange/StackExchange.Redis/issues/1089
        }


        public async Task<bool> GravaDadoBatchAsync(List<Agencia> agenciaLista)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            const int BatchSize = 100;
            var batch = new List<KeyValuePair<RedisKey, RedisValue>>(BatchSize);

            List<Task<bool>> listaTaskBool = new List<Task<bool>>();

            // Faz lote de 100 registros e manda para o banco inserir via MSET.
            foreach (var agencia in agenciaLista)
            {
                var key = agencia.chave_cache;
                var value = JsonConvert.SerializeObject(agencia);
                batch.Add(new KeyValuePair<RedisKey, RedisValue>(key, value));

                if (batch.Count == BatchSize)
                {
                    //Adiciona o registro de forma assincrona
                    listaTaskBool.Add(cache.StringSetAsync(batch.ToArray()));
                    //limpa o a batch para adicionar novos registros.
                    batch.Clear();
                }
            }
            if (batch.Count != 0) // batch final insere os registros restantes.
            {
                listaTaskBool.Add(cache.StringSetAsync(batch.ToArray()));
            }
            var resultado = await Task.WhenAll(listaTaskBool);
            return resultado.All(a => a == true);
        }

        public Agencia LeituraDados(string key)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            //Busca chave e converte em objeto.
            return JsonConvert.DeserializeObject<Agencia>(cache.StringGet(key));
        }

        public List<Agencia> LeituraDadosLista(RedisKey[] keys)
        {
            List<Agencia> agenciaL = new List<Agencia>();
            var cache = RedisConexao.Connection.GetDatabase();
            //Busca chaves
            var valores = cache.StringGet(keys);
            foreach (var valor in valores)
            {
                agenciaL.Add(JsonConvert.DeserializeObject<Agencia>(valor));
            }
            return agenciaL;
        }

        /*
        public void GravaDadoAsyc(List<Agencia> agenciaLista)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            foreach (var agencia in agenciaLista)
            {
                var key = agencia.chave_cache;
                var value = JsonConvert.SerializeObject(agencia);
                await cache.StringSetAsync(key, value);
            }
        }
        */


    }
}
