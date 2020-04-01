using System;
using System.Collections.Generic;
using StackExchange.Redis;
using System.Threading.Tasks;
using System.Text;


namespace Poc_Redis_Docker
{
    class Program
    {


        static void Main(string[] args)
        {
            var random = new Random();
            var agencias = new List<string> { "ACBHZ", "ACPAM", "AAGRU", "AABHZ", "AAPOA" };
            var grupos = new List<string> { "C", "CS", "F", "FX", "G", "LX" };
            var eventos = new List<string> { "BT", "Carnaval 2020", "RS - PWA", "Carnaval 2020 Teste A/B" };

            //valor chave automatico
            //var agencia1 = new Agencia { agencia = "ACBHZ", grupo = "C", evento = "BT" };
            //var agencia2 = new Agencia { agencia = "ACBHZ", grupo = "F", evento = "BT" };

            var stopwatch0 = new System.Diagnostics.Stopwatch();
            var stopwatch1 = new System.Diagnostics.Stopwatch();
            var stopwatch2 = new System.Diagnostics.Stopwatch();
            var stopwatch3 = new System.Diagnostics.Stopwatch();
            var stopwatch4 = new System.Diagnostics.Stopwatch();
            var stopwatch5 = new System.Diagnostics.Stopwatch();

            // definindo o valor da chave
            //var agencia1 = new Agencia("ACBHZ","C","BT");
            //var agencia2 = new Agencia("ACBHZ", "F", "BT");

            List<Agencia> agenciaL = new List<Agencia>();

            for (var i = 0; i < 105; i++)
            {
                agenciaL.Add(new Agencia
                {
                    agencia = agencias[random.Next(agencias.Count)],
                    grupo = grupos[random.Next(grupos.Count)],
                    evento = eventos[random.Next(eventos.Count)]
                });
            }

            List<Agencia> agenciaL2 = new List<Agencia>();

            for (var i = 0; i < 1; i++)
            {
                agenciaL2.Add(new Agencia
                {
                    agencia = agencias[random.Next(agencias.Count)],
                    grupo = grupos[random.Next(grupos.Count)],
                    evento = eventos[random.Next(eventos.Count)]
                });
            }

            List<Agencia> agenciaL3 = new List<Agencia>();

            for (var i = 0; i < 105; i++)
            {
                agenciaL3.Add(new Agencia
                {
                    agencia = agencias[random.Next(agencias.Count)],
                    grupo = grupos[random.Next(grupos.Count)],
                    evento = eventos[random.Next(eventos.Count)]
                });
            }

            List<Agencia> agenciaL4 = new List<Agencia>();

            for (var i = 0; i < 105; i++)
            {
                agenciaL4.Add(new Agencia
                {
                    agencia = agencias[random.Next(agencias.Count)],
                    grupo = grupos[random.Next(grupos.Count)],
                    evento = eventos[random.Next(eventos.Count)]
                });
            }

            /*
            //Teste gravação e leitura do um arquivo unico hash:
            var redisdb = RedisConexao.Connection.GetDatabase();
            var agencia = agenciaL[0];
            stopwatch3.Start();
            redisdb.HashSet("AG:" + agencia.chave_cache, new HashEntry[] {
                    new HashEntry("agencia", agencia.agencia),
                    new HashEntry("grupo", agencia.grupo),
                    new HashEntry("evento", agencia.evento)}
                  );
            stopwatch3.Stop();

            stopwatch4.Start();
            var valorAgencia = redisdb.HashGet("AG:b5be5710-3b09-4c9f-9d95-2dd157614ba3", "agencia"); //Retorna agencia
            var valorColunas = redisdb.HashValues("AG:b5be5710-3b09-4c9f-9d95-2dd157614ba3");
            foreach (var val in valorColunas)
            {
                Console.WriteLine(val); //Retorna ACBHZ , LX, Carnaval 2020 Teste A/ B
            }
            stopwatch4.Stop();
            Console.WriteLine(valorAgencia);
            Console.WriteLine(valorColunas);
            */

            //Manipulação de Chaves.
            var tipoKey = new TipoKey();


            //Metodos Utilizando o tipo string
            var tipoString = new TipoString();

            //Gravação de dados string
            Console.WriteLine("Salvando dados no cache:");
            stopwatch0.Start();
            //GravaDado
            //tipoString.GravaDado(agenciaL);
            stopwatch0.Stop();
            Console.WriteLine("Sucesso.");

            //Gravação de dados string com expiracao
            var expiracao = new TimeSpan(0, 0, 30);
            Console.WriteLine("Salvando dados no cache, com expiração:");
            //GravaDadoExpira
            tipoString.GravaDadoExpira(agenciaL2, expiracao);
            Console.WriteLine("Sucesso.");


            //Gravação de dados string batch
            Console.WriteLine("Salvando dados no cache, batch:");
            stopwatch4.Start();
            //GravaDadoBatch
            tipoString.GravaDadoBatch(agenciaL3);
            stopwatch4.Stop();
            Console.WriteLine("Sucesso.");

            //Gravação de dados string batch Asyc
            Console.WriteLine("Salvando dados no cache, batch Async:");
            stopwatch5.Start();
            //GravaDadoBatch
            var taskConcluida = tipoString.GravaDadoBatchAsync(agenciaL4);
            stopwatch5.Stop();
            Console.WriteLine(taskConcluida);
            Console.WriteLine("Sucesso.");

            //Lista chaves com a mascara
            var mascara = "AG:";
            var chaves = tipoKey.ListaChave(mascara);

            //Leitura de dados string
            Console.WriteLine("Lendo dados do cache:");
            stopwatch1.Start();
            foreach (var chave in chaves)
            {
                //var dados = tipoString.LeituraDados(chave);
                //Console.WriteLine("key = {3}, agencia = {0}, grupo = {1}, evento = {2}", dados.agencia, dados.grupo, dados.evento, dados.chave_cache);
            }
            stopwatch1.Stop();

            //Leitura de um valor
            Console.WriteLine("Lendo 1 registro do cache:");
            var indice_rnd = random.Next(agencias.Count);
            var agencia1 = agenciaL3[indice_rnd];
            stopwatch2.Start();
            var valorString = tipoString.LeituraDados(agencia1.chave_cache.ToString());
            stopwatch2.Stop();
            Console.WriteLine("key = {3}, agencia = {0}, grupo = {1}, evento = {2}", valorString.agencia, valorString.grupo, valorString.evento, valorString.chave_cache);
            Console.WriteLine("Excluido registro {0} do cache:", agencia1.chave_cache);
            var exclui = tipoString.ExcluiDado(agencia1.chave_cache.ToString());
            Console.WriteLine(exclui);
            /*
            //Metodos utilizando o tipo hash
            var tipoHashSet = new TipoHashSet();

            //Gravação Cache
            stopwatch0.Start();
            Console.WriteLine("Salvando dados no cache");
            tipoHashSet.GravaDado(agenciaL);
            stopwatch0.Stop();

            //ListaChaves
            var tipoKey = tipoKey.ListaChave("AG:");

            //Leitura uma coluna
            stopwatch1.Start();
            var coluna = "agencia";
            Console.WriteLine("Lendo dados do cache, por coluna");
            foreach (var chave in chavesHash)
            {
                var retornoColuna = tipoHashSet.LeDadoColuna(chave, coluna);
                Console.WriteLine("key = {0}, {1} = {2}", chave, coluna, retornoColuna);
            }
            stopwatch1.Stop();


            //Leitura Todos os Dados.
            stopwatch2.Start();
            Console.WriteLine("Lendo dados do cache completo");
            foreach (var chave in chavesHash)
            {
                var retornoCompleto = tipoHashSet.LeDadoCompleto(chave);
                Console.WriteLine(retornoCompleto);
            }
            stopwatch2.Stop();


            //Conversão de Dicionario para string
            //var hash = program.LeDadoCompleto("AG:");
            //var dicionario = program.DictToString(hash.ToDictionary());

            */

            //Tempos de leitura e escrita
            //Console.WriteLine("Gravação: " + stopwatch0.ElapsedMilliseconds);
            //Console.WriteLine("Leitura coluna: "+stopwatch1.ElapsedMilliseconds);
            //Console.WriteLine("Leitura completa: "+stopwatch2.ElapsedMilliseconds);

            Console.WriteLine("\nGravação varios registros: " + stopwatch0.ElapsedMilliseconds);
            Console.WriteLine("Gravação varios registros, batch: " + stopwatch4.ElapsedMilliseconds);
            Console.WriteLine("Gravação varios registros, batch async: " + stopwatch5.ElapsedMilliseconds);
            Console.WriteLine("Leitura varios registros: " + stopwatch1.ElapsedMilliseconds);
            Console.WriteLine("Leitura 1 registro: " + stopwatch2.ElapsedMilliseconds);
        }

    }
}


