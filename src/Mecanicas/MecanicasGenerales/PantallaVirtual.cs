using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JuegoDecisionQuest
{
    // Todo el juego se dibuja en una "pantalla virtual" fija de 1000x750.
    // Al final se escala a la ventana real, centrada y sin deformarse (franjas negras si sobra espacio).
    public class PantallaVirtual
    {
        public const int Ancho = 1000;
        public const int Alto = 750;

        private readonly GraphicsDevice _dispositivo;
        private readonly SpriteBatch _spriteBatch;
        private readonly RenderTarget2D _destino;

        public PantallaVirtual(GraphicsDevice dispositivo)
        {
            _dispositivo = dispositivo;
            _spriteBatch = new SpriteBatch(dispositivo);
            _destino = new RenderTarget2D(dispositivo, Ancho, Alto);
        }

        // Zona de la pantalla real donde se muestra la imagen de 1000x750
        public Rectangle AreaDestino
        {
            get
            {
                int anchoReal = _dispositivo.PresentationParameters.BackBufferWidth;
                int altoReal = _dispositivo.PresentationParameters.BackBufferHeight;

                float escala = Math.Min((float)anchoReal / Ancho, (float)altoReal / Alto);

                int w = (int)(Ancho * escala);
                int h = (int)(Alto * escala);

                return new Rectangle((anchoReal - w) / 2, (altoReal - h) / 2, w, h);
            }
        }

        // Llamar AL PRINCIPIO de Draw, antes de limpiar y dibujar
        public void Comenzar()
        {
            _dispositivo.SetRenderTarget(_destino);
        }

        // Llamar AL FINAL de Draw, después de dibujar todo
        public void Terminar()
        {
            _dispositivo.SetRenderTarget(null);
            _dispositivo.Clear(Color.Black);   // franjas negras

            _spriteBatch.Begin();
            _spriteBatch.Draw(_destino, AreaDestino, Color.White);
            _spriteBatch.End();
        }

        // Convierte la posición del mouse (pantalla real) a coordenadas de 1000x750
        public MouseState ConvertirMouse(MouseState mouse)
        {
            Rectangle area = AreaDestino;

            if (area.Width == 0 || area.Height == 0)
                return mouse;

            int x = (int)((mouse.X - area.X) * (float)Ancho / area.Width);
            int y = (int)((mouse.Y - area.Y) * (float)Alto / area.Height);

            return new MouseState(
                x,
                y,
                mouse.ScrollWheelValue,
                mouse.LeftButton,
                mouse.MiddleButton,
                mouse.RightButton,
                mouse.XButton1,
                mouse.XButton2);
        }
    }
}