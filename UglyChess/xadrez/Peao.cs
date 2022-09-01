using tabuleiro;

namespace xadrez {
    class Peao : Peca {
        private PartidaDeXadrez partida;
        public Peao(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(cor, tab) {
            this.partida = partida;
        }

        public override string ToString() {
            return "P";
        }
        private bool existeInimigo(Posicao pos) {
            Peca p = tab.peca(pos);
            return p != null && p.cor != cor;
        }
        private bool livre(Posicao pos) {
            return tab.peca(pos) == null;
        }

        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];
            Posicao pos = new Posicao(0, 0);

            int cima = cor == Cor.Branca ? -1 : 1;

            pos.definirValores(posicao.linha + cima, posicao.coluna);
            if (tab.posicaoValida(pos) && livre(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(posicao.linha + cima * 2, posicao.coluna);
            if (tab.posicaoValida(pos) && livre(pos) && qtdMovimentos == 0) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(posicao.linha + cima, posicao.coluna - 1);
            Posicao esquerda = new Posicao(posicao.linha, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && (existeInimigo(pos) || 
                (posicao.linha == (cor == Cor.Branca ? 3 : 4) && 
                existeInimigo(esquerda) && tab.peca(esquerda) == partida.vulneravelEnPassant))) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(posicao.linha + cima, posicao.coluna + 1);
            Posicao direita = new Posicao(posicao.linha, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && (existeInimigo(pos) || 
                (posicao.linha == (cor == Cor.Branca ? 3 : 4) && 
                existeInimigo(direita) && tab.peca(direita) == partida.vulneravelEnPassant))) {
                mat[pos.linha, pos.coluna] = true;
            }

            return mat;
        }
    }
}
