using JuegoDecisionQuest;
using JuegoDecisionQuest.MecanicasGenerales;
using JuegoDecisionQuest.Objetos;
using JuegoDecisionQuest.Objetos.MecanicasPelea;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;


namespace JuegoDecisionQuest
{
    // Definimos los posibles estados del juego
    public enum EstadoJuego
    {
        Menu,
        Logros,
        Jugando,
        Pelea
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Mapa _mapa;
        private MenuInicio _menu;
        private MenuLogro _logros;
        private Camara _camara;
        private PantallaCompleta _pantalla;
        private PantallaVirtual _virtual;


        //parte de dialogo
        Texture2D texturaC;
        Rectangle caja;
        CasillaDialogo dialogo;
        SpriteFont fuentedialogo;

        // Variable para controlar en qué pantalla estamos
        private EstadoJuego _estadoActual = EstadoJuego.Menu;

        // PILA: estados anteriores del juego (se entra a la pelea o a logros con Push, se vuelve con Pop)
        Stack<EstadoJuego> pilaEstados = new Stack<EstadoJuego>();

        // PILA: historial de decisiones tomadas contra Ruee (la última queda arriba)
        Stack<PeleaConRuee.Decision> historialDecisiones = new Stack<PeleaConRuee.Decision>();

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

        // LISTA ENLAZADA: todos los NPC (quitar a Ruee al derrotarla es O(1))
        LinkedList<NPC> npcs = new LinkedList<NPC>();
        NPC npcCercano;                     // el NPC con el que se puede hablar ahora (o null)
        NPC npcHablando;                    // el NPC con el que se está hablando (o null)

        // COLA (FIFO): líneas del diálogo actual que todavía faltan por mostrar
        Queue<string> lineasPendientes = new Queue<string>();

        // Línea que se inserta al principio del diálogo de Ruee cuando el jugador perdió
        const string LineaRevancha = "¿Otra vez? Veamos si esta vez lo logras.";

        Texture2D texturaDemonio1;
        Texture2D texturaDemonio2;
        Texture2D texturaDemonio3;

        // Todo lo de la pelea con Ruee
        PeleaConRuee pelea;
        Texture2D texturaCorazon;
        Texture2D texturaCalavera;
        Texture2D texturaFlecha;
        Texture2D texturaMonstruoRayo;
        Texture2D texturaRayo;
        float tiempoFinPelea;     // cuánto lleva mostrándose el resultado
        bool peleaResuelta;       // true si Ruee fue derrotada o perdonada (ya no se vuelve a pelear)

        // GRAFO + DFS: estados de la pelea
        GrafoFinales grafo;

        // Panel de desarrollo (F3): caminos del grafo e historial de decisiones
        bool mostrarDev = false;
        List<string> panelDev = new List<string>();

        Song MusicaMapa;

        Colisiones colisiones;

        // Para ver las zonas sólidas mientras se prueba (F1 las muestra/oculta)
        Texture2D texturaColision;
        Texture2D pixel;
        bool mostrarColisiones = false;
        KeyboardState tecladoAnterior;
        MouseState ratonAnterior;

        //_pantalla hace que la pantalla pase de ventana a pantalla completa y viceversa, pero no cambia la resolución de la ventana
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 600;

            _pantalla = new PantallaCompleta(_graphics);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _virtual = new PantallaVirtual(GraphicsDevice);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Cargar sprites del jugador
            TexturaParado = Content.Load<Texture2D>("parado");
            TexturaDerecha = Content.Load<Texture2D>("correrDerecha");
            TexturaIzquierda = Content.Load<Texture2D>("correrIzquierda");
            fuentedialogo = Content.Load<SpriteFont>("fuentes/fuentetextos");

            //Parte de dialogos
            texturaC = new Texture2D(GraphicsDevice, 1, 1);
            texturaC.SetData(new[] { Color.White });

