

namespace tabuleiro
{
    public abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Color { get; protected set; }
        public int QtdMovimentos { get; protected set; }

        public Tabuleiro Tab { get; protected set; }

        public Peca(Cor color, Tabuleiro tab)
        {
            Posicao = null;
            Color = color;
            Tab = tab;
            QtdMovimentos = 0;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for(int i = 0; i<Tab.Linhas; i++)
            {
                for (int j = 0; j<Tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }
        public abstract bool[,] MovimentosPossiveis();
        public void IncrementarQtdMovimentos()
        { 
            QtdMovimentos++; 
        }

        public void DecrementarQtdMovimentos()
        {
            QtdMovimentos--;
        }
    }
}
