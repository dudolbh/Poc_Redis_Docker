using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace Poc_Redis_Docker
{
    public class RedisConexao
    {
        static RedisConexao()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" }
            };


            // O ConnectionMultiplexer é designado para compartilhamento da conexao na aplicação inteira
            // Não necessitando de criar um instancia toda vez que precisar realizar uma operação.
            RedisConexao.conexao = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }

        private static readonly Lazy<ConnectionMultiplexer> conexao;
        
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return conexao.Value;
            }
        }

    }
}
