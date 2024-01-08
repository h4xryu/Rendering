using System.Security.Cryptography;
using static System.Math;
namespace Rendering
{

    public partial class Cube : Form
    {
        private float[,] nodes_default = { {-100, -100, -100},
                                   {-100, -100, 100},
                                   {-100, 100, -100},
                                   {-100, 100, 100},
                                   {100, -100, -100},
                                   {100, -100, 100},
                                   {100, 100, -100},
                                   {100, 100, 100}
                                 };
        private float[,] nodes = { {-100, -100, -100},
                                   {-100, -100, 100},
                                   {-100, 100, -100},
                                   {-100, 100, 100},
                                   {100, -100, -100},
                                   {100, -100, 100},
                                   {100, 100, -100},
                                   {100, 100, 100}
                                 };
        private int[,] edges = {
                                    {0, 1},
                                    {1, 3},
                                    {3, 2},
                                    {2, 0},
                                    {4, 5},
                                    {5, 7},
                                    {7, 6},
                                    {6, 4},
                                    {0, 4},
                                    {1, 5},
                                    {2, 6},
                                    {3, 7}
                               };
        private Graphics g;
        private float[,] node;
        private float firstPointX = 0;
        private float firstPointY = 0;
        private float currentThetaX = 0;
        private float currentThetaY = 0;
        private float currentThetaZ = 0;
        private float cubePosX= 500;
        private float cubePosY= 200;
        private Boolean mouseFlag = false;
        private float distanceFromCam = 0;
        private float A,B,C;
        private float x,y,z;
        private float ooz;
        private int xp, yp;
        private static int width = 160;
        private static int height = 110;
        private static int K1 = 20;
        private int idx;

        public Cube()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            //this.Paint += new PaintEventHandler(draw);

            cube_init();
            draw();
        }
        private void cube_init()
        {
            node = (float[,]) nodes.Clone();
            // Z->Y->X
            rotateZ3D(currentThetaZ);
            rotateY3D(currentThetaY);
            rotateX3D(currentThetaX);
            SetCubePosition(cubePosX, cubePosY, 0);
        }
        private float[,] GetNodes()
        {
            return nodes;
        }

        private void SetCubePosition(float x, float y, float z)
        {
            for (int n = 0; n < nodes.GetLength(0); n++)
            {

                node[n, 0] += x;
                node[n, 1] += y;
                node[n, 2] -= z;
            }
        }

        private void rotateX3D(float theta)
        {
            float sinTheta = (float)Math.Sin(theta);
            float cosTheta = (float)Math.Cos(theta);

            for (int n = 0; n < nodes.GetLength(0); n++)
            {
                
                float y = node[n, 1];
                float z = node[n, 2];

                node[n, 1] = (float)(y * cosTheta - z * sinTheta);
                node[n, 2] = (float)(z * cosTheta + y * sinTheta);
            }
        }

        private void rotateY3D(float theta)
        {
            float sinTheta = (float)Math.Sin(theta);
            float cosTheta = (float)Math.Cos(theta);

            for (int n = 0; n < nodes.GetLength(0); n++)
            {
              
                float x = node[n, 0];
                float z = node[n, 2];

                node[n, 0] = (float)(x * cosTheta + z * sinTheta);
                node[n, 2] = (float)(z * cosTheta - x * sinTheta);
            }
        }


        private void rotateZ3D(float theta)
        {
            float sinTheta = (float)Math.Sin(theta);
            float cosTheta = (float)Math.Cos(theta);

            for (int n = 0; n < nodes.GetLength(0); n++)
            {
               
                float x = node[n, 0];
                float y = node[n, 1];

                node[n, 0] = (float)(x * cosTheta - y * sinTheta);
                node[n, 1] = (float)(y * cosTheta + x * sinTheta);
            }
        }

