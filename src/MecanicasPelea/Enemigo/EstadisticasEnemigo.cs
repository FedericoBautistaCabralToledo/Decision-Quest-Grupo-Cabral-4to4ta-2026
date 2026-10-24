using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoDecisionQuest.MecanicasPelea.Enemigo
{
    public class EstadisticasEnemigo
    {
        int vida { get; set; }
        int ataque { get; set; }
        int defensa { get; set; }
        public EstadisticasEnemigo(int vida, int ataque, int defensa)
        {
            this.vida = vida;
            this.ataque = ataque;
            this.defensa = defensa;
        }
    }
}
