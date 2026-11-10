using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JuegoDecisionQuest.Objetos.MecanicasPelea.Jugador;

namespace JuegoDecisionQuest.Objetos.MecanicasPelea.Enemigo
{
    public class AtaqueEnemigo
    {
        private enum Tipo
        {
            Calavera,
            Flecha,
            Rayo
        }

        private struct Proyectil
        {
            public Rectangle Area;
            public Vector2 Velocidad;
            public Tipo Tipo;
            public float TiempoVida;
            public bool Activo;

            public Proyectil(
                Rectangle area,
                Vector2 velocidad,
                Tipo tipo,
                float tiempoVida)
            {
                Area = area;
                Velocidad = velocidad;
                Tipo = tipo;
                TiempoVida = tiempoVida;
                Activo = true;
            }
        }

        // LISTA ENLAZADA: quitar un proyectil en medio de la lista es O(1)
        private LinkedList<Proyectil> proyectiles =
            new LinkedList<Proyectil>();

        // COLA (FIFO): las oleadas de la fase actual. Se saca la siguiente,
        // se lanza, y se vuelve a encolar al final para que el ciclo se repita.
        private Queue<Action<Rectangle>> oleadas =
            new Queue<Action<Rectangle>>();

        // Fase para la que se cargó la cola (0 = todavía no se cargó ninguna)
        private int faseCargada;

        private Random random = new Random();

        private Texture2D calavera;
        private Texture2D flecha;
        private Texture2D monstruoRayo;
        private Texture2D rayo;

        private float tiempoOnda;

        private const float IntervaloOnda = 0.85f;

        public AtaqueEnemigo(
            Texture2D calavera,
            Texture2D flecha,
            Texture2D monstruoRayo,
            Texture2D rayo)
        {
            this.calavera = calavera;
            this.flecha = flecha;
            this.monstruoRayo = monstruoRayo;
            this.rayo = rayo;
        }

        public void Reiniciar()
        {
            proyectiles.Clear();
            oleadas.Clear();
            faseCargada = 0;   // fuerza a recargar el ciclo desde la primera oleada
            tiempoOnda = 0;
        }

        public void Actualizar(
            GameTime gameTime,
            Rectangle arena,
            Rectangle hitboxJugador,
            EstadisticasJugador jugador,
            int fase,
            bool puedeRecibirDanio)
        {
            float delta =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            tiempoOnda += delta;

            if (tiempoOnda >= IntervaloOnda)
            {
                tiempoOnda = 0;
                CrearOnda(arena, fase);
            }

            MoverProyectiles(
                delta,
                arena,
                hitboxJugador,
                jugador,
                puedeRecibirDanio);
        }

        // Carga en la cola el ciclo de oleadas de una fase
        private void PrepararOleadas(int fase)
        {
            oleadas.Clear();

            switch (fase)
            {
                case 1:
                    oleadas.Enqueue(a => LanzarCalaveras(a, 3));
                    oleadas.Enqueue(a => LanzarFlechas(a, 2));
                    oleadas.Enqueue(a => LanzarFlechas(a, 2));
                    break;

                case 2:
                    oleadas.Enqueue(a => LanzarCalaveras(a, 5));
                    oleadas.Enqueue(a => LanzarFlechas(a, 4));
                    oleadas.Enqueue(a => LanzarRayo(a));
                    break;

                case 3:
                    oleadas.Enqueue(a => LanzarCalaveras(a, 6));
                    oleadas.Enqueue(a => LanzarFlechas(a, 5));
                    oleadas.Enqueue(a =>
                    {
                        LanzarRayo(a);
                        LanzarCalaveras(a, 3);
                    });
                    break;
            }

            faseCargada = fase;
        }

        private void CrearOnda(Rectangle arena, int fase)
        {
            if (fase != faseCargada)
                PrepararOleadas(fase);

            if (oleadas.Count == 0)
                return;

            Action<Rectangle> onda = oleadas.Dequeue();
            onda(arena);
            oleadas.Enqueue(onda);   // vuelve al final: el ciclo se repite
        }

