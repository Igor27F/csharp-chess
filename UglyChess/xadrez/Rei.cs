using tabuleiro;

namespace xadrez {
    class Rei : Peca {
        private PartidaDeXadrez partida;
        public Rei(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(cor, tab) {
            this.partida = partida;
        }

        public override string ToString() {
            return "R";
        }
        private bool podeMover(Posicao posicao) {
            Peca peca = tab.peca(posicao);
            return peca == null || peca.cor != cor;
        }
        private bool testeTorreParaRoque(Posicao posicao) {
            Peca p = tab.peca(posicao);
            return p != null && p is Torre && p.cor == cor && p.qtdMovimentos == 0;
        }
        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(posicao.linha, posicao.coluna);

            for(int i = -1; i < 2; i++) {
                for(int j = -1; j < 2; j++) {
                    pos.definirValores(posicao.linha + i, posicao.coluna + j);
                    if(tab.posicaoValida(pos) && podeMover(pos)) {
                        mat[pos.linha, pos.coluna] = true;
                    }
                }
            }

            if(qtdMovimentos == 0 && !partida.xeque) {
                Posicao posT1 = new Posicao(posicao.linha, posicao.coluna + 3);
                if (testeTorreParaRoque(posT1)) {
                    Posicao p1 = new Posicao(posicao.linha, posicao.coluna + 1);
                    Posicao p2 = new Posicao(posicao.linha, posicao.coluna + 2);
                    if (tab.peca(p1) == null && tab.peca(p2) == null) {
                        mat[pos.linha, pos.coluna + 2] = true;
                    }
                }
                Posicao posT2 = new Posicao(posicao.linha, posicao.coluna - 4);
                if (testeTorreParaRoque(posT2)) {
                    Posicao p1 = new Posicao(posicao.linha, posicao.coluna - 1);
                    Posicao p2 = new Posicao(posicao.linha, posicao.coluna - 2);
                    Posicao p3 = new Posicao(posicao.linha, posicao.coluna - 3);
                    if (tab.peca(p1) == null && tab.peca(p2) == null && tab.peca(p3) == null) {
                        mat[pos.linha, pos.coluna - 2] = true;
                    }
                }
            }

            return mat;
        }
    }
}
