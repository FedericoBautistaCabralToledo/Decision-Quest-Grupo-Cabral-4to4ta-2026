using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mundoparte2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        

        private Texture2D _mapa;
        private Texture2D _personaje;
        private Texture2D _sobreposicion;

        

        private Vector2 _posicionJugador;

        private float _velocidad = 180f;

        // Tamaño que tendrá el personaje en pantalla
        private int _anchoJugador = 48;
        private int _altoJugador = 48;

        

        private int _columnas = 4;
        private int _filas = 4;

        private int _frameActual = 0;

        
        private Matrix _camara;

        // Acercamiento
        private float _zoom = 1.35f;

        

        private Color[] _datosMapa;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 700;

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            

            _posicionJugador = new Vector2(
                700,
                520
            );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

           

            _mapa = Content.Load<Texture2D>("mapa");

            _personaje = Content.Load<Texture2D>("protagonista");

            _sobreposicion = Content.Load<Texture2D>("sobreposicion");

            

            _datosMapa = new Color[
                _mapa.Width * _mapa.Height
            ];

            _mapa.GetData(_datosMapa);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState teclado = Keyboard.GetState();

            if (teclado.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            float tiempo =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 movimiento = Vector2.Zero;

            
            if (teclado.IsKeyDown(Keys.W))
            {
                movimiento.Y -= 1;
            }

            if (teclado.IsKeyDown(Keys.S))
            {
                movimiento.Y += 1;
            }

            if (teclado.IsKeyDown(Keys.A))
            {
                movimiento.X -= 1;
            }

            if (teclado.IsKeyDown(Keys.D))
            {
                movimiento.X += 1;
            }

            if (movimiento != Vector2.Zero)
            {
                movimiento.Normalize();

                Vector2 nuevaPosicion =
                    _posicionJugador +
                    movimiento * _velocidad * tiempo;

               

                if (PuedeCaminar(nuevaPosicion))
                {
                    _posicionJugador = nuevaPosicion;
                }

                

                if (movimiento.Y < 0)
                {
                    // Arriba
                    _frameActual = 12;
                }
                else if (movimiento.Y > 0)
                {
                    // Abajo
                    _frameActual = 0;
                }
                else if (movimiento.X < 0)
                {
                    // Izquierda
                    _frameActual = 4;
                }
                else if (movimiento.X > 0)
                {
                    // Derecha
                    _frameActual = 8;
                }
            }

            
            CrearCamara();

            base.Update(gameTime);
        }

        
        private bool PuedeCaminar(Vector2 posicion)
        {
            int x = (int)posicion.X;
            int y = (int)posicion.Y;

            // Si está fuera del mapa
            if (x < 0 ||
                y < 0 ||
                x >= _mapa.Width ||
                y >= _mapa.Height)
            {
                return false;
            }

            int margen = 18;

            if (!EsPiso(x - margen, y + margen))
                return false;

            if (!EsPiso(x + margen, y + margen))
                return false;

            if (!EsPiso(x - margen, y - margen))
                return false;

            if (!EsPiso(x + margen, y - margen))
                return false;

            return true;
        }

        

        private bool EsPiso(int x, int y)
        {
            if (x < 0 ||
                y < 0 ||
                x >= _mapa.Width ||
                y >= _mapa.Height)
            {
                return false;
            }

            Color pixel =
                _datosMapa[y * _mapa.Width + x];

            int brillo =
                pixel.R +
                pixel.G +
                pixel.B;

            

            if (brillo > 180)
            {
                return true;
            }

            return false;
        }

        

        private void CrearCamara()
        {
            int anchoPantalla =
                GraphicsDevice.Viewport.Width;

            int altoPantalla =
                GraphicsDevice.Viewport.Height;

            _camara =
                Matrix.CreateTranslation(
                    -_posicionJugador.X,
                    -_posicionJugador.Y,
                    0
                )
                *
                Matrix.CreateScale(
                    _zoom
                )
                *
                Matrix.CreateTranslation(
                    anchoPantalla / 2f,
                    altoPantalla / 2f,
                    0
                );
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            

            _spriteBatch.Begin(
                transformMatrix: _camara,
                samplerState: SamplerState.PointClamp
            );

            _spriteBatch.Draw(
                _mapa,
                Vector2.Zero,
                Color.White
            );

            

            int anchoFrame =
                _personaje.Width / _columnas;

            int altoFrame =
                _personaje.Height / _filas;

            int columna =
                _frameActual % _columnas;

            int fila =
                _frameActual / _columnas;

            Rectangle sourceRectangle =
                new Rectangle(
                    columna * anchoFrame,
                    fila * altoFrame,
                    anchoFrame,
                    altoFrame
                );

            Rectangle destino =
                new Rectangle(
                    (int)_posicionJugador.X - _anchoJugador / 2,
                    (int)_posicionJugador.Y - _altoJugador / 2,
                    _anchoJugador,
                    _altoJugador
                );

            _spriteBatch.Draw(
                _personaje,
                destino,
                sourceRectangle,
                Color.White
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
