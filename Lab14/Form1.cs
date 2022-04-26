using System;
using System.Drawing;
using System.Windows.Forms;


namespace Lab14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const double x_focus = 0;
        private const double y_focus = 0;
        private const double z_focus = 0;
     
        private float r_Eye;
        private float phi_Eye;
        private float theta_Eye;
     
        private const double pi = Math.PI;
 
        private int Octahedron;
        private int Dodecahedron;
     
        private float[,] MatrixProjection = new float[5, 5];

        private const int PerspectiveProjection = 1;

        private void Form1_Load(object sender, EventArgs e)
        {
            update();
        }

        private void update()
        {

            r_Eye = 4;
            phi_Eye = (float)(0.05 * pi);
            theta_Eye = (float)(0.3 * pi);
            Projection(ref MatrixProjection, PerspectiveProjection, r_Eye, phi_Eye, theta_Eye, (float)x_focus, (float)y_focus, (float)z_focus, 0, 1, 0);
            СalculateParameters();
            pictureBox1.Image = new Bitmap(pictureBox1.Width,
            pictureBox1.Height);
  
            Designing((Bitmap)pictureBox1.Image);
        }


        private void Designing(Bitmap bmp)

        {

            Graphics g;
            g = Graphics.FromImage(bmp);
            g.Clear(SystemColors.Window);
            g.Dispose();
            Random random = new Random();

            TransformAllDataFull(ref MatrixProjection);
            DrawSolid(bmp, Octahedron, Dodecahedron - 1,  System.Drawing.Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), false);

        }



        private void СalculateParameters()

        {

     
      

            Octahedron = NumLines + 1;

            DesigningLine(0, 1, 0, 1, 0, 0);

            DesigningLine(0, 1, 0, -1, 0, 0);

            DesigningLine(0, 1, 0, 0, 0, 1);

            DesigningLine(0, 1, 0, 0, 0, -1);

            DesigningLine(0, -1, 0, 1, 0, 0);

            DesigningLine(0, -1, 0, -1, 0, 0);

            DesigningLine(0, -1, 0, 0, 0, 1);

            DesigningLine(0, -1, 0, 0, 0, -1);

            DesigningLine(0, 0, 1, 1, 0, 0);

            DesigningLine(0, 0, 1, -1, 0, 0);

            DesigningLine(0, 0, -1, 1, 0, 0);

            DesigningLine(0, 0, -1, -1, 0, 0);

            Dodecahedron = NumLines + 1;


        }

   

        public struct Line

        {

            public float[] fr_points;

            public float[] to_points;

            public float[] fr_tr_points;

            public float[] to_tr_points;


            public void Initialize()

            {

                fr_points = new float[5];

                to_points = new float[5];

                fr_tr_points = new float[5];

                to_tr_points = new float[5];

            }

        }

        public Line[] Lines = new Line[100];
        public int NumLines = 0;


        public void DesigningLine(float x1, float y1, float z1,

        float x2, float y2, float z2)

        {

            NumLines = NumLines + 1;
            Lines[NumLines].Initialize();
            Lines[NumLines].fr_points[1] = x1;
            Lines[NumLines].fr_points[2] = y1;
            Lines[NumLines].fr_points[3] = z1;
            Lines[NumLines].fr_points[4] = 1;
            Lines[NumLines].to_points[1] = x2;
            Lines[NumLines].to_points[2] = y2;
            Lines[NumLines].to_points[3] = z2;
            Lines[NumLines].to_points[4] = 1;

        }

        public void TransformAllDataFull(ref float[,] M)

        {

            TransformDataFull(ref M, 1, NumLines);

        }
     
        public void TransformDataFull(ref float[,] M,

        int line1, int line2)

        {

            for (int i = line1; i <= line2; i++)

            {

                MatrixApplyFull(ref Lines[i].fr_points, ref M,
                ref Lines[i].fr_tr_points);
                MatrixApplyFull(ref Lines[i].to_points, ref M,
                ref Lines[i].to_tr_points);

            }

        }



        public void DrawSolid(Bitmap bmp,

        int first_line, int last_line, Color color, bool clear)

        {

            float x1, y1, x2, y2;

            Graphics g; Pen pen;
            pen = new Pen(color, 2);

            g = Graphics.FromImage(bmp);

            if (clear) g.Clear(System.Drawing.Color.Black);

            for (int i = first_line; i <= last_line; i++)

            {

                x1 = Lines[i].fr_tr_points[1];

                y1 = Lines[i].fr_tr_points[2];

                x2 = Lines[i].to_tr_points[1];

                y2 = Lines[i].to_tr_points[2];


                g.DrawLine(pen,

                (x1 * bmp.Width / 4) + bmp.Width / 2.0F,

                bmp.Height / 2.0F - (y1 * bmp.Height / 4),

                (x2 * bmp.Width / 4) + bmp.Width / 2.0F,

                bmp.Height / 2.0F - (y2 * bmp.Height / 4));

            }


            g.Dispose(); pen.Dispose();

        }


        public void MatrixIdentity(ref float[,] M)

        {

            for (int i = 1; i <= 4; i++)

            {

                for (int j = 1; j <= 4; j++)

                {

                    if (i == j) M[i, j] = 1;

                    else M[i, j] = 0;

                }

            }

        }

        public void MatrixPerspectiveXZ(ref float[,] M,

        float Distance)

        {

            MatrixIdentity(ref M);

            if (Distance != 0) M[3, 4] = -1 / Distance;

        }

        public void MatrixTransformation(ref float[,] M,

        int type_of_projection,

        float Cx, float Cy, float Cz,

        float Fx, float Fy, float Fz,

        float ux, float uy, float uz)

        {

            float[,] M1 = new float[5, 5];

            float[,] M2 = new float[5, 5];

            float[,] M3 = new float[5, 5];

            float[,] M4 = new float[5, 5];

            float[,] M5 = new float[5, 5];

            float[,] M12 = new float[5, 5];

            float[,] M34 = new float[5, 5];

            float[,] M1234 = new float[5, 5];

            float sin1 = 0, cos1 = 0; float sin2 = 0, cos2 = 0;

            float sin3, cos3; float A, B, C; float d1, d2, d3;

            float[] up1 = new float[5]; float[] up2 = new float[5];

    

            MatrixTranslate(ref M1, -Fx, -Fy, -Fz);

            A = Cx - Fx; B = Cy - Fy; C = Cz - Fz;

            d1 = (float)Math.Sqrt(A * A + C * C);

            if (d1 != 0)

            {

                sin1 = -A / d1; cos1 = C / d1;

            }

            d2 = (float)Math.Sqrt(A * A + B * B + C * C);

            if (d2 != 0)

            {

                sin2 = B / d2; cos2 = d1 / d2;

            }

           

            MatrixIdentity(ref M2);

      

            if (d1 != 0)

            {

                M2[1, 1] = cos1; M2[1, 3] = -sin1;

                M2[3, 1] = sin1; M2[3, 3] = cos1;

            }

          
            MatrixIdentity(ref M3);

          

            if (d2 != 0)

            {

                M3[2, 2] = cos2; M3[2, 3] = sin2;

                M3[3, 2] = -sin2; M3[3, 3] = cos2;

            }

        

            up1[1] = ux; up1[2] = uy; up1[3] = uz;

            up1[4] = 1;

            MatrixApply(ref up1, ref M2, ref up2);

            MatrixApply(ref up2, ref M3, ref up1);

          

            d3 = (float)Math.Sqrt(up1[1] * up1[1] +

            up1[2] * up1[2]);

            MatrixIdentity(ref M4);

  
            if (d3 != 0)

            {

                sin3 = up1[1] / d3; cos3 = up1[2] / d3;

                M4[1, 1] = cos3; M4[1, 2] = sin3;

                M4[2, 1] = -sin3; M4[2, 2] = cos3;

            }

    
            if (type_of_projection == PerspectiveProjection)

                MatrixPerspectiveXZ(ref M5, d2);

            else

                MatrixIdentity(ref M5);

            if (d2 != 0)

                MatrixPerspectiveXZ(ref M5, d2);

            else

                MatrixIdentity(ref M5);

  

            m3MatMultiply(ref M12, ref M1, ref M2);

            m3MatMultiply(ref M34, ref M3, ref M4);

            m3MatMultiply(ref M1234, ref M12, ref M34);

            if (type_of_projection == PerspectiveProjection)

                m3MatMultiplyFull(ref M, ref M1234, ref M5);

            else

                m3MatMultiply(ref M, ref M1234, ref M5);


        }



        public void Projection(ref float[,] M,

        int type_of_projection, float R,

        float phi, float theta,

        float Fx, float Fy, float Fz,

        float ux, float uy, float uz)

        {

            float Cx, Cy, Cz, r2;

            //Переходим к прямоугольным координатам:

            Cy = R * (float)Math.Sin(phi);

            r2 = R * (float)Math.Cos(phi);

            Cx = r2 * (float)Math.Cos(theta);

            Cz = r2 * (float)Math.Sin(theta);

            MatrixTransformation(ref M, type_of_projection,

            Cx, Cy, Cz, Fx, Fy, Fz, ux, uy, uz); //ref M

        }


        public void m3Reflect(ref float[,] M,

        float p1, float p2, float p3,

        float n1, float n2, float n3)

        {

            float[,] T = new float[5, 5]; //Перенос.

            float[,] R1 = new float[5, 5]; //Вращение 1.

            float[,] r2 = new float[5, 5]; //Вращение 2.

            float[,] S = new float[5, 5]; //Отражение.

            float[,] R2i = new float[5, 5]; //Не вращать 2.

            float[,] R1i = new float[5, 5]; //Не вращать 1.

            float[,] Ti = new float[5, 5]; //Не переносить.

            float D, L;

            float[,] M12 = new float[5, 5];

            float[,] M34 = new float[5, 5];

            float[,] M1234 = new float[5, 5];

            float[,] M56 = new float[5, 5];

            float[,] M567 = new float[5, 5];

            //Переносим плоскость к началу координат:

            MatrixTranslate(ref T, -p1, -p2, -p3);

            MatrixTranslate(ref Ti, p1, p2, p3);

            //Вращаем вокруг оси z,

            //пока нормаль не будет в y-z плоскости:

            MatrixIdentity(ref R1);

            D = (float)Math.Sqrt(n1 * n1 + n2 * n2);

            R1[1, 1] = n2 / D; R1[1, 2] = n1 / D;

            R1[2, 1] = -R1[1, 2]; R1[2, 2] = R1[1, 1];

            MatrixIdentity(ref R1i);

            R1i[1, 1] = R1[1, 1]; R1i[1, 2] = -R1[1, 2];

            R1i[2, 1] = -R1[2, 1]; R1i[2, 2] = R1[2, 2];

            //Вращаем вокруг оси x, когда нормаль будет по оси y:

            MatrixIdentity(ref r2);

            L = (float)Math.Sqrt(n1 * n1 + n2 * n2 + n3 * n3);

            r2[2, 2] = D / L; r2[2, 3] = -n3 / L;

            r2[3, 2] = -r2[2, 3]; r2[3, 3] = r2[2, 2];

            MatrixIdentity(ref R2i);

            R2i[2, 2] = r2[2, 2]; R2i[2, 3] = -r2[2, 3];

            R2i[3, 2] = -r2[3, 2]; R2i[3, 3] = r2[3, 3];

            //Рисуем отражение объекта перпендикулярно x-z плоскости:

            MatrixIdentity(ref S); S[2, 2] = -1;

            //Комбинируем матрицы:

            m3MatMultiply(ref M12, ref T, ref R1);

            m3MatMultiply(ref M34, ref r2, ref S);

            m3MatMultiply(ref M1234, ref M12, ref M34);

            m3MatMultiply(ref M56, ref R2i, ref R1i);

            m3MatMultiply(ref M567, ref M56, ref Ti);

            m3MatMultiply(ref M, ref M1234, ref M567);

        }


        public void m3LineRotate(ref float[,] M,

        float p1, float p2, float p3,

        float d1, float d2, float d3, float theta)

        {


            float[,] T = new float[5, 5];
            float[,] R1 = new float[5, 5]; 

            float[,] r2 = new float[5, 5]; 

            float[,] Rot3 = new float[5, 5]; 

            float[,] R2i = new float[5, 5]; 

            float[,] R1i = new float[5, 5]; 
            float[,] Ti = new float[5, 5]; 
            float D, L;

            float[,] M12 = new float[5, 5];

            float[,] M34 = new float[5, 5];

            float[,] M1234 = new float[5, 5];

            float[,] M56 = new float[5, 5];

            float[,] M567 = new float[5, 5];


            MatrixTranslate(ref T, -p1, -p2, -p3);

            MatrixTranslate(ref Ti, p1, p2, p3);

       

            MatrixIdentity(ref R1);

            D = (float)Math.Sqrt(d1 * d1 + d2 * d2);

            R1[1, 1] = d2 / D; R1[1, 2] = d1 / D;

            R1[2, 1] = -R1[1, 2]; R1[2, 2] = R1[1, 1];

            MatrixIdentity(ref R1i);

            R1i[1, 1] = R1[1, 1]; R1i[1, 2] = -R1[1, 2];

            R1i[2, 1] = -R1[2, 1]; R1i[2, 2] = R1[2, 2];

          

            MatrixIdentity(ref r2);

            L = (float)Math.Sqrt(d1 * d1 + d2 * d2 + d3 * d3);

            r2[2, 2] = D / L; r2[2, 3] = -d3 / L;

            r2[3, 2] = -r2[2, 3]; r2[3, 3] = r2[2, 2];

            MatrixIdentity(ref R2i);

            R2i[2, 2] = r2[2, 2]; R2i[2, 3] = -r2[2, 3];

            R2i[3, 2] = -r2[3, 2]; R2i[3, 3] = r2[3, 3];

            m3MatMultiply(ref M12, ref T, ref R1);

            m3MatMultiply(ref M34, ref r2, ref Rot3);

            m3MatMultiply(ref M1234, ref M12, ref M34);

            m3MatMultiply(ref M56, ref R2i, ref R1i);

            m3MatMultiply(ref M567, ref M56, ref Ti);

            m3MatMultiply(ref M, ref M1234, ref M567);
        }

        public void MatrixTranslate(ref float[,] M,

        float Tx, float Ty, float Tz)

        {

            MatrixIdentity(ref M);

            M[4, 1] = Tx; M[4, 2] = Ty; M[4, 3] = Tz;

        }




        public void MatrixApplyFull(ref float[] V, ref float[,] M, ref float[] Result)

        {

            int i, j; float value = 0;

            Result = new float[5] { 0, 0, 0, 0, 0 };

            for (i = 1; i <= 4; i++)

            {

                value = 0;

                for (j = 1; j <= 4; j++)

                {

                    value = value + V[j] * M[j, i];

                }

                Result[i] = value;

            }

            if (value != 0)

            {

                Result[1] = Result[1] / value;

                Result[2] = Result[2] / value;

            }

            else

            {


                Result[3] = Single.MaxValue;

            }

            Result[4] = 1;

        }


        public void MatrixApply(ref float[] V,

        ref float[,] M, ref float[] Result)

        {

            Result[1] = V[1] * M[1, 1] + V[2] * M[2, 1] +

            V[3] * M[3, 1] + M[4, 1];

            Result[2] = V[1] * M[1, 2] + V[2] * M[2, 2] +

            V[3] * M[3, 2] + M[4, 2];

            Result[3] = V[1] * M[1, 3] + V[2] * M[2, 3] +

            V[3] * M[3, 3] + M[4, 3];

            Result[4] = 1;

        }


        public void m3MatMultiplyFull(ref float[,] Result,

        ref float[,] A, ref float[,] B)

        {

            int i, j, k; float value; Result = new float[5, 5];

            for (i = 1; i <= 4; i++)

            {

                for (j = 1; j <= 4; j++)

                {

                    value = 0;

                    for (k = 1; k <= 4; k++)

                        value = value + A[i, k] * B[k, j];

                    Result[i, j] = value;

                }

            }

        }

        public void m3MatMultiply(ref float[,] Result,

        ref float[,] A, ref float[,] B)

        {


            Result[1, 1] = A[1, 1] * B[1, 1] + A[1, 2] * B[2, 1]

            + A[1, 3] * B[3, 1];

            Result[1, 2] = A[1, 1] * B[1, 2] + A[1, 2] * B[2, 2]

            + A[1, 3] * B[3, 2];

            Result[1, 3] = A[1, 1] * B[1, 3] + A[1, 2] * B[2, 3]

            + A[1, 3] * B[3, 3];

            Result[1, 4] = 0;

            Result[2, 1] = A[2, 1] * B[1, 1] + A[2, 2] * B[2, 1]

            + A[2, 3] * B[3, 1];

            Result[2, 2] = A[2, 1] * B[1, 2] + A[2, 2] * B[2, 2]

            + A[2, 3] * B[3, 2];

            Result[2, 3] = A[2, 1] * B[1, 3] + A[2, 2] * B[2, 3]

            + A[2, 3] * B[3, 3];

            Result[2, 4] = 0;

            Result[3, 1] = A[3, 1] * B[1, 1] + A[3, 2] * B[2, 1]

            + A[3, 3] * B[3, 1];

            Result[3, 2] = A[3, 1] * B[1, 2] + A[3, 2] * B[2, 2]

            + A[3, 3] * B[3, 2];

            Result[3, 3] = A[3, 1] * B[1, 3] + A[3, 2] * B[2, 3]

            + A[3, 3] * B[3, 3];

            Result[3, 4] = 0;

            Result[4, 1] = A[4, 1] * B[1, 1] + A[4, 2] * B[2, 1]

            + A[4, 3] * B[3, 1] + B[4, 1];

            Result[4, 2] = A[4, 1] * B[1, 2] + A[4, 2] * B[2, 2]

            + A[4, 3] * B[3, 2] + B[4, 2];

            Result[4, 3] = A[4, 1] * B[1, 3] + A[4, 2] * B[2, 3]

            + A[4, 3] * B[3, 3] + B[4, 3];

            Result[4, 4] = 1;

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NumLines = 0;
          
            update();
          
        }

       


    }
}
