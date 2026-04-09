using System;
using System.Collections.Generic;
using System.Linq;

namespace AStarMetro
{
    // ─────────────────────────────────────────────────────────────────────
    // Interface de console — cores e layout
    // ─────────────────────────────────────────────────────────────────────
    static class UI
    {
        public static void Cabecalho()
        {
            Console.Clear();
            Cor(ConsoleColor.Cyan);
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║           METRO NAVIGATOR  —  Algoritmo A*           ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Reset();
            Console.WriteLine();
        }

        public static void MapaEstacoes(List<Estacao> estacoes,
                                        Dictionary<(int, int), double> conexoes)
        {
            Cor(ConsoleColor.Yellow);
            Console.WriteLine("  ESTACOES DA REDE");
            Console.WriteLine("  " + new string('─', 54));
            Reset();

            for (int i = 0; i < estacoes.Count; i += 2)
            {
                Cor(ConsoleColor.Green);
                Console.Write($"  [{estacoes[i].Id,2}] ");
                Cor(ConsoleColor.White);
                Console.Write($"{estacoes[i].Nome,-20}");

                if (i + 1 < estacoes.Count)
                {
                    Cor(ConsoleColor.Green);
                    Console.Write($"   [{estacoes[i + 1].Id,2}] ");
                    Cor(ConsoleColor.White);
                    Console.Write($"{estacoes[i + 1].Nome}");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Cor(ConsoleColor.Yellow);
            Console.WriteLine("  CONEXOES (minutos)");
            Console.WriteLine("  " + new string('─', 54));
            Reset();

            foreach (var kv in conexoes)
            {
                var (idA, idB) = kv.Key;
                var a = estacoes.First(e => e.Id == idA);
                var b = estacoes.First(e => e.Id == idB);
                Cor(ConsoleColor.DarkGray);
                Console.WriteLine($"  {a.Nome,-18} <-> {b.Nome,-18} {kv.Value,3:F0} min");
            }

            Reset();
            Console.WriteLine();
        }

        public static void Resultado(List<Estacao> caminho, double custo,
                                     Dictionary<(int, int), double> conexoes)
        {
            Console.WriteLine();
            Cor(ConsoleColor.Green);
            Console.WriteLine("  ╔══════════════════════════════════════════════════╗");
            Console.WriteLine("  ║               ROTA ENCONTRADA                   ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════╝");
            Reset();
            Console.WriteLine();

            double acumulado = 0;
            for (int i = 0; i < caminho.Count - 1; i++)
            {
                var orig = caminho[i];
                var dest = caminho[i + 1];
                int ka   = Math.Min(orig.Id, dest.Id);
                int kb   = Math.Max(orig.Id, dest.Id);
                double t = conexoes.TryGetValue((ka, kb), out double cv) ? cv : 0;
                acumulado += t;

                Cor(ConsoleColor.DarkCyan);
                Console.Write($"  {i + 1,2}. ");
                Cor(ConsoleColor.White);
                Console.Write($"{orig.Nome,-18}");
                Cor(ConsoleColor.Yellow);
                Console.Write(" --> ");
                Cor(ConsoleColor.White);
                Console.Write($"{dest.Nome,-18}");
                Cor(ConsoleColor.DarkGray);
                Console.WriteLine($"  +{t,2:F0} min  (total: {acumulado:F0} min)");
            }

            Console.WriteLine();
            Cor(ConsoleColor.Cyan);
            Console.WriteLine($"  Partida  : {caminho.First().Nome}");
            Console.WriteLine($"  Chegada  : {caminho.Last().Nome}");
            Console.WriteLine();
            Cor(ConsoleColor.Yellow);
            int paradas = caminho.Count - 2;
            Console.WriteLine($"  Tempo estimado  : {custo:F0} minutos");
            Cor(ConsoleColor.White);
            Console.WriteLine($"  Paradas         : {(paradas > 0 ? paradas.ToString() : "nenhuma")} intermediaria(s)");
            Console.WriteLine($"  Trechos         : {caminho.Count - 1}");
            Reset();
            Console.WriteLine();
        }

        public static void Erro(string msg)
        {
            Cor(ConsoleColor.Red);
            Console.WriteLine($"\n  ! {msg}");
            Reset();
        }

        public static void Aviso(string msg)
        {
            Cor(ConsoleColor.Yellow);
            Console.WriteLine($"\n  ! {msg}");
            Reset();
        }

        public static int PedirEstacao(string label, int max)
        {
            while (true)
            {
                Cor(ConsoleColor.White);
                Console.Write($"  {label} (1-{max}): ");
                Cor(ConsoleColor.Yellow);
                string? entrada = Console.ReadLine()?.Trim();
                Reset();

                if (int.TryParse(entrada, out int id) && id >= 1 && id <= max)
                    return id;

                Erro($"Digite um numero entre 1 e {max}.");
            }
        }

        public static bool PerguntarNovaBusca()
        {
            Console.WriteLine();
            Cor(ConsoleColor.DarkGray);
            Console.Write("  Nova busca? (s/n): ");
            Cor(ConsoleColor.Yellow);
            string? r = Console.ReadLine()?.Trim().ToLower();
            Reset();
            return r == "s" || r == "sim";
        }

        static void Cor(ConsoleColor c)  => Console.ForegroundColor = c;
        static void Reset()              => Console.ResetColor();
    }
}
