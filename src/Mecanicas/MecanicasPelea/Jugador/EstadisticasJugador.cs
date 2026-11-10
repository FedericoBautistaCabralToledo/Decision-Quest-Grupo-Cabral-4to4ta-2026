namespace JuegoDecisionQuest.Objetos.MecanicasPelea.Jugador
{
    public class EstadisticasJugador
    {
        public int Vida { get; private set; }
        public int VidaMaxima { get; private set; }
        public int Ataque { get; private set; }

        public EstadisticasJugador()
        {
            Vida = 150;
            VidaMaxima = 150;
            Ataque = 40;
        }

        public void RecibirDanio(int cantidad)
        {
            Vida -= cantidad;

            if (Vida < 0)
                Vida = 0;
        }

        public bool EstaVivo()
        {
            return Vida > 0;
        }
    }
}