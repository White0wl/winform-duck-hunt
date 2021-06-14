using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckHunt
{
    public partial class Form1 : Form
    {
        Duck duck;
        Dog dog;
        Timer timer;
        Image background, costume;

        public int Round
        {
            get
            {
                return round;
            }

            set
            {
                round = value;
                labelRound.Text = round + "";
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
                labelScore.Text = score + "";
            }
        }

        public int Shots
        {
            get
            {
                return shots;
            }

            set
            {
                shots = value;
                pictureBoxShots.Image = new Bitmap(shotsImgs[Shots], pictureBoxShots.Size);
            }
        }

        int round;
        int score;
        int shots;

        List<Image> shotsImgs;
        List<PictureBox> ducks;


        public Form1()
        {
            InitializeComponent();
            InitializeFields();
            FillImages();
        }

        private void FillImages()
        {
            panelRound.BackgroundImage = new Bitmap(Image.FromFile("source/backinfo.png"), panelRound.Size);
            panelScore.BackgroundImage = new Bitmap(Image.FromFile("source/backinfo.png"), panelScore.Size);
            panelDucks.BackgroundImage = new Bitmap(Image.FromFile("source/backinfo.png"), panelDucks.Size);

            foreach (var item in ducks)
            {
                item.Image = new Bitmap(Image.FromFile("source/duck.png"), pictureBox0.Size);
            }

        }

        public void StartRound()
        {
            Shots = 3;
            Round++;
            duck = new Duck(ClientRectangle.Width, ClientRectangle.Height);
        }

        private void InitializeFields()
        {
            Cursor = new Cursor("Crosshair.cur");
            shotsImgs = new List<Image>();
            for (int i = 0; i < 4; i++)
            {
                shotsImgs.Add(Image.FromFile("source/" + i + "shots.png"));
            }

            ducks = new List<PictureBox>();
            ducks.Add(pictureBox0);
            ducks.Add(pictureBox1);
            ducks.Add(pictureBox2);
            ducks.Add(pictureBox3);
            ducks.Add(pictureBox4);
            ducks.Add(pictureBox5);
            ducks.Add(pictureBox6);
            ducks.Add(pictureBox7);
            ducks.Add(pictureBox8);
            ducks.Add(pictureBox9);

            Round = 0;
            StartRound();
            background = Image.FromFile("source/background.png");
            costume = Image.FromFile("source/costume.png");

            Size = background.Size;

            dog = new Dog();
            timer = new Timer();
            timer.Interval = 1000 / 24;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(background, new Point(0, 0));

            duck.DrawDuck(e.Graphics, ClientRectangle.Width, ClientRectangle.Height);


            if (duck.State == DuckState.Fall)
            {
                dog.Fowl(new Point((ClientRectangle.Width - 80) / 2, ClientRectangle.Height));
                Score += 100;
                ducks[round - 1].Image = new Bitmap(Image.FromFile("source/duck2.png"), pictureBox0.Size);
            }
            else if (duck.State == DuckState.FlewAway)
            {
                dog.Laugh(new Point((ClientRectangle.Width - 80) / 2, ClientRectangle.Height));
                background = Image.FromFile("source/background.png");
            }


            if (dog.Moving)
                dog.DrawDog(e.Graphics);
            else if (duck.State == DuckState.None)
            {
                if (round < 10)
                    StartRound();
                else
                {
                    timer.Stop();
                    MessageBox.Show("You Win");
                    Close();
                }
            }

            e.Graphics.DrawImage(costume, new Point(1, 250));



            Text = dog.Location + "";
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (duck.State == DuckState.Moving)
            {

                duck.Shut(new Point(e.X + 16, e.Y + 16));
                Shots--;
                if (Shots <= 0)
                {
                    duck.FlyAway();
                }
            }
        }


    }
}
