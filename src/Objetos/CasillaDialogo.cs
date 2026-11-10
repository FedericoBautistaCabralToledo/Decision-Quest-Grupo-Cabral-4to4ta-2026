using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

public class CasillaDialogo
{
    Texture2D textura;
    Rectangle caja;
    SpriteFont fuente;

    string nombre = "";
    string texto = "";

    // Game1 consulta esto para saber si hay un diálogo abierto
    public bool Visible { get; private set; }

    public CasillaDialogo(Texture2D textura, Rectangle caja, SpriteFont fuente)
    {
        this.textura = textura;
        this.caja = caja;
        this.fuente = fuente;
    }

    // Abre el diálogo con el nombre y el texto del NPC con el que se habla
    public void Mostrar(string nombre, string texto)
    {
        this.nombre = nombre;
        this.texto = AjustarTexto(texto, caja.Width - 30);
        Visible = true;
    }

    public void Ocultar()
    {
        Visible = false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Visible) return;

        spriteBatch.Draw(textura, caja, Color.Black);
        spriteBatch.DrawString(fuente, nombre, new Vector2(caja.X + 15, caja.Y + 12), Color.Yellow);
        spriteBatch.DrawString(fuente, texto, new Vector2(caja.X + 15, caja.Y + 45), Color.White);
    }

    // Inserta saltos de línea cuando una palabra ya no cabe en el ancho disponible
    private string AjustarTexto(string texto, float anchoMaximo)
    {
        var resultado = new StringBuilder();
        float anchoLinea = 0;
        float anchoEspacio = fuente.MeasureString(" ").X;

        foreach (string palabra in texto.Split(' '))
        {
            float anchoPalabra = fuente.MeasureString(palabra).X;

            if (anchoLinea + anchoPalabra > anchoMaximo && anchoLinea > 0)
            {
                resultado.Append('\n');
                anchoLinea = 0;
            }

            resultado.Append(palabra).Append(' ');
            anchoLinea += anchoPalabra + anchoEspacio;
        }

        return resultado.ToString().TrimEnd();
    }
}