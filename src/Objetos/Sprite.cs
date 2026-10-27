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
        public Texture2D textura;
        public Vector2 posicion { get; set; }
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
