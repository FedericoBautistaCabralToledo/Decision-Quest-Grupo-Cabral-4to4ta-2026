using JuegoDecisionQuest;
using JuegoDecisionQuest.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JuegoDecisionQuest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Mapa _mapa;

        Personaje jugador;

        // Texturas del jugador
        Texture2D TexturaParado;
        Texture2D TexturaDerecha;
        Texture2D TexturaIzquierda;

        Sprite parado;
        Sprite derecha;
        Sprite izquierda;

        // NPC RUEE
        NPC ruee;
        Texture2D texturaRuee;

        // OTROS DEMONIOS
        NPC demonio1;
        NPC demonio2;
        NPC demonio3;

        Texture2D texturaDemonio1;
        Texture2D texturaDemonio2;
        Texture2D texturaDemonio3;

        Song MusicaMapa;

        Colisiones colisiones;

        // Para ver las zonas sólidas mientras se prueba (F1 las muestra/oculta)
        Texture2D texturaColision;
        Texture2D pixel;
        bool mostrarColisiones = true;
        KeyboardState tecladoAnterior;

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

            // Cargar sprites del jugador
            TexturaParado = Content.Load<Texture2D>("parado");
            TexturaDerecha = Content.Load<Texture2D>("correrDerecha");
            TexturaIzquierda = Content.Load<Texture2D>("correrIzquierda");

            Vector2 posicioninicial = new Vector2(471, 72);

            parado = new Sprite(
                TexturaParado,
                posicioninicial,
                60,
                80
            );

            derecha = new Sprite(
                TexturaDerecha,
                posicioninicial,
                60,
                80
            );

            izquierda = new Sprite(
                TexturaIzquierda,
                posicioninicial,
                60,
                80
            );

            jugador = new Personaje(
                15,
                100,
                10,
                parado,
                izquierda,
                derecha
            );

            // Cargar mapa
            _mapa = new Mapa(Content);


            // CARGAR NPC RUEE

            texturaRuee = Content.Load<Texture2D>("Ruee");

            ruee = new NPC(
                texturaRuee,
                new Vector2(720, 535),
                80,
                90
            );


            // CARGAR LOS OTROS DEMONIOS

            texturaDemonio1 = Content.Load<Texture2D>("Demonio1");
            texturaDemonio2 = Content.Load<Texture2D>("Demonio2");
            texturaDemonio3 = Content.Load<Texture2D>("Demonio3");

            demonio1 = new NPC(
                texturaDemonio1,
                new Vector2(271, 567),
                80,
                90
            );

            demonio2 = new NPC(
                texturaDemonio2,
                new Vector2(60, 72),
                80,
                90
            );

            demonio3 = new NPC(
                texturaDemonio3,
                new Vector2(821, 62),
                80,
                90
            );


            // COLISIONES

            colisiones = new Colisiones(
                new Rectangle(0, 0, 1000, 750)
            );

            texturaColision = Content.Load<Texture2D>("sobreposicion");

            colisiones.CargarMascara(texturaColision);

            // Un píxel blanco para dibujar rectángulos de depuración
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            // Música
            MusicaMapa = Content.Load<Song>("MusicaMapa");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(MusicaMapa);
        }

        protected override void Update(GameTime gameTime)
        {
            // Leemos el teclado en cada frame
            KeyboardState teclado = Keyboard.GetState();

            if (teclado.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // F1 muestra u oculta las zonas sólidas
            if (teclado.IsKeyDown(Keys.F1) &&
                tecladoAnterior.IsKeyUp(Keys.F1))
            {
                mostrarColisiones = !mostrarColisiones;
            }

            tecladoAnterior = teclado;

            // Movimiento del jugador
            jugador.Update(gameTime, colisiones);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // Dibujar mapa
            _mapa.Dibujar(_spriteBatch);

            // Dibujar a Ruee
            ruee.Draw(_spriteBatch);

            // Dibujar los otros demonios
            demonio1.Draw(_spriteBatch);
            demonio2.Draw(_spriteBatch);
            demonio3.Draw(_spriteBatch);

            // Dibujar jugador
            jugador.Draw(_spriteBatch);

            // Mostrar colisiones
            if (mostrarColisiones)
            {
                _spriteBatch.Draw(
                    texturaColision,
                    new Rectangle(0, 0, 1000, 750),
                    Color.White * 0.6f
                );

                // Pies del jugador
                _spriteBatch.Draw(
                    pixel,
                    jugador.Hitbox,
                    Color.Blue * 0.8f
                );
            }

            _spriteBatch.End();

            base.Draw(gameTime);

            // El titulo de la ventana muestra la posición del jugador y si su hitbox está tocando una colision
            Window.Title = $"X: {(int)jugador.estado.posicion.X} Y: {(int)jugador.estado.posicion.Y} Choca: {colisiones.Choca(jugador.Hitbox)}";
        }
    }
}