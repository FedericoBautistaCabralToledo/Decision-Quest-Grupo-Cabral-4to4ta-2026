using JuegoDecisionQuest;
using JuegoDecisionQuest.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JuegoDecisionQuest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Mapa _mapa;
        //esta variable es la que va a guardar al jugador
        Personaje jugador;
        //estas son todas las texturas que necesita el sprite (las imagenes)
        Texture2D TexturaParado;
        Texture2D TexturaDerecha;
        Texture2D TexturaIzquierda;
        //y estos son simplemente los sprites para cada textura (para que aparezcan en el juego)
        Sprite parado;
        Sprite derecha;
        Sprite izquierda;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 750;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //esto carga cada imagen para que puedan usarse y crear los sprites
            TexturaParado = Content.Load<Texture2D>("parado");
            TexturaDerecha = Content.Load<Texture2D>("correrDerecha");
            TexturaIzquierda = Content.Load<Texture2D>("correrIzquierda");
            //una variable auxiliar que guarda la posicion en la que va a iniciar cada sprite
            //es para simplemente poner posicioninicial en vez de todo el choclo de new Vector2...
            Vector2 posicioninicial = new Vector2(0, 0);
            //acá creamos todos los sprites, le damos la misma posicion de arranque y el mismo tamaño para que no haya conflicto
            parado = new Sprite(TexturaParado, posicioninicial, 200, 100);
            derecha = new Sprite(TexturaDerecha, posicioninicial, 200, 100);
            izquierda = new Sprite(TexturaIzquierda, posicioninicial, 200, 100);
            //acá creamos al jugador y le damos los 3 sprites que necesita para lograr mostrar que se está moviendo (además de orgarle vida, defensa y ataque)
            jugador = new Personaje(15, 100, 10, parado, izquierda, derecha);

            _mapa = new Mapa(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            //esto lo único que hace es llamar al update del jugador para que pueda moverse el personaje
            jugador.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _spriteBatch.Begin();

            jugador.Draw(_spriteBatch);

            _mapa.Dibujar(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
