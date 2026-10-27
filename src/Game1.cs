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
        Personaje jugador;
        Texture2D TexturaParado;
        Texture2D TexturaDerecha;
        Texture2D TexturaIzquierda;
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
            TexturaParado = Content.Load<Texture2D>("parado");
            TexturaDerecha = Content.Load<Texture2D>("correrDerecha");
            TexturaIzquierda = Content.Load<Texture2D>("correrIzquierda");
            Vector2 posicioninicial = new Vector2(0, 0);

            parado = new Sprite(TexturaParado, posicioninicial, 200, 100);
            derecha = new Sprite(TexturaDerecha, posicioninicial, 200, 100);
            izquierda = new Sprite(TexturaIzquierda, posicioninicial, 200, 100);
            jugador = new Personaje(15, 100, 10, parado, izquierda, derecha);

            _mapa = new Mapa(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
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