        private void draw()
        {
            g.Clear(Color.White);
            for (int i = 0; i < edges.GetLength(0); i++)
            {
           
                int n0 = edges[i, 0];
                int n1 = edges[i, 1];
                float[] node0 = { node[n0, 0], node[n0, 1], node[n0, 2] };
                float[] node1 = { node[n1, 0], node[n1, 1], node[n1, 2] };

                g.DrawLine(Pens.Blue, node0[0], node0[1], node1[0], node1[1]);


            }
        }

        private void Cube_MouseDown(object sender, MouseEventArgs e)
        {

            clearNode();
            draw();

        }

        private void Cube_MouseUp(object sender, MouseEventArgs e)
        {
            
            

        }

        private void Cube_MouseMove(object sender, MouseEventArgs e)
        {
            float totalX = 0;
            float totalY = 0;
            float currentPointX;
            float currentPointY;


            if (e.Button == MouseButtons.Left)
            {
                currentPointX = e.X;
                currentPointY = e.Y;
                SetCubePosition(-cubePosX, -cubePosY, 0);
                currentThetaX += ((firstPointX - currentPointX) / 1000);
                currentThetaY -= ((firstPointY - currentPointY) / 1000);
                toolStripStatusLabel1.Text =
                    "Point : ( " + currentPointX + ", " + currentPointY + " )" +
                    " currentThetaX : " + currentThetaX + " " +
                    "currentThetaY : " + currentThetaY;


                rotateY3D(currentThetaX);
                rotateX3D(currentThetaY);
                SetCubePosition(cubePosX, cubePosY, 0);
                draw();
            }

            firstPointX = e.X;
            firstPointY = e.Y;

            
         }
            
        
        

        private void clearNode()
        {
            currentThetaX = 0;
            currentThetaY = 0;
            currentThetaZ = 0;
            node = nodes;
            cube_init();
        }

        private void Cube_Load(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked_1(object sender, EventArgs e)
        {

        }


        private float calculateX(float i, float j, float k)
        {
            return (float)(j * Sin(A) * Sin(B) * Cos(C) - k * Cos(A) * Sin(B) * Cos(C) +
                           j * Cos(A) * Sin(C) + k * Sin(A) * Sin(C) + i * Cos(B) * Cos(C)
                );
        }
        private float calculateY(float i, float j, float k)
        {
            return (float)(j * Cos(A) * Cos(C) + k * Sin(A) * Cos(C) -
                           j * Sin(A) * Sin(B) * Sin(C) + k * Cos(A) * Sin(B) * Sin(C) -
                           i * Cos(B) * Sin(C)
                );
        }
        private float calculateZ(float i, float j, float k)
        {
            return (float)(k * Cos(A) * Cos(B) - j * Sin(A) * Cos(B) + i * Sin(B));
        }

        private void calculateForSurface(float cubeX, float cubeY, float cubeZ)
        {
            x = calculateX(cubeX, cubeY, cubeZ);
            y = calculateY(cubeX, cubeY, cubeZ);
            z = calculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

            ooz = 1 / z;

            xp = (int)(Width / 2 + K1 * ooz * x * 2);
            yp = (int)(Height / 2 + K1 * ooz * y);

            idx = xp + yp * Width;
            if(idx >=  0) { }
        }

        // 점 P가 평행사변형 내부에 있는지 확인하는 함수
        bool IsInsideParallelogram(Point A, Point B, Point C, Point D, Point P)
        {
            // 점 P가 각 변에 대해 양쪽에 있는지 확인합니다.
            // 실제로는 각 변과의 관계를 더욱 정확하게 계산해야 합니다.
            return (CrossProduct(A, B, P) <= 0 && CrossProduct(B, C, P) <= 0 &&
                    CrossProduct(C, D, P) <= 0 && CrossProduct(D, A, P) <= 0);
        }

        // 벡터의 외적 계산 함수
        int CrossProduct(Point A, Point B, Point P)
        {
            return (B.X - A.X) * (P.Y - A.Y) - (B.Y - A.Y) * (P.X - A.X);
        }
    }

}

