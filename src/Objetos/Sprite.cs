using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDecisionQuest
{
    public class Sprite
    {
        //esta es la textura, va a guardar la imagen
        public Texture2D textura;
        //Coordenadas de dónde va a estar el sprite, está medido con el plano cartesiano (x, y) en pixeles dentro del juego
        public Vector2 posicion { get; set; }
        //el tamaño del sprite en pixeles
        public int ancho { get; set; }
        public int alto { get; set; }

        public Sprite(Texture2D textura, Vector2 posicion, int ancho, int alto)
        {
            this.textura = textura;
            this.posicion = posicion;
            this.ancho = ancho;
            this.alto = alto;
        }
    }
}
