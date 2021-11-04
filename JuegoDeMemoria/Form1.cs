using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JuegoDeMemoria
{
    //Nombre: José Daniel Medrano Guadamuz
    public partial class Form1 : Form
    {
        PictureBox cartaEscogida1;
        PictureBox cartaEscogida2;
        //Atributo utilizado para identificar si es la primera o segunda carta que se le da vuelta, esto por turno.
        int movimientosJugador;
        //Puntos de los jugadores, si un jugador hace una pareja de cartas, entonces gana 1 punto.
        int puntosJugador1;
        int puntosJugador2;
        //Son dos jugadores, por lo tanto, son dos turno. El 1 es para el jugador 1 y el 2 para el jugador 2.
        int turno;
        //Atributo utilizado en Verificar() para evitar que se haga algo mientras el timer esta activo.
        bool juegoPausado;
        public Form1()
        {
            InitializeComponent();
            movimientosJugador = 0;
            puntosJugador1 = 0;
            puntosJugador2 = 0;
            turno = 1;
            juegoPausado = false;
            //Es el layout donde se encuentra el label que indica el ganador o si es un empate.
            layoutEstado.Visible = false;
        }

        public void CambiarImagen(PictureBox carta)
        {
            /*Cada carta tiene un tag del 1 al 12. 
             * Solo pueden haber dos cartas con un mismo número. 
             * Solo hay una carta que no tiene pareja y posee el tag 13.
             * 
             * Con tags se puede llegar a manipular las cartas con más facilidad con una matriz
             * y generar, por ejemplo, las cartas de manera aleatoria.
             */
            int tag = Convert.ToInt32(carta.Tag);

            switch (tag)
            {
                case 1:
                    carta.Image = new Bitmap("Imagenes/ca.png");
                    break;
                case 2:
                    carta.Image = new Bitmap("Imagenes/c2.png");
                    break;
                case 3:
                    carta.Image = new Bitmap("Imagenes/c3.png");
                    break;
                case 4:
                    carta.Image = new Bitmap("Imagenes/c4.png");
                    break;
                case 5:
                    carta.Image = new Bitmap("Imagenes/c5.png");
                    break;
                case 6:
                    carta.Image = new Bitmap("Imagenes/c6.png");
                    break;
                case 7:
                    carta.Image = new Bitmap("Imagenes/c7.png");
                    break;
                case 8:
                    carta.Image = new Bitmap("Imagenes/c8.png");
                    break;
                case 9:
                    carta.Image = new Bitmap("Imagenes/c9.png");
                    break;
                case 10:
                    carta.Image = new Bitmap("Imagenes/cj.png");
                    break;
                case 11:
                    carta.Image = new Bitmap("Imagenes/ck.png");
                    break;
                case 12:
                    carta.Image = new Bitmap("Imagenes/cq.png");
                    break;
                case 13:
                    carta.Image = new Bitmap("Imagenes/ha.png");
                    break;
            }
        }
        public bool EsPrimerMovimiento()
        {
            return movimientosJugador == 0;
        }
        public bool EsSegundoMovimiento()
        {
            return movimientosJugador == 1;
        }
        public void IncrementarMovimientos()
        {
            movimientosJugador++;
        }
        public void ReiniciarMovimientos()
        {
            movimientosJugador = 0;
        }
        public bool SonCartasIguales()
        {   
            //Si es el jugador selecciona la misma carta dos veces, se devolvera false.
            if (cartaEscogida1 != cartaEscogida2)
                return Convert.ToInt32(cartaEscogida1.Tag) == Convert.ToInt32(cartaEscogida2.Tag);
            return false;
        }
        public void DesactivarCartasIguales()
        {
            //Si las dos cartas escogidas fueron iguales, entonces se desactivan para que no sean verificadas en el metodo Verificar().
            cartaEscogida1.Enabled = false;
            cartaEscogida2.Enabled = false;
        }

        public void CambiarTurno()
        {
            //Si el turno esta en 1, lo cambia a 2.
            if (turno == 1)
            {
                turno = 2;
            }
            //Si el turno esta en 2, lo cambia a 1.
            else if (turno == 2)
            {
                turno = 1;
            }
        }

        public void Verificar(PictureBox carta)
        {
            //Si el timer se activo, el juego se "pausa". Esto para que no manipule otras cartas mientras se dan vuelta las impares escogidas.
            if (juegoPausado)
                return;
            //Si la carta esta desactivada significa que la misma y su pareja ya habian sido encontradas.
            if (carta.Enabled == false)
                return;
            //Si en un turno X el jugador le da la vuelta a la primera carta, entonces se le da vuelta a esta y se guarda la referencia del pictureBox en cartaEscogida1.
            if (EsPrimerMovimiento())
            {
                IncrementarMovimientos();
                cartaEscogida1 = carta;
                CambiarImagen(carta);

            }
            //Si en un turno X el jugador le da la vuelta a la segunda carta, entonces se le da vuelta a esta y se guarda la referencia del pictureBox en cartaEscogida2, despues se procede a comparar las cartas.
            else if (EsSegundoMovimiento())
            {
                ReiniciarMovimientos();
                cartaEscogida2 = carta;
                CambiarImagen(carta);
                //Si las cartas son iguales, entonces, dependiendo del turno, se le aumenta puntos a un jugador.
                if (SonCartasIguales() && turno == 1)
                {
                    puntosJugador1++;
                    labelJugador1.Text = puntosJugador1.ToString();
                    CambiarTurno();

                    DesactivarCartasIguales();

                }
                else if (SonCartasIguales() && turno == 2)
                {
                    puntosJugador2++;
                    labelJugador2.Text = puntosJugador2.ToString();
                    CambiarTurno();

                    DesactivarCartasIguales();
                } 
                else
                {
                    CambiarTurno();
                    //Se "pausa" el juego para que el jugador no manipule otras cartas mientras se le da vuelta a las otras. Esto funciona en conjunto con el código en la linea 140.
                    juegoPausado = true;
                    //Si las cartas no son iguales entonces se le dan vuelta otra vez, pero para que eso pasa hay un pequeño lapso de tiempo en el que las cartas se quedan volteadas.
                    timer1.Start();
                }
                //Verifica si termino el juego <==========================
                EsGameOver();
                //Se actualiza el label del turno.
                labelTurno.Text = turno.ToString();
            }
        }

        private void Carta_Click(object sender, EventArgs e)
        {

            //Se obtiene el PictureBox correspondiente con el sender para despues verificarlo.
            PictureBox carta = sender as PictureBox;
            Verificar(carta);
        }

        private void EsGameOver()
        {
            //Debido a que solo existen 12 pares, eso significa que para que un jugador gane, este necesita encontrar 7 pares de primero.
            //En el caso en el que ambos jugadores alcancen 6 puntos, entonces se considera un empate.
            if (puntosJugador1 == 6 && puntosJugador2 == 6)
            {
                labelEstado.Text = "¡Es un empate!";
                layoutEstado.Size = new Size(156, 24);
                layoutEstado.Visible = true;
            }
            else if (puntosJugador1 == 7) 
            {
                labelEstado.Text = "El jugador 1 ha ganado por alcanzar la mayoria de puntos primero.";
                layoutEstado.Size = new Size(156, 72);
                layoutEstado.Visible = true;
            }
            else if (puntosJugador2 == 7)
            {
                labelEstado.Text = "El jugador 2 ha ganado por alcanzar la mayoria de puntos primero.";
                layoutEstado.Size = new Size(156, 72);
                layoutEstado.Visible = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            //Se les da la vuelta a las cartas.
            cartaEscogida1.Image = new Bitmap("Imagenes/front.png");
            cartaEscogida2.Image = new Bitmap("Imagenes/front.png");
            //Como ya se le dio vuelta a las cartas, ya no es neceseario que el juego este "pausado".
            juegoPausado = false;
        }

        private void buttonSalir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Salir?", "Salir", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
