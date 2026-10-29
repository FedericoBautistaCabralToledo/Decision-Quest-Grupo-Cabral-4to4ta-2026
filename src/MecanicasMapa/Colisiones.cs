using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDecisionQuest
{
    public class Colisiones
    {
        // Zonas rectangulares por las que NO se puede pasar (X, Y, ancho, alto)
        public List<Rectangle> Obstaculos { get; } = new List<Rectangle>();

        // Área total por la que el personaje puede moverse
        public Rectangle LimitesMapa { get; }

        // Máscara de píxeles: _solido[i] es true si ese píxel de la imagen bloquea el paso
        private bool[] _solido;
        private int _anchoTextura;
        private int _altoTextura;

        public Colisiones(Rectangle limitesMapa)
        {
            LimitesMapa = limitesMapa;
        }

        // Lee la imagen UNA sola vez y guarda qué píxeles son sólidos.
        // umbralAlfa: opacidad mínima (0 a 255) para considerar un píxel sólido.
        // Con 10 casi cualquier píxel visible bloquea; súbelo (por ejemplo a 128)
        // para ignorar sombras o bordes suaves semitransparentes.
        public void CargarMascara(Texture2D textura, byte umbralAlfa = 10)
        {
            _anchoTextura = textura.Width;
            _altoTextura = textura.Height;

            // GetData copia todos los colores de la imagen a un arreglo
            Color[] colores = new Color[_anchoTextura * _altoTextura];
            textura.GetData(colores);

            _solido = new bool[colores.Length];
            for (int i = 0; i < colores.Length; i++)
            {
                // El canal A (alfa) es la opacidad: 0 = transparente, 255 = opaco
                _solido[i] = colores[i].A > umbralAlfa;
            }
        }

        // true si la caja se sale del mapa, toca un rectángulo o toca un píxel sólido
        public bool Choca(Rectangle caja)
        {
            if (!LimitesMapa.Contains(caja))
                return true;

            foreach (Rectangle obstaculo in Obstaculos)
            {
                if (caja.Intersects(obstaculo))
                    return true;
            }

            if (_solido != null && ChocaConMascara(caja))
                return true;

            return false;
        }

        private bool ChocaConMascara(Rectangle caja)
        {
            // La imagen se dibuja estirada sobre LimitesMapa (1000x750),
            // así que convertimos coordenadas de pantalla a coordenadas de la imagen.
            float escalaX = (float)_anchoTextura / LimitesMapa.Width;
            float escalaY = (float)_altoTextura / LimitesMapa.Height;

            // Recorremos cada píxel de pantalla de la hitbox (es pequeña, así que es rápido)
            for (int y = caja.Top; y < caja.Bottom; y++)
            {
                for (int x = caja.Left; x < caja.Right; x++)
                {
                    int tx = (int)((x - LimitesMapa.X) * escalaX);
                    int ty = (int)((y - LimitesMapa.Y) * escalaY);

                    // Si cae fuera de la imagen, lo ignoramos
                    if (tx < 0 || ty < 0 || tx >= _anchoTextura || ty >= _altoTextura)
                        continue;

                    // Si ese píxel de la imagen es sólido, hay choque
                    if (_solido[ty * _anchoTextura + tx])
                        return true;
                }
            }
            return false;
        }
    }
}