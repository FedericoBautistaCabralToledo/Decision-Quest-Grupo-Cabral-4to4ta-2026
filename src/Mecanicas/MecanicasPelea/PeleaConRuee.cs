using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using JuegoDecisionQuest.Objetos.MecanicasPelea.Jugador;
using JuegoDecisionQuest.Objetos.MecanicasPelea.Enemigo;

namespace JuegoDecisionQuest.Objetos.MecanicasPelea
{
    public class PeleaConRuee
    {
        private enum Estado
        {
            Atacar,
            Defensa,
            Decision,
            Victoria,
            Derrota,
            Perdonado
        }

        // Ahora es público para que Game1 pueda guardarlo en su pila de decisiones
        public enum Decision
        {
            Ninguna,
            Matar,
            Perdonar
        }

        private readonly Texture2D pixel;
        private readonly Texture2D corazon;
        private readonly Texture2D ruee;
        private readonly SpriteFont fuente;

        private readonly Rectangle arena =
            new Rectangle(250, 270, 500, 300);

        private readonly Rectangle botonMatar =
            new Rectangle(250, 620, 200, 55);

        private readonly Rectangle botonPerdonar =
            new Rectangle(550, 620, 200, 55);

        private EstadisticasJugador jugador;
        private EstadisticasEnemigo enemigo;

        private AtaqueJugador ataqueJugador;
        private AtaqueEnemigo ataqueEnemigo;

        private Estado estado = Estado.Atacar;

        private Vector2 posicionCorazon =
            new Vector2(492, 405);

        private float tiempoDefensa;
        private float invulnerabilidad;

        private KeyboardState tecladoAnterior;
        private MouseState ratonAnterior;

        private const float DuracionDefensa = 14f;
        private const float VelocidadCorazon = 260f;

        public bool Victoria => estado == Estado.Victoria;
        public bool Derrota => estado == Estado.Derrota;
        public bool Perdonado => estado == Estado.Perdonado;

        public bool Terminado =>
            Victoria || Derrota || Perdonado;

        // Qué eligió el jugador al final (Ninguna si perdió antes de decidir)
        public Decision DecisionTomada { get; private set; } = Decision.Ninguna;

        private Rectangle HitboxCorazon =>
            new Rectangle(
                (int)posicionCorazon.X + 3,
                (int)posicionCorazon.Y + 3,
                18,
                18);

        public PeleaConRuee(
            GraphicsDevice dispositivo,
            SpriteFont fuente,
            Texture2D corazon,
            Texture2D ruee,
            Texture2D calavera,
            Texture2D flecha,
            Texture2D monstruoRayo,
            Texture2D rayo)
        {
            this.fuente = fuente;
            this.corazon = corazon;
            this.ruee = ruee;

            pixel = new Texture2D(dispositivo, 1, 1);
            pixel.SetData(new[] { Color.White });

            jugador = new EstadisticasJugador();
            enemigo = new EstadisticasEnemigo();

            ataqueJugador = new AtaqueJugador();

            ataqueEnemigo = new AtaqueEnemigo(
                calavera,
                flecha,
                monstruoRayo,
                rayo);
        }

        public void Update(GameTime gameTime, MouseState raton)
        {
            if (Terminado)
                return;

            float delta =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState teclado = Keyboard.GetState();

            invulnerabilidad =
                MathHelper.Max(0, invulnerabilidad - delta);

            if (estado == Estado.Atacar)
            {
                if (ataqueJugador.Actualizar(teclado, raton))
                    Atacar();
            }
            else if (estado == Estado.Defensa)
            {
                ActualizarDefensa(gameTime, teclado);
            }
            else if (estado == Estado.Decision)
            {
                ActualizarDecision(teclado, raton);
            }

            tecladoAnterior = teclado;
            ratonAnterior = raton;
        }

        private void Atacar()
        {
            if (enemigo.EstaDebilitado())
            {
                estado = Estado.Decision;
                return;
            }

            enemigo.RecibirDanio(jugador.Ataque);

            if (enemigo.EstaDebilitado())
            {
                estado = Estado.Decision;
                return;
            }

            IniciarDefensa();
        }

        private void IniciarDefensa()
        {
            estado = Estado.Defensa;

            tiempoDefensa = 0;
            invulnerabilidad = 0;

            posicionCorazon =
                new Vector2(arena.Center.X, arena.Center.Y);

            ataqueEnemigo.Reiniciar();
        }

        private void ActualizarDefensa(
            GameTime gameTime,
            KeyboardState teclado)
        {
            float delta =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            tiempoDefensa += delta;

            MoverCorazon(delta, teclado);

            ataqueEnemigo.Actualizar(
                gameTime,
                arena,
                HitboxCorazon,
                jugador,
                enemigo.Fase,
                invulnerabilidad <= 0);

            if (!jugador.EstaVivo())
            {
                estado = Estado.Derrota;
                return;
            }

            if (tiempoDefensa >= DuracionDefensa)
            {
                enemigo.CompletarRonda();
                ataqueEnemigo.Reiniciar();
                estado = Estado.Atacar;
            }
        }

        private void MoverCorazon(
            float delta,
            KeyboardState teclado)
        {
            Vector2 movimiento = Vector2.Zero;

            movimiento.X =
                (teclado.IsKeyDown(Keys.D) ? 1 : 0) -
                (teclado.IsKeyDown(Keys.A) ? 1 : 0);

            movimiento.Y =
                (teclado.IsKeyDown(Keys.S) ? 1 : 0) -
                (teclado.IsKeyDown(Keys.W) ? 1 : 0);

            if (movimiento != Vector2.Zero)
                movimiento.Normalize();

            posicionCorazon +=
                movimiento * VelocidadCorazon * delta;

            posicionCorazon.X = MathHelper.Clamp(
                posicionCorazon.X,
                arena.Left,
                arena.Right - 24);

            posicionCorazon.Y = MathHelper.Clamp(
                posicionCorazon.Y,
                arena.Top,
                arena.Bottom - 24);
        }

