using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO.Pipelines;
using System.Text;

public class CasillaDialogo
{
    Texture2D textura;
    Rectangle caja;
    SpriteFont fuente;

    bool visible = false;

    string nombre = "Fernando";
    string texto = "Hola, soy un npc";

    public CasillaDialogo(Texture2D textura, Rectangle caja, SpriteFont fuente)
    {
        this.textura = textura;
        this.caja = caja;
        this.fuente = fuente;
    }

    public void Update()
    {
        KeyboardState teclado = Keyboard.GetState();

        if (teclado.IsKeyDown(Keys.E))
            visible = true;

        if (teclado.IsKeyDown(Keys.Q))
            visible = false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        
        if (visible)
        {
            
            spriteBatch.Draw(textura, caja, Color.Black);

            Vector2 posicion = new Vector2(caja.X + 15, caja.Y + 12);
            Vector2 posicion2 = new Vector2(caja.X + 15, caja.Y + 45);
            spriteBatch.DrawString(fuente, nombre, posicion, Color.Yellow);
            spriteBatch.DrawString(fuente, texto, posicion2, Color.White);
            
        }
        
    }
}