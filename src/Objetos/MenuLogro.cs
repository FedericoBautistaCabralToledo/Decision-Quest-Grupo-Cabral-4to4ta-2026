using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDecisionQuest.Objetos
{
    internal class MenuLogro
    {
        private Texture2D _menuLogro;
        //Estos son los sprites de los logros que no se han desbloqueado
        private Texture2D LogrosAtrasBoton;
        private Texture2D LogroCofreNO;
        private Texture2D LogroGanasteNO;
        private Texture2D LogroDesarrolloNO;
        //Estos son los sprites de los logros que se han desbloqueado
        private Texture2D LogroCofreSI;
        private Texture2D LogroGanasteSI;
        private Texture2D LogroDesarrolloSI;

        public Rectangle RectanguloAtras { get; set; }

        public MenuLogro(ContentManager content)
        {
            _menuLogro = content.Load<Texture2D>("MenuLogros");
            LogrosAtrasBoton = content.Load<Texture2D>("BotonVolverLogros");
            LogroCofreNO = content.Load<Texture2D>("Logro1-X");
            LogroGanasteNO = content.Load<Texture2D>("Logro2-X");
            LogroDesarrolloNO = content.Load<Texture2D>("Logro3-X");
            LogroCofreSI = content.Load<Texture2D>("Logro1");
            LogroDesarrolloSI = content.Load<Texture2D>("Logro3");
            LogroGanasteSI = content.Load<Texture2D>("Logro2");

            // Abajo del todo, para no quedar encima del botón "Logros" del menú principal
            RectanguloAtras = new Rectangle(20, 20, 150, 50);
        }

        public void Dibujar(SpriteBatch spriteBatch)
        {
            Rectangle fondoDestino = new Rectangle(0, 0, 1000, 750);
            spriteBatch.Draw(_menuLogro, fondoDestino, Color.White);

            // Botón de volver al menú
            spriteBatch.Draw(LogrosAtrasBoton, RectanguloAtras, Color.White);
        }
    }
}