            //el graphics device view port define automáticamente el tamaño de la pantalla sin que tengamos que hacerlo manualmente
            caja = new Rectangle(20, 20, GraphicsDevice.Viewport.Width - 40, 130);

            dialogo = new CasillaDialogo(texturaC, caja, fuentedialogo);

            // Inicializar la cámara usando el Viewport correcto del GraphicsDevice
            _camara = new Camara(GraphicsDevice.Viewport);
            _camara.Zoom = 2.0f;

            //una variable auxiliar que guarda la posicion en la que va a iniciar cada sprite
            Vector2 posicioninicial = new Vector2(450, 42);

            //acá creamos todos los sprites, le damos la misma posicion de arranque y el mismo tamaño para que no haya conflicto
            parado = new Sprite(TexturaParado, posicioninicial, 120, 60);
            derecha = new Sprite(TexturaDerecha, posicioninicial, 120, 60);
            izquierda = new Sprite(TexturaIzquierda, posicioninicial, 120, 60);

            //acá creamos al jugador y le damos los 3 sprites que necesita
            jugador = new Personaje(15, 100, 10, parado, izquierda, derecha);

            // Cargar mapa
            _mapa = new Mapa(Content);

            // CARGAR NPC RUEE
            texturaRuee = Content.Load<Texture2D>("Ruee");
            ruee = new NPC(texturaRuee, new Vector2(720, 535), 80, 90);

            // CARGAR LOS OTROS DEMONIOS
            texturaDemonio1 = Content.Load<Texture2D>("Demonio1");
            texturaDemonio2 = Content.Load<Texture2D>("Demonio2");
            texturaDemonio3 = Content.Load<Texture2D>("Demonio3");

            demonio1 = new NPC(texturaDemonio1, new Vector2(271, 567), 80, 90);
            demonio2 = new NPC(texturaDemonio2, new Vector2(60, 72), 80, 90);
            demonio3 = new NPC(texturaDemonio3, new Vector2(821, 62), 80, 90);

            // COLISIONES
            colisiones = new Colisiones(new Rectangle(0, 0, 1000, 750));

            texturaColision = Content.Load<Texture2D>("sobreposicion");
            colisiones.CargarMascara(texturaColision);

            // Esto es lo que dice cada personaje (cada AddLast es una línea del diálogo)
            ruee.Nombre = "Ruee";
            ruee.Lineas.AddLast("Hola, viajero.");
            ruee.Lineas.AddLast("Cuidado con lo que decidas. [E] pelear - [Q] irte");

            demonio1.Nombre = "Demonio 1";
            demonio1.Lineas.AddLast("No deberías estar aquí.");

            demonio2.Nombre = "Demonio 2";
            demonio2.Lineas.AddLast("Sigue el camino y lo verás.");

            demonio3.Nombre = "Demonio 3";
            demonio3.Lineas.AddLast("phonk");

            npcs.AddLast(ruee);
            npcs.AddLast(demonio1);
            npcs.AddLast(demonio2);
            npcs.AddLast(demonio3);

            // Los pies de cada NPC pasan a ser sólidos
            foreach (NPC npc in npcs)
                colisiones.Obstaculos.Add(npc.CajaColision);

            // GRAFO de estados de la pelea (para el panel de desarrollo)
            grafo = GrafoFinales.DePeleaConRuee();

            // Un píxel blanco para dibujar rectángulos de depuración
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            // Texturas de la pelea (los nombres deben coincidir con tus archivos en Content)
            texturaCorazon = Content.Load<Texture2D>("corazon");
            texturaCalavera = Content.Load<Texture2D>("calavera");
            texturaFlecha = Content.Load<Texture2D>("flecha");
            texturaMonstruoRayo = Content.Load<Texture2D>("monstruoRayo");
            texturaRayo = Content.Load<Texture2D>("Rayo");

            _menu = new MenuInicio(Content);
            _logros = new MenuLogro(Content);