        private void ActualizarDecision(
            KeyboardState teclado,
            MouseState raton)
        {
            bool enter = teclado.IsKeyDown(Keys.Enter) &&
                         tecladoAnterior.IsKeyUp(Keys.Enter);

            bool clickMatar =
                botonMatar.Contains(raton.Position) &&
                raton.LeftButton == ButtonState.Pressed &&
                ratonAnterior.LeftButton == ButtonState.Released;

            bool clickPerdonar =
                botonPerdonar.Contains(raton.Position) &&
                raton.LeftButton == ButtonState.Pressed &&
                ratonAnterior.LeftButton == ButtonState.Released;

            if (clickMatar || (enter && teclado.IsKeyDown(Keys.Left)))
            {
                DecisionTomada = Decision.Matar;
                estado = Estado.Victoria;
            }
            else if (clickPerdonar ||
                     (enter && teclado.IsKeyDown(Keys.Right)))
            {
                DecisionTomada = Decision.Perdonar;
                estado = Estado.Perdonado;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                pixel,
                new Rectangle(0, 0, 1000, 750),
                Color.Black);

            DibujarRuee(sb);
            DibujarVida(sb);
            DibujarArena(sb);

            if (estado == Estado.Defensa)
                ataqueEnemigo.Dibujar(sb);

            if (estado == Estado.Defensa)
                DibujarCorazon(sb);

            DibujarInterfaz(sb);
        }

        private void DibujarRuee(SpriteBatch sb)
        {
            sb.Draw(
                ruee,
                new Rectangle(455, 45, 90, 110),
                Color.White);

            Texto(sb, "RUEE", 465, 175, Color.White);

            Texto(
                sb,
                "FASE " + enemigo.Fase,
                465,
                205,
                Color.Yellow);
        }

        private void DibujarVida(SpriteBatch sb)
        {
            Texto(sb, "RUEE", 250, 235, Color.White);

            Barra(
                sb, 320, 235, 430,
                enemigo.Vida,
                enemigo.VidaMaxima,
                Color.Red);

            Texto(sb, "VIDA", 250, 590, Color.White);

            Barra(
                sb, 310, 590, 220,
                jugador.Vida,
                jugador.VidaMaxima,
                Color.LimeGreen);
        }

        private void Barra(
            SpriteBatch sb,
            int x,
            int y,
            int ancho,
            int vida,
            int maxima,
            Color color)
        {
            sb.Draw(
                pixel,
                new Rectangle(x, y, ancho, 16),
                Color.DarkRed);

            int relleno = maxima > 0
                ? MathHelper.Clamp(ancho * vida / maxima, 0, ancho)
                : 0;

            sb.Draw(
                pixel,
                new Rectangle(x, y, relleno, 16),
                color);
        }

        private void DibujarArena(SpriteBatch sb)
        {
            sb.Draw(pixel, arena, Color.White);

            sb.Draw(
                pixel,
                new Rectangle(
                    arena.X + 4,
                    arena.Y + 4,
                    arena.Width - 8,
                    arena.Height - 8),
                Color.Black);
        }

        private void DibujarCorazon(SpriteBatch sb)
        {
            if (invulnerabilidad <= 0 ||
                (int)(invulnerabilidad * 12) % 2 == 0)
            {
                sb.Draw(
                    corazon,
                    new Rectangle(
                        (int)posicionCorazon.X,
                        (int)posicionCorazon.Y,
                        24,
                        24),
                    Color.White);
            }
        }

        private void DibujarInterfaz(SpriteBatch sb)
        {
            if (estado == Estado.Atacar)
            {
                sb.Draw(
                    pixel,
                    ataqueJugador.BotonAtacar,
                    Color.Orange);

                Texto(sb, "ATACAR", 460, 637, Color.Black);
                Texto(sb, "ENTER o clic para atacar",
                    385, 690, Color.White);
            }
            else if (estado == Estado.Defensa)
            {
                Texto(
                    sb,
                    "ESQUIVA LOS ATAQUES - WASD",
                    365, 640, Color.White);
            }
            else if (estado == Estado.Decision)
            {
                Texto(
                    sb,
                    "RUEE ESTA DEBILITADO",
                    390, 580, Color.Yellow);

                sb.Draw(pixel, botonMatar, Color.DarkRed);
                sb.Draw(pixel, botonPerdonar, Color.DarkGreen);

                Texto(sb, "ATACAR", 310, 637, Color.White);
                Texto(sb, "PERDONAR", 605, 637, Color.White);
            }
            else if (Victoria)
            {
                Texto(sb, "RUEE FUE DERROTADO",
                    370, 350, Color.Red);
            }
            else if (Perdonado)
            {
                Texto(sb, "RUEE FUE PERDONADO",
                    370, 350, Color.LimeGreen);
            }
            else if (Derrota)
            {
                Texto(sb, "DERROTA",
                    430, 350, Color.Red);
            }
        }

        private const float EscalaTexto = 0.5f;

        private void Texto(
            SpriteBatch sb,
            string texto,
            int x,
            int y,
            Color color)
        {
            sb.DrawString(
                fuente,
                texto,
                new Vector2(x, y),
                color,
                0f,
                Vector2.Zero,
                EscalaTexto,
                SpriteEffects.None,
                0f);
        }
    }
}