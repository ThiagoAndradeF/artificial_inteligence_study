/*
 * A* Metro — Aplicativo de Console Interativo
 * =============================================
 * Encontra a rota mais rápida entre estações de metrô usando o algoritmo A*.
 *
 * Arquivos do projeto:
 *   - Models.cs     → Estacao, Conexao, No (modelos de dados)
 *   - UI.cs         → Interface de console (cores, layout, entrada)
 *   - Roteador.cs   → RedeMetro (grafo) e AlgoritmoAStar (busca A*)
 *   - DadosMetro.cs → Dados mockados da rede (estações e conexões)
 *   - Program.cs    → Ponto de entrada (este arquivo)
 */

using System;

namespace AStarMetro
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var (rede, mapa, custos) = DadosMetro.CriarRede();
            var astar = new AlgoritmoAStar(rede);
            int total = rede.Estacoes.Count;

            // ── Loop principal ────────────────────────────────────────
            do
            {
                UI.Cabecalho();
                UI.MapaEstacoes(rede.Estacoes, custos);

                int idO = UI.PedirEstacao("Estacao de PARTIDA ", total);
                int idD = UI.PedirEstacao("Estacao de DESTINO  ", total);

                if (idO == idD)
                {
                    UI.Aviso("Partida e destino sao a mesma estacao!");
                    Console.WriteLine("  Pressione qualquer tecla para continuar...");
                    Console.ReadKey(true);
                    continue;
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  Calculando rota com A*...");
                Console.ResetColor();

                var (caminho, custo) = astar.Buscar(mapa[idO], mapa[idD]);

                if (caminho is null)
                    UI.Erro("Nao foi possivel encontrar uma rota entre essas estacoes.");
                else
                    UI.Resultado(caminho, custo, custos);

            } while (UI.PerguntarNovaBusca());

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Encerrando. Boa viagem!\n");
            Console.ResetColor();
        }
    }
}