            // Música del juego
            MusicaMapa = Content.Load<Song>("MusicaMapa");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState teclado = Keyboard.GetState();
            _pantalla.Actualizar(teclado);
            MouseState raton = _virtual.ConvertirMouse(Mouse.GetState());
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // true solo en el frame en que se pulsa el botón izquierdo (no mientras se mantiene)
            bool clic =
                raton.LeftButton == ButtonState.Pressed &&
                ratonAnterior.LeftButton == ButtonState.Released;

            if (teclado.IsKeyDown(Keys.Escape))
            {
               _estadoActual = EstadoJuego.Menu;
            }

            if (Presionada(teclado, Keys.F1))
            {
                mostrarColisiones = !mostrarColisiones;
            }

            // F2: panel de desarrollo
            if (Presionada(teclado, Keys.F3))
            {
                mostrarDev = !mostrarDev;

                if (mostrarDev)
                    ActualizarPanelDev();
            }

            // Lógica del menú que detecta el mouse
            if (_estadoActual == EstadoJuego.Menu)
            {
                if (clic)
                {
                    Point posicionRaton = raton.Position;

                    if (_menu.RectanguloInicio.Contains(posicionRaton))
                    {
                        _estadoActual = EstadoJuego.Jugando;
                        MediaPlayer.Play(MusicaMapa);
                    }
                    else if (_menu.RectanguloLogros.Contains(posicionRaton))
                    {
                        // Se apila el menú para poder volver a él con el botón Atrás
                        CambiarA(EstadoJuego.Logros);
                    }
                    else if (_menu.RectanguloSalir.Contains(posicionRaton))
                    {
                        Exit();
                    }
                }
            }
            else if (_estadoActual == EstadoJuego.Logros)
            {
                if (clic && _logros.RectanguloAtras.Contains(raton.Position))
                    Volver();
            }
            else if (_estadoActual == EstadoJuego.Jugando)
            {
                // Mientras hay un diálogo abierto, el jugador no se mueve
                if (!dialogo.Visible)
                    jugador.Update(gameTime, colisiones);

                _camara.Seguir(jugador.Hitbox);

                // ¿Hay algún NPC al alcance del jugador?
                npcCercano = null;
                foreach (NPC npc in npcs)
                {
                    if (jugador.Hitbox.Intersects(npc.ZonaInteraccion))
                    {
                        npcCercano = npc;
                        break;
                    }
                }

                if (Presionada(teclado, Keys.E))
                {
                    if (!dialogo.Visible)
                    {
                        // E junto a un NPC: abre su diálogo
                        if (npcCercano != null)
                            AbrirDialogo(npcCercano);
                    }
                    else if (lineasPendientes.Count > 0)
                    {
                        // Todavía quedan líneas: muestra la siguiente
                        dialogo.Mostrar(npcHablando.Nombre, lineasPendientes.Dequeue());
                    }
                    else if (npcHablando == ruee && !peleaResuelta)
                    {
                        // Última línea de Ruee: empieza la pelea
                        IniciarPelea();
                    }
                    else
                    {
                        // Última línea de cualquier otro NPC: cierra el diálogo
                        CerrarDialogo();
                    }
                }

                // Q cierra el diálogo
                if (Presionada(teclado, Keys.Q))
                {
                    CerrarDialogo();
                }
            }
            else if (_estadoActual == EstadoJuego.Pelea)
            {
                pelea.Update(gameTime, raton);

                // Cuando termina, se deja ver el resultado 3 segundos y se vuelve al mapa
                if (pelea.Terminado)
                {
                    tiempoFinPelea += delta;

                    if (tiempoFinPelea >= 3f)
                        TerminarPelea();
                }
            }

            // Se guarda AL FINAL para poder comparar "antes" y "ahora" en el siguiente frame
            tecladoAnterior = teclado;
            ratonAnterior = raton;

            base.Update(gameTime);
        }

        // true solo en el frame en que se pulsa la tecla, no mientras se mantiene apretada
        private bool Presionada(KeyboardState actual, Keys tecla)
        {
            return actual.IsKeyDown(tecla) && tecladoAnterior.IsKeyUp(tecla);
        }

