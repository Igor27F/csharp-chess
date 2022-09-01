using tabuleiro;

namespace xadrez {
    class PartidaDeXadrez {
        public Tabuleiro tabuleiro { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaDeXadrez() {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            arrumarTabuleiro();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino) {
            Peca p = tabuleiro.retirarPeca(origem);
            p.incrementarQtdMovimentos();
            Peca pecaCapturada = tabuleiro.retirarPeca(destino);
            tabuleiro.colocarPeca(p, destino);

            if (p is Rei) {
                if (destino.coluna == origem.coluna + 2) {
                    Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                    Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                    Peca T = tabuleiro.retirarPeca(origemT);
                    T.incrementarQtdMovimentos();
                    tabuleiro.colocarPeca(T, destinoT);
                }
                else if (destino.coluna == origem.coluna - 2) {
                    Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                    Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                    Peca T = tabuleiro.retirarPeca(origemT);
                    T.incrementarQtdMovimentos();
                    tabuleiro.colocarPeca(T, destinoT);
                }
            }
            if (p is Peao && origem.coluna != destino.coluna && pecaCapturada == null) {
                pecaCapturada = tabuleiro.retirarPeca(new Posicao(destino.linha + (p.cor == Cor.Branca ? 1 : -1), destino.coluna));
            }

            if (pecaCapturada != null) {
                capturadas.Add(pecaCapturada);
            }

            return pecaCapturada;
        }
        private void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = tabuleiro.retirarPeca(destino);
            p.decrementarQtdMovimentos();
            if (pecaCapturada != null) {
                tabuleiro.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tabuleiro.colocarPeca(p, origem);

            if (p is Rei) {
                if (destino.coluna == origem.coluna + 2) {
                    Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                    Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                    Peca T = tabuleiro.retirarPeca(destinoT);
                    T.decrementarQtdMovimentos();
                    tabuleiro.colocarPeca(T, origemT);
                }
                else if (destino.coluna == origem.coluna - 2) {
                    Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                    Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                    Peca T = tabuleiro.retirarPeca(destinoT);
                    T.decrementarQtdMovimentos();
                    tabuleiro.colocarPeca(T, origemT);
                }
            }
            if (p is Peao && origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant) {
                Peca peao = tabuleiro.retirarPeca(destino);
                tabuleiro.colocarPeca(peao, new Posicao(destino.linha + (p.cor == Cor.Branca ? 1 : -1), destino.coluna));
            }
        }
        public void realizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = executaMovimento(origem, destino);
            if (estaEmXeque(jogadorAtual)) {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Voce nao pode se colocar em xeque");
            }
            xeque = estaEmXeque(adversaria(jogadorAtual));

            if (testeXequeMate(adversaria(jogadorAtual))) {
                terminada = true;
            }
            else {
                turno++;
                mudaJogador();
            }

            Peca p = tabuleiro.peca(destino);

            if (p is Peao) {
                if (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2) {
                    vulneravelEnPassant = p;
                }
                else {
                    vulneravelEnPassant = null;
                }
                if (destino.linha == (p.cor == Cor.Branca ? 0 : 7 )) {
                    p = tabuleiro.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(tabuleiro, p.cor);
                    tabuleiro.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }
        }
        public void validarPosicaoDeOrigem(Posicao posicao) {
            if (tabuleiro.peca(posicao) == null) {
                throw new TabuleiroException("Nao existe peca na posicao escolhida");
            }
            if (jogadorAtual != tabuleiro.peca(posicao).cor) {
                throw new TabuleiroException("É a vez das " + (jogadorAtual == Cor.Branca ? "brancas" : "pretas") + "!!!");
            }
            if (!tabuleiro.peca(posicao).existeMovimentosPossiveis()) {
                throw new TabuleiroException("Nao existem movimentos possiveis para essa peca");
            }
        }
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino) {
            if (!tabuleiro.peca(origem).movimentoPossivel(destino)) {
                throw new TabuleiroException("Posicao de destino invalida");
            }
        }
        public void mudaJogador() {
            jogadorAtual = jogadorAtual == Cor.Branca ? Cor.Preta : Cor.Branca;
        }
        public HashSet<Peca> pecasCapturadas(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in capturadas) {
                if (peca.cor == cor) {
                    aux.Add(peca);
                }
            }
            return aux;
        }
        public HashSet<Peca> pecasEmJogo(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in pecas) {
                if (peca.cor == cor) {
                    aux.Add(peca);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }
        private Cor adversaria(Cor cor) {
            return (cor == Cor.Branca ? Cor.Preta : Cor.Branca);
        }
        public bool testeXequeMate(Cor cor) {
            if (!estaEmXeque(cor)) {
                return false;
            }
            foreach (Peca p in pecasEmJogo(cor)) {
                bool[,] mat = p.movimentosPossiveis();
                for (int i = 0; i < tabuleiro.linhas; i++) {
                    for (int j = 0; j < tabuleiro.colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = p.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void colocarNovaPeca(char coluna, int linha, Peca peca) {
            tabuleiro.colocarPeca(peca, new PosicaoXadrez(coluna, linha).posicaoConverter());
            pecas.Add(peca);
        }
        private Peca rei(Cor cor) {
            foreach (Peca peca in pecasEmJogo(cor)) {
                if (peca is Rei) {
                    return peca;
                }
            }
            return null;
        }
        public bool estaEmXeque(Cor cor) {
            Peca R = rei(cor);
            if (R == null) {
                throw new TabuleiroException("Nao tem rei da cor " + cor);
            }
            foreach (Peca peca in pecasEmJogo(adversaria(cor))) {
                bool[,] mat = peca.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna]) {
                    return true;
                }
            }
            return false;
        }
        private void colocarPeca(Cor cor, int linha, bool peao) {
            Peca peca;
            if (peao) {
                for (int i = 0; i < 8; i++) {
                    peca = new Peao(tabuleiro, cor, this);
                    colocarNovaPeca((char)('A' + i), linha, peca);
                }
            }
            else {
                for (int i = 0; i < 8; i++) {
                    switch (i) {
                        case 0:
                        case 7:
                            peca = new Torre(tabuleiro, cor);
                            break;
                        case 1:
                        case 6:
                            peca = new Cavalo(tabuleiro, cor);
                            break;
                        case 2:
                        case 5:
                            peca = new Bispo(tabuleiro, cor);
                            break;
                        case 3:
                            peca = new Dama(tabuleiro, cor);
                            break;
                        case 4:
                            peca = new Rei(tabuleiro, cor, this);
                            break;
                        default:
                            peca = new Peao(tabuleiro, cor, this);
                            break;
                    }
                    colocarNovaPeca((char)('A' + i), linha, peca);
                }
            }
        }
        private void arrumarTabuleiro() {
            colocarPeca(Cor.Preta, 8, false);
            colocarPeca(Cor.Preta, 7, true);
            colocarPeca(Cor.Branca, 2, true);
            colocarPeca(Cor.Branca, 1, false);
        }
    }
}
