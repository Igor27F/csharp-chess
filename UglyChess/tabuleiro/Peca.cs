﻿namespace tabuleiro {
    abstract class Peca {
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qtdMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        public Peca(Cor cor, Tabuleiro tab) {
            this.posicao = null;
            this.cor = cor;
            this.qtdMovimentos = 0;
            this.tab = tab;
        }
        public abstract bool[,] movimentosPossiveis();
        public void incrementarQtdMovimentos() {
            qtdMovimentos++;
        }
        public void decrementarQtdMovimentos() {
            qtdMovimentos--;
        }
        public bool existeMovimentosPossiveis() {
            bool[,] mat = movimentosPossiveis();
            for(int i=0;i<tab.linhas;i++){
                for(int j=0; j < tab.colunas; j++) {
                    if (mat[i, j]) {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool movimentoPossivel(Posicao posicao) {
            return movimentosPossiveis()[posicao.linha, posicao.coluna];
        }
    }
}