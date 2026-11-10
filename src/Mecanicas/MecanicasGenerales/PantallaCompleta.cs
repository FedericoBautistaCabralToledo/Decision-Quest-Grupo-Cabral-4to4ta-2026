using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JuegoDecisionQuest
{
    // Alterna entre ventana y pantalla completa al pulsar F11
    public class PantallaCompleta
    {
        private readonly GraphicsDeviceManager _graphics;
        private KeyboardState _tecladoAnterior;

        public PantallaCompleta(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;

            // Pantalla completa "sin bordes": cambia rápido y sin parpadeos
            _graphics.HardwareModeSwitch = false;
        }

        // Hay que llamarla en CADA frame, para que _tecladoAnterior esté siempre al día
        public void Actualizar(KeyboardState teclado)
        {
            bool f11Pulsada =
                teclado.IsKeyDown(Keys.F11) &&
                _tecladoAnterior.IsKeyUp(Keys.F11);

            if (f11Pulsada)
                _graphics.ToggleFullScreen();

            _tecladoAnterior = teclado;
        }
    }
}