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
        Texture2D TexturaParado;
        Texture2D TexturaDerecha;
        Texture2D TexturaIzquierda;
        Sprite parado;
        Sprite derecha;
        Sprite izquierda;
        Song MusicaMapa;

        Colisiones colisiones;

        // Para ver las zonas sólidas mientras se prueba (F1 las muestra/oculta)
        Texture2D texturaColision;
        Texture2D pixel;                 //para dibujar la hitbox del jugador
        bool mostrarColisiones = true;
        KeyboardState tecladoAnterior;   //el campo "teclado": ahora es local en Update

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

            Vector2 posicioninicial = new Vector2(496, 363);
            parado = new Sprite(TexturaParado, posicioninicial, 60, 80);
            derecha = new Sprite(TexturaDerecha, posicioninicial, 60, 80);
            izquierda = new Sprite(TexturaIzquierda, posicioninicial, 60, 80);
            jugador = new Personaje(15, 100, 10, parado, izquierda, derecha);
            _mapa = new Mapa(Content);

            colisiones = new Colisiones(new Rectangle(0, 0, 1000, 750));
            texturaColision = Content.Load<Texture2D>("sobreposicion");
            colisiones.CargarMascara(texturaColision);

            // Un píxel blanco, para dibujar rectángulos de depuración
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            //Estas lineas hacen que suene la musica
            MusicaMapa = Content.Load<Song>("MusicaMapa");   // el nombre del archivo, sin extensión

            MediaPlayer.IsRepeating = true;   // que se repita al terminar
            MediaPlayer.Volume = 0.2f;        // volumen de 0.0 a 1.0
            MediaPlayer.Play(MusicaMapa);
        }

        protected override void Update(GameTime gameTime)
        {
            // Leemos el teclado en CADA frame (antes se leía solo al crear el juego)
            KeyboardState teclado = Keyboard.GetState();

            if (teclado.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // F1 muestra u oculta las zonas sólidas
            if (teclado.IsKeyDown(Keys.F1) && tecladoAnterior.IsKeyUp(Keys.F1))
            {
                mostrarColisiones = !mostrarColisiones;
            }
            tecladoAnterior = teclado;

            jugador.Update(gameTime, colisiones);

            Window.Title = "DECISION QUEST";
            // El titulo de la ventana mustra la posición del jugador y si su hitbox está tocando una colision
            //Window.Title = $"X: {(int)jugador.estado.posicion.X}  Y: {(int)jugador.estado.posicion.Y}  Choca: {colisiones.Choca(jugador.Hitbox)}";

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _mapa.Dibujar(_spriteBatch);
            jugador.Draw(_spriteBatch);

            if (mostrarColisiones)
            {
                // La máscara que realmente se cargó, encima del mapa
                _spriteBatch.Draw(texturaColision, new Rectangle(0, 0, 1000, 750), Color.White * 0.6f);
                // Los pies del jugador, en azul
                _spriteBatch.Draw(pixel, jugador.Hitbox, Color.Blue * 0.8f);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}