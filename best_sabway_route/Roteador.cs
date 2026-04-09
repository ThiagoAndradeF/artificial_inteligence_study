using System;
using System.Collections.Generic;

namespace AStarMetro
{
    // ─────────────────────────────────────────────────────────────────────
    // Grafo da rede de metrô
    // ─────────────────────────────────────────────────────────────────────
    class RedeMetro
    {
        private readonly Dictionary<Estacao, List<Conexao>> _adj = new();
        public  readonly List<Estacao> Estacoes = new();

        public void AdicionarEstacao(Estacao e)
        {
            _adj[e] = new List<Conexao>();
            Estacoes.Add(e);
        }

        public void AdicionarConexao(Estacao a, Estacao b, double custo)
        {
            _adj[a].Add(new Conexao(b, custo));
            _adj[b].Add(new Conexao(a, custo));
        }

        public IEnumerable<Conexao> Vizinhos(Estacao e) => _adj[e];
    }

    // ─────────────────────────────────────────────────────────────────────
    // Algoritmo A*
    // ─────────────────────────────────────────────────────────────────────
    class AlgoritmoAStar
    {
        private readonly RedeMetro _rede;
        private const    double    FatorVelocidade = 0.8;

        public AlgoritmoAStar(RedeMetro rede) { _rede = rede; }

        // h(n) = distância euclidiana x fator → admissível (nunca superestima)
        private double H(Estacao atual, Estacao destino)
        {
            double dx = atual.X - destino.X;
            double dy = atual.Y - destino.Y;
            return Math.Sqrt(dx * dx + dy * dy) * FatorVelocidade;
        }

        public (List<Estacao>? caminho, double custo) Buscar(Estacao inicio, Estacao destino)
        {
            var abertos = new SortedSet<No>(Comparer<No>.Create((a, b) =>
            {
                int cmp = a.CompareTo(b);
                return cmp != 0 ? cmp : a.GetHashCode().CompareTo(b.GetHashCode());
            }));

            var melhorG  = new Dictionary<Estacao, double>();
            var fechados = new HashSet<Estacao>();

            abertos.Add(new No(inicio, 0, H(inicio, destino), null));
            melhorG[inicio] = 0;

            while (abertos.Count > 0)
            {
                var atual = abertos.Min!;
                abertos.Remove(atual);

                if (fechados.Contains(atual.Estacao)) continue;
                fechados.Add(atual.Estacao);

                if (atual.Estacao == destino)
                    return (ReconstruirCaminho(atual), atual.G);

                foreach (var con in _rede.Vizinhos(atual.Estacao))
                {
                    if (fechados.Contains(con.Destino)) continue;
                    double novoG = atual.G + con.Custo;
                    if (!melhorG.TryGetValue(con.Destino, out double ant) || novoG < ant)
                    {
                        melhorG[con.Destino] = novoG;
                        abertos.Add(new No(con.Destino, novoG, H(con.Destino, destino), atual));
                    }
                }
            }

            return (null, double.MaxValue);
        }

        private static List<Estacao> ReconstruirCaminho(No no)
        {
            var c = new List<Estacao>();
            for (var n = (No?)no; n != null; n = n.Pai) c.Add(n.Estacao);
            c.Reverse();
            return c;
        }
    }
}
