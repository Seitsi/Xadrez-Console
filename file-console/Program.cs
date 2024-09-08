using tabuleiro;
using xadrez;

try
{
 
    
    PartidaXadrez partida = new PartidaXadrez();

    while (!partida.Terminada)
    {
        try
        {
            Console.Clear();
            Tela.ImprimirPartida(partida);

            Console.WriteLine();
            Console.Write("Origem: ");
            Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();

            partida.ValidaPosicaoOrigem(origem);
            bool[,] movimentosPossiveis = partida.Tab.Peca(origem).MovimentosPossiveis();

            Console.Clear();
            Tela.ImprimirTabuleiro(partida.Tab, movimentosPossiveis);
            Console.WriteLine();


            Console.WriteLine();
            Console.Write("Destino: ");
            Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
            partida.ValidaPosicaoDestino(origem, destino);

            partida.RealizaJogada(origem, destino);
        }
        catch (TabuleiroException e)
        {
            Console.WriteLine(e.Message);
            Console.ReadLine();
        }
    }
    Console.Clear();
    Tela.ImprimirPartida(partida);
    Console.WriteLine();
}
catch (TabuleiroException e)
{
    Console.WriteLine(e.Message);
}

