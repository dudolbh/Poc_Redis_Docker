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
        public void GravaDado(List<Agencia> agenciaLista)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            foreach (var agencia in agenciaLista)
            {
                var key = agencia.chave_cache;
                var value = JsonConvert.SerializeObject(agencia);
                cache.StringSet(key, value);
            }
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
        
            foreach(var agencia in agenciaLista)
            {
                var key = agencia.chave_cache;
                var value = JsonConvert.SerializeObject(agencia);
                batch.Add(new KeyValuePair<RedisKey, RedisValue>(key, value));

                if (batch.Count == BatchSize)
                {
                    cache.StringSet(batch.ToArray());
                    batch.Clear();
                }
            }
            if (batch.Count != 0) // batch final
                cache.StringSet(batch.ToArray());

            //Fonte: https://github.com/StackExchange/StackExchange.Redis/issues/1089
        }


        public async Task<bool> GravaDadoBatchAsync(List<Agencia> agenciaLista)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            const int BatchSize = 100;
            var batch = new List<KeyValuePair<RedisKey, RedisValue>>(BatchSize);

            List<Task<bool>> listaTaskBool = new List<Task<bool>>();

            foreach (var agencia in agenciaLista)
            {
                var key = agencia.chave_cache;
                var value = JsonConvert.SerializeObject(agencia);
                batch.Add(new KeyValuePair<RedisKey, RedisValue>(key, value));

                if (batch.Count == BatchSize)
                {
                    listaTaskBool.Add(cache.StringSetAsync(batch.ToArray()));
                    batch.Clear();
                }
            }
            if (batch.Count != 0) // batch final
            {
                listaTaskBool.Add(cache.StringSetAsync(batch.ToArray()));
            }
            var resultado = await Task.WhenAll(listaTaskBool);
            return resultado.All(a => a == true);
        }

        public Agencia LeituraDados(string key)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            return JsonConvert.DeserializeObject<Agencia>(cache.StringGet(key));
        }

        public bool ExcluiDado(string key)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            return cache.KeyDelete(key);
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
