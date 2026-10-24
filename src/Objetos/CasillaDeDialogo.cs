using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class CasillaDialogo
{
    // ───────────── Recursos y configuración ─────────────

    //Esto lo estoy escribiendo yo fede, el "readonly" sirve para que la varaiable no pueda ser reasignada
    //pero que si se pueda modificar el contenido, osea, no se puede hacer "_fuente = OtraCosa"
    //El resto del codigo te lo explica la IA, igual no usa nada que no vimos, ta facil de entender

    // Fuente con la que se dibuja el texto (viene del Content Pipeline)
    private readonly SpriteFont _fuente;

    // Textura de 1x1 píxel blanco; la estiramos para dibujar rectángulos de color
    private readonly Texture2D _pixel;

    // Posición y tamaño de la caja en pantalla
    private readonly Rectangle _area;

    // Espacio entre el borde de la caja y el texto
    private const int Margen = 16;

    // Velocidad del efecto "máquina de escribir" (letras por segundo)
    private const float LetrasPorSegundo = 40f;

    // ───────────── Estado del diálogo ─────────────

    // Texto completo, ya con los saltos de línea aplicados
    private string _texto = "";

    // Cuántas letras se están mostrando ahora mismo.
    // Es float para poder sumar fracciones en cada frame.
    private float _letrasVisibles;

    // Estado del teclado en el frame anterior.
    // Sirve para detectar el momento exacto en que se pulsa una tecla.
    private KeyboardState _tecladoAnterior;

    // Indica si la caja está visible y activa
    public bool EstaAbierta { get; private set; }

    // Indica si ya se escribió todo el texto
    public bool TextoCompleto => _letrasVisibles >= _texto.Length;

    // ───────────── Constructor ─────────────

    public CasillaDialogo(GraphicsDevice dispositivoGrafico, SpriteFont fuente, Rectangle area)
    {
        _fuente = fuente;
        _area = area;

        // Creamos una textura de 1x1 y la rellenamos de blanco.
        // Luego la dibujamos estirada para hacer el fondo y el borde.
        _pixel = new Texture2D(dispositivoGrafico, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    // ───────────── Mostrar un texto nuevo ─────────────

    public void Mostrar(string texto)
    {
        // Ajustamos el texto al ancho de la caja (menos los márgenes de cada lado)
        _texto = AjustarTexto(texto, _area.Width - Margen * 2);

        // Reiniciamos el efecto de escritura desde cero
        _letrasVisibles = 0;

        // Hacemos visible la caja
        EstaAbierta = true;
    }

    // ───────────── Lógica (se llama en cada frame) ─────────────

    public void Actualizar(GameTime gameTime)
    {
        // Si la caja está cerrada no hay nada que actualizar
        if (!EstaAbierta) return;

        // Leemos el teclado actual
        var teclado = Keyboard.GetState();

        // "Pulsó Enter" = ahora está presionada Y en el frame anterior no lo estaba.
        // Así evitamos que mantener la tecla dispare la acción muchas veces.
        bool pulsoEnter = teclado.IsKeyDown(Keys.Enter) && _tecladoAnterior.IsKeyUp(Keys.Enter);

        // Guardamos el estado para compararlo en el siguiente frame
        _tecladoAnterior = teclado;

        if (!TextoCompleto)
        {
            // Avanzamos el texto según el tiempo transcurrido desde el último frame.
            // Usar el tiempo (y no los frames) hace que la velocidad sea igual en cualquier PC.
            _letrasVisibles += LetrasPorSegundo * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Si el jugador pulsa Enter mientras se escribe, mostramos todo de golpe
            if (pulsoEnter)
                _letrasVisibles = _texto.Length;
        }
        else if (pulsoEnter)
        {
            // El texto ya estaba completo y el jugador pulsa Enter: cerramos la caja.
            // Aquí es donde más adelante pasarías a la siguiente línea de diálogo.
            EstaAbierta = false;
        }
    }

    // ───────────── Dibujo (se llama en cada frame) ─────────────

    public void Dibujar(SpriteBatch spriteBatch)
    {
        // Si está cerrada, no dibujamos nada
        if (!EstaAbierta) return;

        // 1) Borde: un rectángulo blanco del tamaño total de la caja
        spriteBatch.Draw(_pixel, _area, Color.White);

        // 2) Fondo: un rectángulo más pequeño encima (3 px menos por lado).
        //    Lo que queda visible del rectángulo blanco hace de borde.
        var interior = new Rectangle(_area.X + 3, _area.Y + 3, _area.Width - 6, _area.Height - 6);
        spriteBatch.Draw(_pixel, interior, new Color(20, 20, 40, 230)); // azul oscuro semitransparente

        // 3) Texto: solo las primeras N letras, para lograr el efecto de escritura.
        //    Math.Min evita pasarnos del largo real del texto.
        int cantidad = Math.Min((int)_letrasVisibles, _texto.Length);
        string textoVisible = _texto.Substring(0, cantidad);

        spriteBatch.DrawString(
            _fuente,
            textoVisible,
            new Vector2(_area.X + Margen, _area.Y + Margen), // esquina superior izquierda del texto
            Color.White);
    }

    // ───────────── Salto de línea automático ─────────────

    // MonoGame no corta el texto solo, así que lo hacemos a mano:
    // vamos agregando palabras y, cuando una no cabe, insertamos un '\n'.
    private string AjustarTexto(string texto, float anchoMaximo)
    {
        var resultado = new StringBuilder();

        // Ancho acumulado de la línea actual
        float anchoLinea = 0;

        // Ancho de un espacio, para contarlo entre palabras
        float anchoEspacio = _fuente.MeasureString(" ").X;

        foreach (var palabra in texto.Split(' '))
        {
            // Medimos cuánto ocupa la palabra con esta fuente
            float anchoPalabra = _fuente.MeasureString(palabra).X;

            // Si no cabe en la línea actual (y la línea no está vacía), pasamos a la siguiente
            if (anchoLinea + anchoPalabra > anchoMaximo && anchoLinea > 0)
            {
                resultado.Append('\n');
                anchoLinea = 0;
            }

            // Agregamos la palabra y un espacio, y sumamos su ancho a la línea
            resultado.Append(palabra).Append(' ');
            anchoLinea += anchoPalabra + anchoEspacio;
        }

        // TrimEnd quita el espacio sobrante del final
        return resultado.ToString().TrimEnd();
    }
}
