using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JuegoDecisionQuest.Objetos.MecanicasPelea.Jugador
{
    public class AtaqueJugador
    {
        private KeyboardState tecladoAnterior;
        private MouseState ratonAnterior;

        public Rectangle BotonAtacar { get; private set; }

        public AtaqueJugador()
        {
            BotonAtacar = new Rectangle(400, 620, 200, 55);
        }

        public bool Actualizar(
            KeyboardState teclado,
            MouseState raton)
        {
            bool enter =
                teclado.IsKeyDown(Keys.Enter) &&
                tecladoAnterior.IsKeyUp(Keys.Enter);

            bool click =
                BotonAtacar.Contains(raton.Position) &&
                raton.LeftButton == ButtonState.Pressed &&
                ratonAnterior.LeftButton == ButtonState.Released;

            tecladoAnterior = teclado;
            ratonAnterior = raton;

            return enter || click;
        }
    }
}