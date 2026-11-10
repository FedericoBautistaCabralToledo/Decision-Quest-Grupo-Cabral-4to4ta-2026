namespace JuegoDecisionQuest.Objetos.MecanicasPelea.Enemigo
{
    public class EstadisticasEnemigo
    {
        public int Vida { get; private set; }
        public int VidaMaxima { get; private set; }
        public int Fase { get; private set; }

        private int rondasFase;

        public EstadisticasEnemigo()
        {
            Vida = 300;
            VidaMaxima = 300;
            Fase = 1;
            rondasFase = 0;
        }

        public void RecibirDanio(int cantidad)
        {
            Vida -= cantidad;

            if (Vida < 0)
                Vida = 0;
        }

        public void CompletarRonda()
        {
            rondasFase++;

            if (rondasFase >= 2 && Fase < 3)
            {
                Fase++;
                rondasFase = 0;
            }
        }

        public bool EstaVivo()
        {
            return Vida > 0;
        }

        public bool EstaDebilitado()
        {
            return Vida <= 30;
        }
    }
}