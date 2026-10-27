using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoDecisionQuest.Objetos
{
    public class Personaje
{
    // ataque vida y defensa son las estadísticas para el personaje
    public int ataque { get; set; }
    public int vida { get; set; }
    public int defensa { get; set; }
    //esto está relacionado con el desplazamiento del personaje, está medida en pixeles por segundo
    public float velocidad_jugador { get; set; } = 300f;
    //Esta es una variable auxiliar que se encarga de guardar el estado en el que se encuentra el personaje (quieto o moviendose a izquierda, derecha, abajo y arriba)
    public Sprite estado { get; set; }
    //todos estos sprites son para cada imagen del personaje, una imagen del pj moviendose a la derecha y viceversa.
    //En este caso el sprite quieto no va a ser unicamente para cuando está quieto el personaje, sino que también para cuando se mueve para arriba y abajo.
    public Sprite spriteparado;
    public Sprite spritederecha;
    public Sprite spriteizquierda;

    public Personaje(int ataque, int vida, int defensa, Sprite quieto, Sprite izquierda, Sprite derecha )
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

    //esta parte de acá es la que se encarga de todo el movimiento del personaje
    public void Update(GameTime gameTime)
    {
        // esto es simplemente para que se lea el teclado
        KeyboardState teclas = Keyboard.GetState();
        // el delta es tiempo real en el que se está moviendo el personaje por cada frame
        //de esta manera conserva la misma velocidad incluso si tenés 300 fps
        //sin el delta la velocidad en pixeles por segundo del personaje estaría medida únicamente por cada 1 frame (pixeles por fotograma)
        //eso provoca que el personaje vaya demasiado rápido
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //esto es otra variable auxiliar que guarda la posición del sprite teniendo en cuenta el estado en el que está
        Vector2 nuevapos = estado.posicion;
        Sprite posicionactual = spriteparado;
        //esto es una secuencia de if's para cada tecla 
        //Ejemplo: Si x tecla está presionada, entonces el sprite se mueve a la derecha
        //estos primeros dos if son para la posición del sprite en las coordenadas X
        if (teclas.IsKeyDown(Keys.Left) || teclas.IsKeyDown(Keys.A))
        {
            //este es un cálculo matemático de MRU (movimiento rectilíneo uniforme) donde la distancia = velocidad x tiempo
            nuevapos.X -= velocidad_jugador * delta;
            posicionactual = spriteizquierda;
        }
        if (teclas.IsKeyDown(Keys.Right) || teclas.IsKeyDown(Keys.D))
        {
            nuevapos.X += velocidad_jugador * delta;
            posicionactual = spritederecha;
        }

        //Lo mismo pero para la posición en Y
        if (teclas.IsKeyDown(Keys.Up) || teclas.IsKeyDown(Keys.W))
        {
            nuevapos.Y -= velocidad_jugador * delta;
            posicionactual = spriteparado;
        }
        if (teclas.IsKeyDown(Keys.Down) || teclas.IsKeyDown(Keys.S))
        {
            nuevapos.Y += velocidad_jugador * delta;
            posicionactual = spriteparado;
        }

        //Acá estoy tomando la posicion actual del sprite y la estoy guardando dentro del sprite (para que se mueva)
        posicionactual.posicion = nuevapos;
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
