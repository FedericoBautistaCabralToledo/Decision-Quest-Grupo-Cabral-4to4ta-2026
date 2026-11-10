using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDecisionQuest.Objetos
{
    public class NPC
    {
        public Texture2D textura;
        public Vector2 posicion;

        public int ancho;
        public int alto;

        public string Nombre { get; set; }

        // LISTA ENLAZADA: líneas que dice el NPC, en orden.
        // Permite insertar o quitar líneas según el estado del juego (AddFirst, AddLast, Remove).
        public LinkedList<string> Lineas { get; set; } = new LinkedList<string>();

        public NPC(Texture2D textura, Vector2 posicion, int ancho, int alto,
                   string nombre = "???")
        {
            this.textura = textura;
            this.posicion = posicion;
            this.ancho = ancho;
            this.alto = alto;
            Nombre = nombre;
        }

        // Todo el sprite
        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)posicion.X, (int)posicion.Y, ancho, alto);
            }
        }

        // Zona sólida del NPC: solo sus pies, para que el jugador pueda pasar "por detrás".
        public Rectangle CajaColision
        {
            get
            {
                int altoPies = 30;
                return new Rectangle(
                    (int)posicion.X + 10,
                    (int)posicion.Y + alto - altoPies,
                    ancho - 20,
                    altoPies);
            }
        }

        // Zona donde el jugador puede hablarle: la caja de colisión ampliada 30 px por cada lado
        public Rectangle ZonaInteraccion
        {
            get
            {
                Rectangle zona = CajaColision;
                zona.Inflate(30, 30);
                return zona;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, Hitbox, Color.White);
        }
    }
}