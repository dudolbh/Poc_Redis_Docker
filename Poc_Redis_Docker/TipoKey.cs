using System;
using System.Collections.Generic;
using StackExchange.Redis;
using System.Text;

namespace Poc_Redis_Docker
{
    class TipoKey
    {
        public IEnumerable<RedisKey> ListaChave(string mascara)
        {
            var endpoint = RedisConexao.Connection.GetEndPoints();
            var server = RedisConexao.Connection.GetServer(endpoint[0]);
            /*foreach (var key in server.Keys(pattern: mascara+"*"))
            {
                Console.WriteLine(key);
            }*/

            var chaves = server.Keys(database: 0, pattern: mascara + "*");
            return chaves;
        }

        public bool ExcluiChave(string key)
        {
            var cache = RedisConexao.Connection.GetDatabase();
            return cache.KeyDelete(key);
        }


    }
}
