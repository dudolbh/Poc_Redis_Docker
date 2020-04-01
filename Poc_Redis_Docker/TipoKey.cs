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

            var chaves = server.Keys(pattern: mascara + "*");
            return chaves;
        }

    }
}
