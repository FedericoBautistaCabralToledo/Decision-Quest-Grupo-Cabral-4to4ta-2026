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

        public NPC(Texture2D textura, Vector2 posicion, int ancho, int alto)
        {
            this.textura = textura;
            this.posicion = posicion;
            this.ancho = ancho;
            this.alto = alto;
        }

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle(
                    (int)posicion.X,
                    (int)posicion.Y,
                    ancho,
                    alto
                );
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destino = new Rectangle(
                (int)posicion.X,
                (int)posicion.Y,
                ancho,
                alto
            );

            spriteBatch.Draw(
                textura,
                destino,
                Color.White
            );
        }
    }
}