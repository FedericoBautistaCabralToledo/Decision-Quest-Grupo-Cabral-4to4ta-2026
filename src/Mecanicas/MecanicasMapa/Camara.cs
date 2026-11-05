using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDecisionQuest
{
    public class Camara
    {
        // Matriz de transformación que le pasaremos al SpriteBatch
        public Matrix Transformacion { get; set; }

        private Viewport _viewport;

        public float Zoom { get; set; }

        public Camara(Viewport viewport)
        {
            _viewport = viewport;
        }

        // Replicación del código de la imagen adaptado a matrices de MonoGame
        public void Seguir(Rectangle target)
        {
            // Calculamos el centro exacto del rectángulo objetivo
            float centroX = target.X + (target.Width / 2f);
            float centroY = target.Y + (target.Height / 2f);

            // Creamos la matriz de transformación usando el centro del objetivo y el centro de la pantalla
            Transformacion = Matrix.CreateTranslation(-centroX, -centroY, 0) *
                             Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                             Matrix.CreateTranslation(_viewport.Width / 2f, _viewport.Height / 2f, 0);
        }
    }
}