        // PILA de estados: guarda el estado actual y pasa a otro
        private void CambiarA(EstadoJuego nuevo)
        {
            pilaEstados.Push(_estadoActual);
            _estadoActual = nuevo;
        }

        // PILA de estados: vuelve al estado anterior
        private void Volver()
        {
            if (pilaEstados.Count > 0)
                _estadoActual = pilaEstados.Pop();
            else
                _estadoActual = EstadoJuego.Jugando;
        }

        // Carga las líneas del NPC en la cola y muestra la primera
        private void AbrirDialogo(NPC npc)
        {
            if (npc.Lineas.Count == 0)
                return;

            npcHablando = npc;
            lineasPendientes = new Queue<string>(npc.Lineas);
            dialogo.Mostrar(npc.Nombre, lineasPendientes.Dequeue());
        }

        private void CerrarDialogo()
        {
            dialogo.Ocultar();
            lineasPendientes.Clear();
            npcHablando = null;
        }

        // Crea una pelea nueva y cambia al estado Pelea
        private void IniciarPelea()
        {
            CerrarDialogo();
            tiempoFinPelea = 0;

            // Se crea una pelea nueva cada vez, así la vida de ambos vuelve a empezar
            pelea = new PeleaConRuee(
                GraphicsDevice,
                fuentedialogo,
                texturaCorazon,
                texturaRuee,
                texturaCalavera,
                texturaFlecha,
                texturaMonstruoRayo,
                texturaRayo);

            CambiarA(EstadoJuego.Pelea);
        }

        // Aplica el resultado de la pelea en el mapa y vuelve a jugar
        private void TerminarPelea()
        {
            // Se apila la decisión tomada (si hubo una)
            if (pelea.DecisionTomada != PeleaConRuee.Decision.Ninguna)
                historialDecisiones.Push(pelea.DecisionTomada);

            if (pelea.Victoria)
            {
                // Ruee desaparece del mapa y deja de ser sólida
                npcs.Remove(ruee);
                colisiones.Obstaculos.Remove(ruee.CajaColision);
                peleaResuelta = true;
            }
            else if (pelea.Perdonado)
            {
                ruee.Lineas.Clear();
                ruee.Lineas.AddLast("jaja te re cabio ahora no me vas a poder matarrr");
                peleaResuelta = true;
            }
            else
            {
                // Derrota: no se marca como resuelta, se puede volver a intentar.
                // LISTA ENLAZADA: se inserta una línea al principio (solo una vez)
                if (ruee.Lineas.First == null || ruee.Lineas.First.Value != LineaRevancha)
                    ruee.Lineas.AddFirst(LineaRevancha);
            }

            if (mostrarDev)
                ActualizarPanelDev();

            Volver();
        }

        // Arma el texto del panel de desarrollo (F2)
        private void ActualizarPanelDev()
        {
            panelDev.Clear();

            // GRAFO + DFS
            panelDev.Add("CAMINOS A LOS FINALES:");
            foreach (string camino in grafo.CaminosHastaFinales("Atacar"))
                panelDev.Add("  " + camino);

            // PILA (el foreach de una pila va de la más reciente a la más antigua)
            panelDev.Add("HISTORIAL DE DECISIONES:");

            if (historialDecisiones.Count == 0)
                panelDev.Add("  (vacio)");

            foreach (PeleaConRuee.Decision d in historialDecisiones)
                panelDev.Add("  " + d);
        }

        // ORDENAMIENTO POR SELECCIÓN de los objetos a dibujar, según la Y de sus pies
        private static void OrdenarPorSeleccion(List<KeyValuePair<float, Action>> lista)
        {
            for (int i = 0; i < lista.Count - 1; i++)
            {
                int min = i;

                for (int j = i + 1; j < lista.Count; j++)
                {
                    if (lista[j].Key < lista[min].Key)
                        min = j;
                }

                KeyValuePair<float, Action> aux = lista[i];
                lista[i] = lista[min];
                lista[min] = aux;
            }
        }

