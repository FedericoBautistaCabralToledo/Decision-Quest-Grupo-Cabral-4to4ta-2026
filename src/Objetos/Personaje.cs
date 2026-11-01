using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JuegoDecisionQuest.Objetos
{
    public class Personaje
    {
        // ataque vida y defensa son las estadísticas para el personaje
        public int ataque { get; set; }
        public int vida { get; set; }
        public int defensa { get; set; }

        // velocidad del desplazamiento, en pixeles por segundo
        public float velocidad_jugador { get; set; } = 300f;

        // sprite que se está mostrando ahora mismo (quieto, izquierda o derecha)
        public Sprite estado { get; set; }
        public Sprite spriteparado;
        public Sprite spritederecha;
        public Sprite spriteizquierda;

        // Zona de colisión del personaje: solo sus PIES, no todo el cuerpo.
        // La usamos también en Game1 para dibujarla al depurar.
        public Rectangle Hitbox => CrearHitbox(estado.posicion, estado);

        public Personaje(int ataque, int vida, int defensa, Sprite quieto, Sprite izquierda, Sprite derecha)
        {
            this.ataque = ataque;
            this.vida = vida;
            this.defensa = defensa;
            this.spriteparado = quieto;
            this.spriteizquierda = izquierda;
            this.spritederecha = derecha;
            // el estado inicial es que esté quieto
            estado = this.spriteparado;
        }

        // Crea el rectángulo de los pies para una posición dada
        public Rectangle CrearHitbox(Vector2 pos, Sprite sprite)
        {
            int anchoCaja = 28;
            int altoCaja = 16;

            return new Rectangle(
                (int)pos.X + (sprite.ancho - anchoCaja) / 2,   // centrada horizontalmente
                (int)pos.Y + sprite.alto - altoCaja,           // pegada a la parte de abajo
                anchoCaja,
                altoCaja);
        }

        // Update ahora recibe las colisiones para poder comprobarlas
        public void Update(GameTime gameTime, Colisiones colisiones)
        {
            KeyboardState teclas = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 1) Leemos las teclas y armamos la dirección (-1, 0 o 1 en cada eje)
            Vector2 direccion = Vector2.Zero;
            Sprite posicionactual = spriteparado;

            if (teclas.IsKeyDown(Keys.Left) || teclas.IsKeyDown(Keys.A))
            {
                direccion.X -= 1;
                posicionactual = spriteizquierda;
            }
            if (teclas.IsKeyDown(Keys.Right) || teclas.IsKeyDown(Keys.D))
            {
                direccion.X += 1;
                posicionactual = spritederecha;
            }
            if (teclas.IsKeyDown(Keys.Up) || teclas.IsKeyDown(Keys.W))
            {
                direccion.Y -= 1;
                if (direccion.X == 0) posicionactual = spriteparado;
            }
            if (teclas.IsKeyDown(Keys.Down) || teclas.IsKeyDown(Keys.S))
            {
                direccion.Y += 1;
                if (direccion.X == 0) posicionactual = spriteparado;
            }

            // Cuánto se mueve en este frame (distancia = velocidad x tiempo)
            Vector2 movimiento = direccion * velocidad_jugador * delta;

            // Movemos y comprobamos colisiones EJE POR EJE.
            // Si choca en un eje, deshacemos solo ese movimiento y el otro eje sigue funcionando,
            // así el personaje se desliza por las paredes en vez de quedarse trabado.
            Vector2 pos = estado.posicion;

            pos.X += movimiento.X;
            if (colisiones.Choca(CrearHitbox(pos, estado)))
                pos.X -= movimiento.X;

            pos.Y += movimiento.Y;
            if (colisiones.Choca(CrearHitbox(pos, estado)))
                pos.Y -= movimiento.Y;

            // 5) Guardamos la posición final en el sprite que corresponde y lo dejamos como estado
            posicionactual.posicion = pos;
            estado = posicionactual;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectanguloDestino = new Rectangle(
                (int)estado.posicion.X,
                (int)estado.posicion.Y,
                estado.ancho,
                estado.alto
                );
            spriteBatch.Draw(estado.textura, rectanguloDestino, Color.White);
        }
    }
}