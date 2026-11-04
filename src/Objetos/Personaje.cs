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

       //Esta es una variable auxiliar que se encarga de guardar el estado en el que se encuentra el sprite (quieto o moviendose a izquierda, derecha, abajo y arriba)
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
            //lo que le estoy diciendo acá es que su estado inicial sea que esté quieto.
            estado = this.spriteparado;
        }

        // Crea el rectángulo de los pies para una posición dada
        public Rectangle CrearHitbox(Vector2 pos, Sprite sprite)
        {
            int anchoCaja = 12;
            int altoCaja = 10;

            return new Rectangle(
                (int)pos.X + (sprite.ancho - anchoCaja) / 2,   // centrada horizontalmente
                (int)pos.Y + sprite.alto - altoCaja,           // pegada a la parte de abajo
                anchoCaja,
                altoCaja);
        }

        // Update ahora recibe las colisiones para poder comprobarlas
        public void Update(GameTime gameTime, Colisiones colisiones)
        {
            // esto es simplemente para que se lea el teclado
            KeyboardState teclas = Keyboard.GetState();
            // el delta es tiempo real en el que se está moviendo el personaje por cada frame
            //de esta manera conserva la misma velocidad incluso si tenés 300 fps
            //sin el delta la velocidad en pixeles por segundo del personaje estaría medida únicamente por cada 1 frame (pixeles por fotograma)
            //eso provoca que el personaje vaya demasiado rápido
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

            //Acá estoy tomando la posicion actual del sprite y la estoy guardando dentro del sprite (para que se mueva)
            posicionactual.posicion = pos;
            //esto es para que el draw pueda saber que sprite dibujar en pantalla
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
