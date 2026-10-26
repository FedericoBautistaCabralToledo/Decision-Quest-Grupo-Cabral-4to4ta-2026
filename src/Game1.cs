using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mundoparte3;

namespace mundoparte3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Mapa _mapa;
        public Personaje jugador;
        Texture2D TexturaPersonaje;
        Sprite spritejugador;

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
            TexturaPersonaje = Content.Load<Texture2D>("caballero_hollow");
            spritejugador = new Sprite(TexturaPersonaje, new Vector2(0, 0), 64, 64);
            jugador = new Personaje(15, 100, 10, spritejugador);

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
