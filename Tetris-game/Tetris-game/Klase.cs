using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris_game
{
    class Kockica
    {
        private int duzina = 20;
        private Pen olovka = new Pen(Color.Black, 1);
        private SolidBrush boja_ispuni = new SolidBrush(Color.Blue);
        private SolidBrush boja_isprazni = new SolidBrush(Color.White);

        public int X;
        public int Y;
        public int ID;
        public bool ispunjen;

        public Rectangle rect;


        public Kockica(int id, int x, int y)
        {
            X = x;
            Y = y;
            ID = id;
            ispunjen = false;
            rect = new Rectangle(X, Y, duzina, duzina);
        }

        public void nacrtaj_rub(Graphics g)
        {
            g.DrawRectangle(olovka, rect);
        }

        public void ispuni(Graphics g)
        {
            g.FillRectangle(boja_ispuni, rect);
        }

        public void isprazni(Graphics g)
        {
            g.FillRectangle(boja_isprazni, rect);
        }
    }
}
