using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using MatrixCalculus;

namespace Ellipse
{

    public partial class Form1 : Form
    {
        int Xmean;
        int Ymean;
        
        public struct CurvePoint
        {
            public int X;
            public int Y;
        }

        CurvePoint[] _CurvePoints=new CurvePoint[50];

        int nPoints=0;

        public Form1()
        {
            InitializeComponent();
            this.MouseClick += mouseClick;
        }

 
        protected override void OnPaint(PaintEventArgs e)
        {
            //Rotate quadrant, use Cartesian
            //e.Graphics.ScaleTransform(1.0f, -1.0f);
            //e.Graphics.TranslateTransform(0, -this.ClientRectangle.Height);

            //System.Drawing.Graphics graphics = this.CreateGraphics();
            //e.Graphics.TranslateTransform(10, 10);

            //Adjust coordinate system to Cartesian
            //Matrix rotmatrix = new Matrix();
            //rotmatrix.RotateAt(-90, new PointF(0,0));
            //graphics.Transform = rotmatrix;
            //graphics.TranslateTransform(-this.ClientRectangle.Height +10, 10);

            //Draw axis lines
            //e.Graphics.DrawLine(Pens.Black, 0, 0, 0, this.ClientRectangle.Height-20);
            //e.Graphics.DrawLine(Pens.Black, 0, 0, this.ClientRectangle.Width - 20, 0);

            int xModel = this.ClientRectangle.Width/2, yModel = this.ClientRectangle.Height/2, rModel=150;

            e.Graphics.DrawEllipse(Pens.Green, xModel - rModel, yModel - rModel, rModel * 2, rModel * 2);
            //graphics.DrawLine(Pens.Green, xModel - 2, yModel, xModel + 2, yModel);
            e.Graphics.DrawLine(Pens.Green, xModel - rModel-10, yModel, xModel + rModel+10, yModel);
            //graphics.DrawLine(Pens.Green, xModel, yModel - 2, xModel, yModel + 2);
            e.Graphics.DrawLine(Pens.Green, xModel, yModel - rModel-10, xModel, yModel + rModel+10);

            for (int i = 0; i < nPoints; i++ )
            {
                e.Graphics.DrawLine(Pens.Blue, _CurvePoints[i].X - 2, _CurvePoints[i].Y, _CurvePoints[i].X + 2, _CurvePoints[i].Y);
                e.Graphics.DrawLine(Pens.Blue, _CurvePoints[i].X, _CurvePoints[i].Y - 2, _CurvePoints[i].X, _CurvePoints[i].Y + 2);
            }

            //graphics.DrawLine(Pens.Red, Xmean - 2, Ymean, Xmean + 2, Ymean);
            //graphics.DrawLine(Pens.Red, Xmean, Ymean - 2, Xmean, Ymean + 2);
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {

            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "Button", e.Button);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Clicks", e.Clicks);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "X", e.X);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Y", e.Y);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Delta", e.Delta);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Location", e.Location);
            messageBoxCS.AppendLine();
            //MessageBox.Show(messageBoxCS.ToString(), "MouseClick Event");

            _CurvePoints[nPoints].X = e.X;
            _CurvePoints[nPoints].Y = e.Y;

            nPoints++;

            int Xsoma=0;
            int Ysoma=0;

            for (int i=0; i <= nPoints; i++)
            {
                Xsoma += _CurvePoints[i].X;
                Ysoma += _CurvePoints[i].Y;
            }
            Xmean = Xsoma / nPoints;
            Ymean = Ysoma / nPoints;

            messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "X", Xmean);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Y", Ymean);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "N", nPoints);
            //MessageBox.Show(messageBoxCS.ToString(), "MouseClick Event");

            this.Invalidate();
        }

        private void CircularRegression(out float a, out float b, out float R)
        {
            int n = nPoints; //Minimum 5
            float sum_x = 0;
            float sum_y = 0;
            float sum_xy = 0;
            float sum_x2y = 0;
            float sum_xy2 = 0;
            float sum_x2 = 0;
            float sum_x3 = 0;
            float sum_y2 = 0;
            float sum_y3 = 0;

            for(int i=0; i<n; i++)
            {
                sum_x = sum_x + _CurvePoints[i].X;
                sum_y = sum_y + _CurvePoints[i].Y;
                sum_xy = sum_xy + _CurvePoints[i].X * _CurvePoints[i].Y;
                sum_x2y = sum_x2y + (float)Math.Pow(_CurvePoints[i].X, 2) * _CurvePoints[i].Y;
                sum_xy2 = sum_xy2 + _CurvePoints[i].X * (float)Math.Pow(_CurvePoints[i].Y, 2);
                sum_x2 = sum_x2 + (float)Math.Pow(_CurvePoints[i].X, 2);
                sum_x3 = sum_x3 + (float)Math.Pow(_CurvePoints[i].X, 3);
                sum_y2 = sum_y2 + (float)Math.Pow(_CurvePoints[i].Y, 2);
                sum_y3 = sum_y3 + (float)Math.Pow(_CurvePoints[i].Y, 3);
            }

            float d11 = 0;
            float d20 = 0;
            float d02 = 0;
            float d30 = 0;
            float d03 = 0;
            float d21 = 0;
            float d12 = 0;

            d11 = n * sum_xy - sum_x * sum_y;
            d20 = n * sum_x2 - (float)Math.Pow(sum_x, 2);
            d02 = n * sum_y2 - (float)Math.Pow(sum_y, 2);
            d30 = n * sum_x3 - sum_x2 * sum_x;
            d03 = n * sum_y3 - sum_y * sum_y2;
            d21 = n * sum_x2y - sum_x2 * sum_y;
            d12 = n * sum_xy2 - sum_x * sum_y2;

            a = ((d30 + d12) * d02 - (d03 + d21) * d11) / (2 * (d20 * d02 - (float)Math.Pow(d11, 2)));
            b = ((d03 + d21) * d20 - (d30 + d12) * d11) / (2 * (d20 * d02 - (float)Math.Pow(d11, 2)));

            float c = 0;

            c = (1 / (float)n) * (sum_x2 + sum_y2 - 2 * a * sum_x - 2 * b * sum_y);
            
            R = (float)Math.Sqrt(c + (float)Math.Pow(a, 2) + (float)Math.Pow(b, 2));
        }

        private void EllipseRegression(out float Xc, out float Yc, out float a, out float b, out float theta)
        {
            int n = nPoints; //Minimum 5
            float sum_x = 0;
            float sum_y = 0;
            float sum_xy = 0;
            float sum_x2y = 0;
            float sum_x3y = 0;
            float sum_xy2 = 0;
            float sum_xy3 = 0;
            float sum_x2y2 = 0;
            float sum_x2 = 0;
            float sum_x3 = 0;
            float sum_x4 = 0;
            float sum_y2 = 0;
            float sum_y3 = 0;
            float sum_y4 = 0;

            for (int i = 0; i < n; i++)
            {
                sum_x = sum_x + _CurvePoints[i].X;
                sum_y = sum_y + _CurvePoints[i].Y;
                sum_xy = sum_xy + _CurvePoints[i].X * _CurvePoints[i].Y;
                sum_x2y = sum_x2y + (float)Math.Pow(_CurvePoints[i].X, 2) * _CurvePoints[i].Y;
                sum_x3y = sum_x3y + (float)Math.Pow(_CurvePoints[i].X, 3) * _CurvePoints[i].Y;
                sum_xy2 = sum_xy2 + _CurvePoints[i].X * (float)Math.Pow(_CurvePoints[i].Y, 2);
                sum_xy3 = sum_xy3 + _CurvePoints[i].X * (float)Math.Pow(_CurvePoints[i].Y, 3);
                sum_x2y2 = sum_x2y2 + (float)Math.Pow(_CurvePoints[i].X, 2) * (float)Math.Pow(_CurvePoints[i].Y, 2);
                sum_x2 = sum_x2 + (float)Math.Pow(_CurvePoints[i].X, 2);
                sum_x3 = sum_x3 + (float)Math.Pow(_CurvePoints[i].X, 3);
                sum_x4 = sum_x4 + (float)Math.Pow(_CurvePoints[i].X, 4);
                sum_y2 = sum_y2 + (float)Math.Pow(_CurvePoints[i].Y, 2);
                sum_y3 = sum_y3 + (float)Math.Pow(_CurvePoints[i].Y, 3);
                sum_y4 = sum_y4 + (float)Math.Pow(_CurvePoints[i].Y, 4);
            }

            double[][] m = MatrixCalc.MatrixCreate(5, 5);

            m[0][0] = sum_y4; m[0][1] = sum_x2y2; m[0][2] = sum_xy3; m[0][3] = sum_y3; m[0][4] = sum_xy2;
            m[1][0] = sum_x2y2; m[1][1] = sum_x4; m[1][2] = sum_x3y; m[1][3] = sum_x2y; m[1][4] = sum_x3;
            m[2][0] = sum_xy3; m[2][1] = sum_x3y; m[2][2] = sum_x2y2; m[2][3] = sum_xy2; m[2][4] = sum_x2y;
            m[3][0] = sum_y3; m[3][1] = sum_x2y; m[3][2] = sum_xy2; m[3][3] = sum_y2; m[3][4] = sum_xy;
            m[4][0] = sum_xy2; m[4][1] = sum_x3; m[4][2] = sum_x2y; m[4][3] = sum_xy; m[4][4] = sum_x2;

            double[][] vars=MatrixCalc.MatrixCreate(5, 1);

            vars[0][0] = -sum_y2; //Cy2
            vars[1][0] = -sum_x2; //Ax2
            vars[2][0] = -sum_xy; //Bxy
            vars[3][0] = -sum_y;  //Ey
            vars[4][0] = -sum_x;  //Dx
         
            double[][] inv = MatrixCalc.MatrixInverse(m);

            double[][] m1 = MatrixCalc.MatrixProduct(inv, vars);

            //Ay2 + Bx2 + Cxy + Dy + Ex + 1 = 0

            float C = (float)m1[0][0];
            float A = (float)m1[1][0];
            float B = (float)m1[2][0];
            float E = (float)m1[3][0];
            float D = (float)m1[4][0];
            float F = 1;

            //Ax2 + Bxy + Cy2 + Dx + Ey + F = 0

            Xc = ((2 * C * D) - (B * E)) / ((float)Math.Pow(B, 2) - 4 * A * C);
            Yc = ((2 * A * E) - (B * D)) / ((float)Math.Pow(B, 2) - 4 * A * C);
            
            a = -(float)Math.Sqrt(2 * (A * (float)Math.Pow(E, 2) + C * (float)Math.Pow(D, 2) - B * D * E + ((float)Math.Pow(B, 2) - 4 * A * C) * F) * (A + C + (float)Math.Sqrt((float)Math.Pow((A - C), 2) + (float)Math.Pow(B, 2)))) / ((float)Math.Pow(B, 2) - 4 * A * C);
            b = -(float)Math.Sqrt(2 * (A * (float)Math.Pow(E, 2) + C * (float)Math.Pow(D, 2) - B * D * E + ((float)Math.Pow(B, 2) - 4 * A * C) * F) * (A + C - (float)Math.Sqrt((float)Math.Pow((A - C), 2) + (float)Math.Pow(B, 2)))) / ((float)Math.Pow(B, 2) - 4 * A * C);

            if(a<b)
            {
                float c = a;
                a = b;
                b = c;
            }

            if(B==0)
            {
                if (A < C)
                    theta = 0;
                else theta = (float)Math.PI/2;
            }
            else theta=(float)(Math.Atan((C-A-(float)Math.Sqrt((float)Math.Pow((A-C),2)+(float)Math.Pow(B,2))))/B);
            theta = theta * 180 / (float)Math.PI;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            float a, b, R;
            float x, y, r;
            CircularRegression(out a, out b, out R);

            x = (float)a;
            y = (float)b;
            r = (float)R;

            System.Drawing.Graphics graphics = this.CreateGraphics();

            graphics.DrawEllipse(Pens.Red, x - r, y - r, 2*r, 2*r);
            graphics.DrawLine(Pens.Red, x - 2, y, x + 2, y);
            graphics.DrawLine(Pens.Red, x, y - 2, x, y + 2);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            nPoints = 0;
            this.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            float Xc, Yc, a, b, Theta;

            EllipseRegression(out Xc, out Yc, out a, out b, out Theta);

            System.Drawing.Graphics graphics = this.CreateGraphics();

            Matrix rotmatrix = new Matrix();
            rotmatrix.RotateAt(Theta, new PointF(Xc, Yc));

            graphics.Transform = rotmatrix;

            graphics.DrawEllipse(Pens.Red, Xc - a, Yc - b, 2 * a, 2 * b);

            //graphics.DrawLine(Pens.Red, Xc - 2, Yc, Xc + 2, Yc);
            graphics.DrawLine(Pens.Red, Xc - a, Yc, Xc + a, Yc);
            //graphics.DrawLine(Pens.Red, Xc, Yc - 2, Xc, Yc + 2);
            graphics.DrawLine(Pens.Red, Xc, Yc - b, Xc, Yc + b);

            rotmatrix.RotateAt(-Theta, new PointF(Xc, Yc));

            graphics.Transform = rotmatrix;

            var fontFamily = new FontFamily("Times New Roman");
            var font = new Font(fontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel);

            graphics.DrawString("Theta="+Theta.ToString(), font, Brushes.Red, 10,10 );
            graphics.DrawString("Xc=" + Xc.ToString(), font, Brushes.Red, 10, 24);
            graphics.DrawString("Yc=" + Yc.ToString(), font, Brushes.Red, 10, 38);
            graphics.DrawString("a=" + (a*2).ToString(), font, Brushes.Red, 10, 52);
            graphics.DrawString("b=" + (b*2).ToString(), font, Brushes.Red, 10, 66);

        }
    }
}
