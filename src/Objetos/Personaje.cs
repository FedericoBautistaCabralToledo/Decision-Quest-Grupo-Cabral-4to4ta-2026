using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoDecisionQuest.Objetos
{
    internal class Personaje
    {
        public int ataque { get; set; }
public int vida { get; set; }
public int defensa { get; set; }
public Sprite sprite { get; set; }
public float velocidad_jugador { get; set; } = 300f;

public Personaje(int ataque, int vida, int defensa, Sprite sprite)
{
    this.ataque = ataque;
    this.vida = vida;
    this.defensa = defensa;
    this.sprite = sprite;
}

public void Update(GameTime gameTime)
{
    KeyboardState teclas = Keyboard.GetState();

    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
    Vector2 nuevapos = sprite.posicion;

    if(teclas.IsKeyDown(Keys.Left) || teclas.IsKeyDown(Keys.A))
    {
        nuevapos.X -= velocidad_jugador * delta;
    }
    if (teclas.IsKeyDown(Keys.Right) || teclas.IsKeyDown(Keys.D))
    {
        nuevapos.X += velocidad_jugador * delta;
    }
    if (teclas.IsKeyDown(Keys.Up) || teclas.IsKeyDown(Keys.W))
    {
        nuevapos.Y -= velocidad_jugador * delta;
    }
    if (teclas.IsKeyDown(Keys.Down) || teclas.IsKeyDown(Keys.S))
    {
        nuevapos.Y += velocidad_jugador * delta;
    }

    sprite.posicion = nuevapos;
}

public void Draw(SpriteBatch spriteBatch)
{
    Rectangle rectanguloDestino = new Rectangle(
        (int)sprite.posicion.X,
        (int)sprite.posicion.Y,
        sprite.ancho,
        sprite.alto
        );
    spriteBatch.Draw(sprite.textura, rectanguloDestino, Color.White);
}
    }
}
