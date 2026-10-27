using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoDecisionQuest.Objetos
{
    public class Personaje
{
    public int ataque { get; set; }
    public int vida { get; set; }
    public int defensa { get; set; }
    public float velocidad_jugador { get; set; } = 300f;

    public Sprite estado { get; set; }
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

        estado = this.spriteparado;
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState teclas = Keyboard.GetState();

        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 nuevapos = estado.posicion;
        Sprite posicionactual = spriteparado;

        if (teclas.IsKeyDown(Keys.Left) || teclas.IsKeyDown(Keys.A))
        {
            nuevapos.X -= velocidad_jugador * delta;
            posicionactual = spriteizquierda;
        }
        if (teclas.IsKeyDown(Keys.Right) || teclas.IsKeyDown(Keys.D))
        {
            nuevapos.X += velocidad_jugador * delta;
            posicionactual = spritederecha;
        }
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

        posicionactual.posicion = nuevapos;
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
