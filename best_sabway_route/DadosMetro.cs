using System;
using System.Collections.Generic;

namespace AStarMetro
{
    // ─────────────────────────────────────────────────────────────────────
    // Dados mockados da rede de metrô
    // ─────────────────────────────────────────────────────────────────────
    static class DadosMetro
    {
        public static (RedeMetro rede, Dictionary<int, Estacao> mapa, Dictionary<(int, int), double> custos) CriarRede()
        {
            // ── Definição das estações (id, nome, x, y) ───────────────
            var defs = new (int id, string nome, double x, double y)[]
            {
                (1, "Central",   3.5, 9.5),
                (2, "Norte",     5.5, 9.8),
                (3, "Leste",     8.0, 9.0),
                (4, "Oeste",     1.0, 6.5),
                (5, "Sul",       4.0, 6.5),
                (6, "Sudeste",   6.5, 5.0),
                (7, "Nordeste",  8.0, 5.0),
                (8, "Vila Nova", 1.0, 9.0),
                (9, "Parque",    2.5, 3.5),
            };

            // ── Definição das conexões (idA, idB, minutos) ────────────
            var conDefs = new (int a, int b, double custo)[]
            {
                (8, 1, 4),
                (1, 2, 5),
                (2, 3, 6),
                (8, 4, 5),
                (1, 5, 7),
                (4, 5, 6),
                (2, 5, 8),
                (5, 6, 5),
                (4, 9, 9),
                (9, 6, 7),
                (6, 3, 8),
                (3, 7, 6),
                (6, 7, 4),
            };

            // ── Monta a rede ──────────────────────────────────────────
            var rede = new RedeMetro();
            var mapa = new Dictionary<int, Estacao>();

            foreach (var (id, nome, x, y) in defs)
            {
                var est = new Estacao(id, nome, x, y);
                rede.AdicionarEstacao(est);
                mapa[id] = est;
            }

            var custos = new Dictionary<(int, int), double>();
            foreach (var (a, b, custo) in conDefs)
            {
                rede.AdicionarConexao(mapa[a], mapa[b], custo);
                custos[(Math.Min(a, b), Math.Max(a, b))] = custo;
            }

            return (rede, mapa, custos);
        }
    }
}
