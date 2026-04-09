using System;

namespace AStarMetro
{
    // ─────────────────────────────────────────────────────────────────────
    // Estação de metrô com coordenadas geográficas (usadas na heurística)
    // ─────────────────────────────────────────────────────────────────────
    class Estacao
    {
        public int    Id   { get; }
        public string Nome { get; }
        public double X    { get; }
        public double Y    { get; }

        public Estacao(int id, string nome, double x, double y)
        {
            Id   = id;
            Nome = nome;
            X    = x;
            Y    = y;
        }

        public override string ToString() => Nome;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Conexão entre duas estações com custo em minutos
    // ─────────────────────────────────────────────────────────────────────
    class Conexao
    {
        public Estacao Destino { get; }
        public double  Custo   { get; }

        public Conexao(Estacao destino, double custo)
        {
            Destino = destino;
            Custo   = custo;
        }
    }

    // ─────────────────────────────────────────────────────────────────────
    // Nó interno do A*
    // ─────────────────────────────────────────────────────────────────────
    class No : IComparable<No>
    {
        public Estacao Estacao { get; }
        public double  G       { get; }   // custo real acumulado
        public double  H       { get; }   // estimativa heurística
        public double  F       => G + H;  // custo total estimado
        public No?     Pai     { get; }

        public No(Estacao estacao, double g, double h, No? pai)
        {
            Estacao = estacao;
            G       = g;
            H       = h;
            Pai     = pai;
        }

        public int CompareTo(No? outro)
        {
            if (outro is null) return 1;
            int cmp = F.CompareTo(outro.F);
            return cmp != 0 ? cmp : H.CompareTo(outro.H);
        }
    }
}
