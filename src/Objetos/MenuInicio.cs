using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDecisionQuest
{
    public class MenuInicio
    {
        private Texture2D _menu;
        private Texture2D _botonInicio;
        private Texture2D _botonSalir;

        // Rectángulos públicos para dibujar y detectar clics del mouse
        public Rectangle RectanguloInicio { get; set; }
        public Rectangle RectanguloSalir { get; set; }

        public MenuInicio(ContentManager content)
        {
            _menu = content.Load<Texture2D>("MenuTitulo");
            _botonInicio = content.Load<Texture2D>("BotonInicio");
            _botonSalir = content.Load<Texture2D>("BotonSalida");

            // Configura aquí las posiciones (X, Y) y tamaños (Ancho, Alto) reales de tus botones
            // Ajusta estos números según cómo quieras que se vean en tu pantalla de 1000x750
            RectanguloInicio = new Rectangle(350, 300, 300, 100);
            RectanguloSalir = new Rectangle(350, 450, 300, 100);
        }

        public void Dibujar(SpriteBatch spriteBatch)
        {
            // 1. Aseguramos que el fondo ocupe toda la pantalla de 1000x750
            Rectangle fondoDestino = new Rectangle(0, 0, 1000, 750);
            spriteBatch.Draw(_menu, fondoDestino, Color.White);

            // 2. Dibujamos el botón de inicio. 
            spriteBatch.Draw(_botonInicio, RectanguloInicio, Color.White);

            // 3. Dibujamos el botón de salir.
            spriteBatch.Draw(_botonSalir, RectanguloSalir, Color.White);
        }
    }
}