using System;
using System.Net.WebSockets;
using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    public class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    ImprimirPeca(tab.Peca(i, j));
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] movimentosPossiveis)
        {
            ConsoleColor fundoPadrao = Console.BackgroundColor;
            ConsoleColor fundoDestaque = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (movimentosPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoDestaque;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoPadrao;
                    }
                    ImprimirPeca(tab.Peca(i, j));
                    Console.Write(" ");
                    Console.BackgroundColor = fundoPadrao;
                }
                Console.WriteLine();
            }
            Console.Write("  a b c d e f g h");
            Console.BackgroundColor = fundoPadrao;
        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write("-");
            }
            else
            {

                if (peca.Color == Cor.Branco)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write("");
            }
        }

        public static void ImprimirPartida(PartidaXadrez partida)
        {
            ImprimirTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.Turno);
            if (!partida.Terminada)
            {
                Console.WriteLine("Aguardando jogada: " + partida.JogadorAtual);
                if (partida.Xeque)
                {
                    Console.WriteLine("XEQUE!");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Xeque Mate! Parabéns Jogador " + partida.JogadorAtual + "!!!");
            }



        }

        public static void ImprimirPecasCapturadas(PartidaXadrez partida)
        {
            ConsoleColor aux = Console.ForegroundColor;

            Console.WriteLine("Peças capturadas");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadasCor(Cor.Branco));
            Console.WriteLine();

            Console.Write("Pretas: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            ImprimirConjunto(partida.PecasCapturadasCor(Cor.Preto));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> pecasCapturadas)
        {
            Console.Write("[");
            foreach (Peca x in pecasCapturadas)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }
        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }
    }
}
