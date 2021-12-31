using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Game_of_Life_2
{
    public partial class Form1 : Form
    {
        Board board = new Board();
        int cellSizeMultiplier = 10;

        bool looping = false;
        private Timer loopTimer;
        bool dragging = false;

        private bool selectingMany = false;
        int selectStartX = 0;
        int selectStartY = 0;

        List<List<bool>> savedBoard = new List<List<bool>>();

        public Form1()
        {
            InitializeComponent();
            Render();
        }


        private void Render()
        {

            using (var bmp = new Bitmap(board.width * cellSizeMultiplier, board.height * cellSizeMultiplier))
            using (var gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(Color.Red))
            {
                gfx.Clear(ColorTranslator.FromHtml("#2f3539"));

                var cellSize = new Size(1 * cellSizeMultiplier - 1, 1 * cellSizeMultiplier - 1);

                for (int width = 0; width < board.height; width++)
                {
                    for (int height = 0; height < board.width; height++)
                    {
                        if (board.board[height][width])
                        {
                            Rectangle cellRect = new Rectangle(new Point(height * cellSizeMultiplier, width * cellSizeMultiplier), cellSize);
                            gfx.FillRectangle(brush, cellRect);
                        }
                    }
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = (Bitmap)bmp.Clone();
            }
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
           /* MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            coordinates = new Point(coordinates.X / cellSizeMultiplier, coordinates.Y / cellSizeMultiplier);
            Console.WriteLine(coordinates);

            switch (e.Button)
            {

                case MouseButtons.Left:

                    board.board[coordinates.X][coordinates.Y] = !board.board[coordinates.X][coordinates.Y];
                    break;

            }

            Render();*/

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {

            switch (e.Button)
            {
                case MouseButtons.Left:
                    board.Tick();
                    Render();
                    break;

            }


        }

        private void Form1_Scroll(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("op");
            if (e.OldValue > e.NewValue)
            {

            }
            else
            {

                // here down
            }


        }


        private void button4_Click(object sender, EventArgs e)
        {
            looping = !looping;

            if (looping)
            {
                decimal speed = numericUpDown1.Value;
                loopTimer = new Timer();
                loopTimer.Tick += new EventHandler(timer_loopEvent);
                loopTimer.Interval = (int)(1000 / speed);
                loopTimer.Start();
                button4.Text = "Stop looping";
            }
            else
            {
                loopTimer.Stop();
                button4.Text = "Start Looping";
            }
        }

        private void timer_loopEvent(object sender, EventArgs e)
        {
            board.Tick();
            Render();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            savedBoard = Board.DeepCopy(board.board);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            board.board = Board.DeepCopy(savedBoard);
            Render();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            coordinates = new Point(coordinates.X / cellSizeMultiplier, coordinates.Y / cellSizeMultiplier);
            Console.WriteLine(coordinates);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (board.board[coordinates.X][coordinates.Y])
                    {
                        board.board[coordinates.X][coordinates.Y] = false;
                    }
                    else
                    {
                        dragging = true;
                    }
                   
                    break;
                case MouseButtons.Right:


                    {
                        selectingMany = true;
                        selectStartX = coordinates.X;
                        selectStartY = coordinates.Y;
                    }
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            coordinates = new Point(coordinates.X / cellSizeMultiplier, coordinates.Y / cellSizeMultiplier);
            Console.WriteLine(coordinates);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    dragging = false;
                    break;
                case MouseButtons.Right:
                    selectingMany = false;
                    break;
            }
            Render();

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            coordinates = new Point(coordinates.X / cellSizeMultiplier, coordinates.Y / cellSizeMultiplier);
            Console.WriteLine(coordinates);


            if (selectingMany)
            {
                int tempStartX = selectStartX;
                int tempStartY = selectStartY;
                int tempEndX = coordinates.X;
                int tempEndY = coordinates.Y;

                if(tempStartX > tempEndX)
                {
                    tempStartX = tempEndX;
                    tempEndX = selectStartX;
                }
                if (tempStartY > tempEndY)
                {
                    tempStartY = tempEndY;
                    tempEndY = selectStartY;
                }

                for (int row = tempStartX; row <= tempEndX; row++)
                {
                    if (row >= board.width)
                        continue;
                    for (int col = tempStartY; col <= tempEndY; col++)
                    {
                        if (col >= board.height)
                            continue;

                        Console.WriteLine(String.Format("x1: {0}, y1: {1}, x2: {2}, y2: {3}", tempStartX, tempStartY, tempEndX, tempEndY));
                        bool enable = true;
                        if (Control.ModifierKeys == Keys.Shift)
                            enable = false;

                        board.board[row][col] = enable;
                    }
                }
            }
            else if(dragging)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:

                        board.board[coordinates.X][coordinates.Y] = true;
                        break;

                }

            }
            Render();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            board.changeSize((int)numericUpDown2.Value, (int)numericUpDown3.Value);
            Render();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}


