using tabuleiro;

namespace xadrez {
    class PosicaoXadrez {
        public char coluna { get; set; }
        public int linha { get; set; }

        public PosicaoXadrez(char coluna, int linha) {
            this.coluna = char.ToUpper(coluna);
            this.linha = linha;
        }

        public Posicao posicaoConverter() {
            return new Posicao(8 - linha, coluna - 'A');
        }

        public override string ToString() {
            return "" + coluna + linha;
        }
    }
}
