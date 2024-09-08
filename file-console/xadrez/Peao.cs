using tabuleiro;

namespace xadrez
{
    public class Peao : Peca
    {
        private PartidaXadrez Partida;
        public Peao(Cor color, Tabuleiro tab, PartidaXadrez partida) : base(color, tab)
        {
            Partida = partida;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.Peca(pos);
            return p == null || p.Color != Color;
        }

        private bool ExisteInimigo(Posicao pos)
        {
            Peca p = Tab.Peca(pos);
            return p != null && p.Color != Color;  
        }

        private bool Livre(Posicao pos)
        {
            return Tab.Peca(pos) == null;
        }
        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Colunas, Tab.Linhas];
            Posicao pos = new Posicao(0, 0);

            if(Color == Cor.Branco)
            {
                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos) && QtdMovimentos == 0)
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //em passant
                if(Posicao.Linha == 3)
                {
                    Posicao esq = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if(Tab.PosicaoValida(esq) && ExisteInimigo(esq) && Tab.Peca(esq) == Partida.EnPassant)
                    {
                        mat[esq.Linha - 1, esq.Coluna] = true;
                    }
                    Posicao dir = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(dir) && ExisteInimigo(dir) && Tab.Peca(dir) == Partida.EnPassant)
                    {
                        mat[dir.Linha - 1, dir.Coluna] = true;
                    }
                }
            }
            else
            {
                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
                if (Tab.PosicaoValida(pos) && Livre(pos) && QtdMovimentos == 0)
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                {
                    mat[pos.Linha, pos.Coluna] = true;
                }

                //em passant
                if (Posicao.Linha == 4)
                {
                    Posicao esq = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    if (Tab.PosicaoValida(esq) && ExisteInimigo(esq) && Tab.Peca(esq) == Partida.EnPassant)
                    {
                        mat[esq.Linha + 1, esq.Coluna] = true;
                    }
                    Posicao dir = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(dir) && ExisteInimigo(dir) && Tab.Peca(dir) == Partida.EnPassant)
                    {
                        mat[dir.Linha + 1, dir.Coluna] = true;
                    }
                }
            }


            return mat;
        }
    }
}