        // Dibuja NPC y jugador de atrás hacia adelante, así el jugador pasa "por detrás" de un NPC
        private void DibujarOrdenado()
        {
            List<KeyValuePair<float, Action>> objetos = new List<KeyValuePair<float, Action>>();

            foreach (NPC npc in npcs)
            {
                NPC n = npc;
                objetos.Add(new KeyValuePair<float, Action>(
                    n.posicion.Y + n.alto,
                    () => n.Draw(_spriteBatch)));
            }

            objetos.Add(new KeyValuePair<float, Action>(
                jugador.Hitbox.Bottom,
                () => jugador.Draw(_spriteBatch)));

            OrdenarPorSeleccion(objetos);

            foreach (KeyValuePair<float, Action> objeto in objetos)
                objeto.Value();
        }

        protected override void Draw(GameTime gameTime)
        {
            _virtual.Comenzar();
            GraphicsDevice.Clear(Color.Black);

            if (_estadoActual == EstadoJuego.Menu)
            {
                _spriteBatch.Begin();
                _menu.Dibujar(_spriteBatch);
                _spriteBatch.End();
            }
            else if (_estadoActual == EstadoJuego.Logros)
            {
                _spriteBatch.Begin();
                _logros.Dibujar(_spriteBatch);
                _spriteBatch.End();
            }
            else if (_estadoActual == EstadoJuego.Jugando)
            {
                // 1) MUNDO: con la cámara
                _spriteBatch.Begin(transformMatrix: _camara.Transformacion);

                _mapa.Dibujar(_spriteBatch);

                // NPC y jugador ordenados por profundidad (si Ruee fue derrotada, ya no está en la lista)
                DibujarOrdenado();

                if (mostrarColisiones)
                {
                    _spriteBatch.Draw(texturaColision, new Rectangle(0, 0, 1000, 750), Color.White * 0.6f);
                    _spriteBatch.Draw(pixel, jugador.Hitbox, Color.Blue * 0.8f);

                    foreach (NPC npc in npcs)
                    {
                        _spriteBatch.Draw(pixel, npc.ZonaInteraccion, Color.Green * 0.3f);  // donde se puede hablar
                        _spriteBatch.Draw(pixel, npc.CajaColision, Color.Red * 0.5f);       // sólido
                    }
                }

                _spriteBatch.End();

                // 2) INTERFAZ: sin cámara, en coordenadas de pantalla
                _spriteBatch.Begin();
                dialogo.Draw(_spriteBatch);

                if (mostrarDev)
                {
                    _spriteBatch.Draw(
                        pixel,
                        new Rectangle(20, 170, 600, panelDev.Count * 28 + 20),
                        Color.Black * 0.75f);

                    for (int i = 0; i < panelDev.Count; i++)
                    {
                        _spriteBatch.DrawString(
                            fuentedialogo,
                            panelDev[i],
                            new Vector2(35, 180 + i * 28),
                            Color.White);
                    }
                }

                _spriteBatch.End();
            }
            else if (_estadoActual == EstadoJuego.Pelea)
            {
                // La pelea se dibuja sin cámara, en coordenadas de pantalla
                _spriteBatch.Begin();
                pelea.Draw(_spriteBatch);
                _spriteBatch.End();
            }
            _virtual.Terminar();
            base.Draw(gameTime);

            if (_estadoActual == EstadoJuego.Jugando)
            {
                Window.Title = "DECISION QUEST";
            }
            else if (_estadoActual == EstadoJuego.Pelea)
            {
                Window.Title = "DECISION QUEST - PELEA: Presiona WASD para moverte!";
            }
            else if (_estadoActual == EstadoJuego.Logros)
            {
                Window.Title = "DECISION QUEST - LOGROS";
            }
        }
    }
}