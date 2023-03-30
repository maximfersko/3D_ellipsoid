using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics
{
    public partial class Form1 : Form
    {

        struct point 
        { 
            public float x, y, z; 
        };
        const int S = 20;
        const int R = 80;
        point[,] points;

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            GetPoints();
        }

        void GetPoints()
        {
            points = new point[S, S];
            for (int i = 0; i < S; ++i)
                for (int j = 0; j < S; ++j)
                {
                    float alpha = (float)(j * 2.0 * Math.PI / S);
                    float beta = (float)(i * 2.0 * Math.PI / S);
                    points[i, j].y = (float)(R * Math.Cos(alpha) * 0.5);
                    points[i, j].x = (float)(R * Math.Sin(alpha) * Math.Sin(beta));
                    points[i, j].z = (float)(R * Math.Sin(alpha) * Math.Cos(beta));
                    //поворот вокруг x

                    //перенос на центр экрана
                    points[i,j].y += pictureBox1.Height / 2;
                    points[i,j].x += pictureBox1.Width / 2;
 
                }
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(pictureBox1.BackColor);
            Pen pen = new Pen(Color.Green, 2);
            for (int i = 0; i < S; i++)
            {
                PointF begin = new PointF(points[i, S - 1].x, points[i, S - 1].y);
                for (int j = 0; j < S; ++j) {
                    PointF next = new PointF(points[i, j].x, points[i, j].y);
                    e.Graphics.DrawLine(pen, begin, next);
                    begin = new PointF(points[i, j].x, points[i, j].y); 
                }
            }

            for (int i = 0; i < S; i++)
            {
                PointF begin = new PointF(points[S - 1, i].x, points[S - 1, i].y);
                for (int j = 0; j < S; j++)
                {
                    PointF next = new PointF(points[j, i].x, points[j, i].y);
                    e.Graphics.DrawLine(pen, begin, next);
                    begin = new PointF(points[j, i].x, points[j, i].y);
                }
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            GetPoints();
            Refresh();
        }

        Point mouse;
        bool click = false;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (click && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                float rotate = (float)(Math.PI / 200);
                if (mouse.Y != e.Y)
                {
                    // поворот по X
                    rotate *= (e.Y - mouse.Y);
                    mouse.X = e.X;
                    mouse.Y = e.Y;
                    for (int i = 0; i < S; ++i)
                        for (int j = 0; j < S; ++j)
                        {

                            point np;
                            points[i, j].x -= pictureBox1.Width / 2;
                            points[i, j].y -= pictureBox1.Height / 2;
                            np.x = points[i, j].x + pictureBox1.Width / 2;
                            np.y = (float)(points[i, j].y * Math.Cos(rotate) - points[i, j].z * Math.Sin(rotate) + pictureBox1.Height / 2);
                            np.z = (float)(points[i, j].y * Math.Sin(rotate) + points[i, j].z * Math.Cos(rotate));
                            points[i, j] = np;
                        }
                }
                if (mouse.X != e.X)
                {
                    // поворот по Y
                    rotate *= (mouse.X - e.X);
                    mouse.X = e.X;
                    mouse.Y = e.Y;
                    for (int i = 0; i < S; ++i)
                        for (int j = 0; j < S; ++j)
                        {

                            point np;
                            points[i, j].x -= pictureBox1.Width / 2;
                            points[i, j].y -= pictureBox1.Height / 2;

                            np.x = (float)(points[i, j].x * Math.Cos(rotate) + points[i, j].z * Math.Sin(rotate) + pictureBox1.Width / 2);
                            np.y = points[i, j].y + pictureBox1.Height / 2;
                            np.z = (float)(-(points[i, j].x * Math.Sin(rotate)) + points[i, j].z * Math.Cos(rotate));
                            points[i, j] = np;
                        }
                }
            }
            if (click && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                float dx = mouse.X - e.X;
                float dy = mouse.Y - e.Y;
                mouse.X = e.X;
                mouse.Y = e.Y;

                for (int i = 0; i < S; ++i)
                    for (int j = 0; j < S; ++j)
                    {
                        points[i, j].x -= dx;
                        points[i, j].y -= dy;
                    }
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            click = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse.X = e.X;
            mouse.Y = e.Y;
            click = true;
        }
    }
}
