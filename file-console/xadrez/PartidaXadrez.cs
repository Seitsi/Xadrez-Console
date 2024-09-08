using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    public class PartidaXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        public bool Xeque { get; private set; }
        private HashSet<Peca> Pecas { get; set; }
        private HashSet<Peca> PecasCapturadas { get; set; }
        public Peca EnPassant { get;  private set; }

        public PartidaXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            Pecas = new HashSet<Peca>();
            PecasCapturadas = new HashSet<Peca>();
            Terminada = false;
            Xeque = false;
            EnPassant = null;
            ColocarPeca();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                PecasCapturadas.Add(pecaCapturada);
            }

            //roque pequeno
            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca torre = Tab.RetirarPeca(origemT);
                torre.IncrementarQtdMovimentos();
                Tab.ColocarPeca(torre, destinoT);
            }

            //roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca torre = Tab.RetirarPeca(origemT);
                torre.IncrementarQtdMovimentos();
                Tab.ColocarPeca(torre, destinoT);
            }

            //en passant
            if (p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if(p.Color == Cor.Branco)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = Tab.RetirarPeca(posP);
                    PecasCapturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            Peca p = Tab.Peca(destino);
            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você se colocou em xeque, movimento desfeito!");
            }

            //promoção
            if(p is Peao)
            {
                if(p.Color == Cor.Branco && destino.Linha == 0 || p.Color == Cor.Preto && destino.Linha == 7)
                {
                    p = Tab.RetirarPeca(destino);
                    Pecas.Remove(p);
                    Peca dama = new Dama(p.Color, Tab);
                    Tab.ColocarPeca(dama, destino);
                    Pecas.Add(dama);
                    //commit
                }
            }

            if (EstaEmXeque(CorAdversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (XequeMate(CorAdversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }

            //en passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                EnPassant = p;
            }
            else
            {
                EnPassant = null;
            }

          
        }

        public bool XequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach(Peca x in PecasEmJogoCor(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for(int i = 0; i < Tab.Linhas; i++)
                {
                    for(int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }

        private void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQtdMovimentos();

            if(pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                PecasCapturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);

            //roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca torre = Tab.RetirarPeca(destinoT);
                torre.DecrementarQtdMovimentos();
                Tab.ColocarPeca(torre, origemT);
            }

            //roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca torre = Tab.RetirarPeca(destinoT);
                torre.DecrementarQtdMovimentos();
                Tab.ColocarPeca(torre, origemT);
            }

            //en passant 
            if(p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == EnPassant)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if (p.Color == Cor.Branco)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tab.ColocarPeca(peao, posP);
                }
                
            }
        }

        public void ValidaPosicaoOrigem(Posicao origem)
        {
            if (Tab.Peca(origem) == null)
            {
                throw new TabuleiroException("Não existe peça na posição indicada!");
            }

            if (JogadorAtual != Tab.Peca(origem).Color)
            {
                throw new TabuleiroException("Nao é sua vez de jogar! Turno do jogador: " + JogadorAtual);
            }

            if (!Tab.Peca(origem).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existe movimento possível para peça escolhida!");
            }
        }

        public void ValidaPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.Peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("A posição de destino é inválida. Insira uma nova posição!");
            }
        }
        public void MudaJogador()
        {
            if (JogadorAtual == Cor.Branco)
            {
                JogadorAtual = Cor.Preto;
            }
            else
            {
                JogadorAtual = Cor.Branco;
            }
        }

        public HashSet<Peca> PecasCapturadasCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in PecasCapturadas)
            {
                if (x.Color == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogoCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Color == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadasCor(cor));
            return aux;
        }

        private Cor CorAdversaria(Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branco;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogoCor(cor))
            {
                if(x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca rei = Rei(cor);
            if (rei == null)
            {
                throw new TabuleiroException("Não possui rei da "+ cor +" no tabuleiro!" );
            }

            foreach(Peca x in PecasEmJogoCor(CorAdversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[rei.Posicao.Linha, rei.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }
        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }
        private void ColocarPeca()
        {
            ColocarNovaPeca('a', 1, new Torre(Cor.Branco, Tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branco, Tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branco, Tab));
            ColocarNovaPeca('d', 1, new Dama(Cor.Branco, Tab));
            ColocarNovaPeca('e', 1, new Rei(Cor.Branco, Tab, this));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branco, Tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branco, Tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branco, Tab));

            ColocarNovaPeca('a', 8, new Torre(Cor.Preto, Tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preto, Tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preto, Tab));
            ColocarNovaPeca('d', 8, new Dama(Cor.Preto, Tab));
            ColocarNovaPeca('e', 8, new Rei(Cor.Preto, Tab, this));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preto, Tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preto, Tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preto, Tab));

            ColocarNovaPeca('a', 2, new Peao(Cor.Branco, Tab, this));
            ColocarNovaPeca('b', 2, new Peao(Cor.Branco, Tab, this));
            ColocarNovaPeca('c', 2, new Peao(Cor.Branco, Tab, this));
            ColocarNovaPeca('d', 2, new Peao(Cor.Branco, Tab, this));
            ColocarNovaPeca('e', 2, new Peao(Cor.Branco, Tab, this));
            ColocarNovaPeca('f', 2, new Peao(Cor.Branco, Tab, this));
            ColocarNovaPeca('g', 2, new Peao(Cor.Branco, Tab, this));
            ColocarNovaPeca('h', 2, new Peao(Cor.Branco, Tab, this));

            ColocarNovaPeca('a', 7, new Peao(Cor.Preto, Tab, this));
            ColocarNovaPeca('b', 7, new Peao(Cor.Preto, Tab, this));
            ColocarNovaPeca('c', 7, new Peao(Cor.Preto, Tab, this));
            ColocarNovaPeca('d', 7, new Peao(Cor.Preto, Tab, this));
            ColocarNovaPeca('e', 7, new Peao(Cor.Preto, Tab, this));
            ColocarNovaPeca('f', 7, new Peao(Cor.Preto, Tab, this));
            ColocarNovaPeca('g', 7, new Peao(Cor.Preto, Tab, this));
            ColocarNovaPeca('h', 7, new Peao(Cor.Preto, Tab, this));

        }
    }
}
