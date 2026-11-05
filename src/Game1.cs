using JuegoDecisionQuest;
using JuegoDecisionQuest.Objetos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JuegoDecisionQuest
{
    // Definimos los posibles estados del juego
    public enum EstadoJuego
    {
        Menu,
        Jugando
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Mapa _mapa;
        private MenuInicio _menu;
        private Camara _camara;

        // Variable para controlar en qué pantalla estamos
        private EstadoJuego _estadoActual = EstadoJuego.Menu;

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
        bool mostrarColisiones = false;
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

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // Inicializar la cámara usando el Viewport correcto del GraphicsDevice
            _camara = new Camara(GraphicsDevice.Viewport);

            _camara = new Camara(GraphicsDevice.Viewport);
            _camara.Zoom = 2.0f; // Ajusta este número (1.5f, 2.0f, 0.5f para alejar, etc.)


            //una variable auxiliar que guarda la posicion en la que va a iniciar cada sprite
            //es para simplemente poner posicioninicial en vez de todo el choclo de new Vector2...
            Vector2 posicioninicial = new Vector2(450, 42);

            //acá creamos todos los sprites, le damos la misma posicion de arranque y el mismo tamaño para que no haya conflicto
            parado = new Sprite(
                TexturaParado,
                posicioninicial,
                120,
                60
            );

            derecha = new Sprite(
                TexturaDerecha,
                posicioninicial,
                120,
                60
            );

            izquierda = new Sprite(
                TexturaIzquierda,
                posicioninicial,
                120,
                60
            );

            //acá creamos al jugador y le damos los 3 sprites que necesita para lograr mostrar que se está moviendo (además de orgarle vida, defensa y ataque)
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

            // SOLUCIÓN: Inicializar el menú pasándole el ContentManager obligatorio
            _menu = new MenuInicio(Content);

            // Música del juego
            MusicaMapa = Content.Load<Song>("MusicaMapa");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;

            // NOTA: Quitamos el MediaPlayer.Play de aquí para que no suene en el menú de inicio.
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState teclado = Keyboard.GetState();

            if (teclado.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (teclado.IsKeyDown(Keys.F1) && tecladoAnterior.IsKeyUp(Keys.F1))
            {
                mostrarColisiones = !mostrarColisiones;
            }

            tecladoAnterior = teclado;

            //Logica del menu que detecta el mouse
            if (_estadoActual == EstadoJuego.Menu)
            {
                MouseState raton = Mouse.GetState();

                // Verificamos si se presionó el botón izquierdo del mouse
                if (raton.LeftButton == ButtonState.Pressed)
                {
                    // Creamos un punto en la posición actual del cursor
                    Point posicionRaton = new Point(raton.X, raton.Y);

                    // El cursor está dentro del botón de Inicio?
                    if (_menu.RectanguloInicio.Contains(posicionRaton))
                    {
                        _estadoActual = EstadoJuego.Jugando; //Inicia el juego inicial
                        MediaPlayer.Play(MusicaMapa); // Inicia la música del juego
                    }

                    //¿El cursor está dentro del botón de Salir?
                    else if (_menu.RectanguloSalir.Contains(posicionRaton))
                    {
                        Exit(); // Cierra el juego de inmediato
                    }
                }
            }
            else if (_estadoActual == EstadoJuego.Jugando)
            {
                jugador.Update(gameTime, colisiones);
                _camara.Seguir(jugador.Hitbox);
            }

          

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.Clear(Color.Black);

            if (_estadoActual == EstadoJuego.Menu)
            {
                // El menú se inicia de forma normal (estático en pantalla)
                _spriteBatch.Begin();
                _menu.Dibujar(_spriteBatch);
                _spriteBatch.End();
            }
            else if (_estadoActual == EstadoJuego.Jugando)
            {
                // IMPORTANTE: Pasamos la matriz de transformación de la cámara aquí
                _spriteBatch.Begin(transformMatrix: _camara.Transformacion);

                // Todo lo que se dibuje aquí dentro se moverá junto con la cámara
                _mapa.Dibujar(_spriteBatch);
                ruee.Draw(_spriteBatch);
                demonio1.Draw(_spriteBatch);
                demonio2.Draw(_spriteBatch);
                demonio3.Draw(_spriteBatch);
                jugador.Draw(_spriteBatch);

                // Mostrar colisiones si F1 está activado
                if (mostrarColisiones)
                {
                    _spriteBatch.Draw(texturaColision, new Rectangle(0, 0, 1000, 750), Color.White * 0.6f);
                    _spriteBatch.Draw(pixel, jugador.Hitbox, Color.Blue * 0.8f);
                }

                _spriteBatch.End();
            }

            base.Draw(gameTime);

            // Actualizar título de la ventana dinámicamente según el estado
            if (_estadoActual == EstadoJuego.Jugando)
            {
                Window.Title = $"X: {(int)jugador.estado.posicion.X} Y: {(int)jugador.estado.posicion.Y} Choca: {colisiones.Choca(jugador.Hitbox)}";
            }
            else
            {
                Window.Title = "DECISION QUEST";
            }
        }
    }
}
