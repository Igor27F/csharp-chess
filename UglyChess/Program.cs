// See https://aka.ms/new-console-template for more information
using tabuleiro;
using UglyChess;
using xadrez;

try {
    PartidaDeXadrez partida = new PartidaDeXadrez();
    while (!partida.terminada) {

        try {
            Console.Clear();
            Tela.imprimirPartida(partida);

            Console.Write("\nOrigem: ");
            Posicao origem = Tela.lerPosicaoXadrez().posicaoConverter();
            partida.validarPosicaoDeOrigem(origem);

            bool[,] posicoesPossiveis = partida.tabuleiro.peca(origem).movimentosPossiveis();

            Console.Clear();
            Tela.imprimirTabuleiro(partida.tabuleiro, posicoesPossiveis);

            Console.Write("\nDestino: ");
            Posicao destino = Tela.lerPosicaoXadrez().posicaoConverter();
            partida.validarPosicaoDeDestino(origem, destino);

            partida.realizaJogada(origem, destino);
        }
        catch (TabuleiroException ex) {
            Console.WriteLine(ex.Message);
            Console.ReadKey();
        }
    }
    Console.Clear();
    Tela.imprimirPartida(partida);
}
catch (TabuleiroException ex) {
    Console.WriteLine(ex.Message);
}


Console.ReadLine();
