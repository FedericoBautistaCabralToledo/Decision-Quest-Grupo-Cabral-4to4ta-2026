using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDecisionQuest
{
    public class Mapa
    {
        private Texture2D _mapa;
        private Texture2D _sobreposicion;

        public Mapa(ContentManager content)
        {
            _mapa = content.Load<Texture2D>("mapa");
            _sobreposicion = content.Load<Texture2D>("sobreposicion");
        }

        public void Dibujar(SpriteBatch spriteBatch)
        {
            Rectangle destino = new Rectangle(0, 0, 1000, 750);

            // Dibujar primero la capa inferior.
            spriteBatch.Draw(
                _sobreposicion,
                destino,
                Color.White
            );

            // Dibujar el mapa por encima.
            spriteBatch.Draw(
                _mapa,
                destino,
                Color.White
            );
        }
    }

}