        // RECURSIVIDAD: lanza una calavera y se llama a sí misma con una menos
        private void LanzarCalaveras(
            Rectangle arena,
            int cantidad)
        {
            if (cantidad <= 0)
                return;

            int x = random.Next(
                arena.Left + 10,
                arena.Right - 35);

            Rectangle area =
                new Rectangle(x, arena.Top - 35, 34, 34);

            Vector2 velocidad = new Vector2(
                random.Next(-65, 66),
                VelocidadFase(170) + random.Next(0, 70));

            Lanzar(area, velocidad, Tipo.Calavera);

            LanzarCalaveras(arena, cantidad - 1);
        }

        private void LanzarFlechas(
            Rectangle arena,
            int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                int y = random.Next(
                    arena.Top + 10,
                    arena.Bottom - 35);

                bool izquierda = random.Next(2) == 0;

                int x = izquierda
                    ? arena.Left - 45
                    : arena.Right + 10;

                float direccion = izquierda ? 1 : -1;

                Rectangle area =
                    new Rectangle(x, y, 45, 18);

                Vector2 velocidad = new Vector2(
                    direccion * VelocidadFase(170), 0);

                Lanzar(area, velocidad, Tipo.Flecha);
            }
        }

        private void LanzarRayo(Rectangle arena)
        {
            int y = random.Next(
                arena.Top + 35,
                arena.Bottom - 65);

            Rectangle area = new Rectangle(
                arena.Left + 5,
                y,
                arena.Width - 10,
                35);

            Lanzar(area, Vector2.Zero, Tipo.Rayo, 0.8f);
        }

        private void Lanzar(
            Rectangle area,
            Vector2 velocidad,
            Tipo tipo,
            float tiempoVida = 5f)
        {
            proyectiles.AddLast(
                new Proyectil(
                    area,
                    velocidad,
                    tipo,
                    tiempoVida));
        }

        private float VelocidadFase(float velocidadBase)
        {
            return velocidadBase;
        }

        private void MoverProyectiles(
            float delta,
            Rectangle arena,
            Rectangle hitboxJugador,
            EstadisticasJugador jugador,
            bool puedeRecibirDanio)
        {
            LinkedListNode<Proyectil> nodo = proyectiles.First;

            while (nodo != null)
            {
                // Se guarda el siguiente ANTES, porque quizá se elimine este nodo
                LinkedListNode<Proyectil> siguiente = nodo.Next;

                Proyectil p = nodo.Value;

                p.Area.X += (int)(p.Velocidad.X * delta);
                p.Area.Y += (int)(p.Velocidad.Y * delta);

                p.TiempoVida -= delta;

                if (p.TiempoVida <= 0)
                    p.Activo = false;

                if (p.Activo &&
                    puedeRecibirDanio &&
                    p.Area.Intersects(hitboxJugador))
                {
                    jugador.RecibirDanio(Danio(p.Tipo));
                    p.Activo = false;
                }

                if (p.Tipo != Tipo.Rayo &&
                    !arena.Intersects(p.Area))
                {
                    p.Activo = false;
                }

                if (p.Activo)
                    nodo.Value = p;            // struct: hay que volver a asignarlo
                else
                    proyectiles.Remove(nodo);  // eliminación O(1)

                nodo = siguiente;
            }
        }

        private int Danio(Tipo tipo)
        {
            switch (tipo)
            {
                case Tipo.Calavera: return 20;
                case Tipo.Flecha: return 25;
                case Tipo.Rayo: return 35;
                default: return 10;
            }
        }

        public void Dibujar(SpriteBatch sb)
        {
            foreach (Proyectil p in proyectiles)
            {
                Texture2D textura =
                    p.Tipo == Tipo.Calavera ? calavera :
                    p.Tipo == Tipo.Flecha ? flecha : rayo;

                if (p.Tipo == Tipo.Rayo)
                {
                    sb.Draw(
                        monstruoRayo,
                        new Rectangle(
                            p.Area.Left - 65,
                            p.Area.Y - 10,
                            65,
                            55),
                        Color.White);
                }

                sb.Draw(textura, p.Area, Color.White);
            }
        }
    }
}