using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PingPong
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.CursorVisible = false;                                // turn off blinking cursor
            PingPong gra = new PingPong();

            gra.RemoveScrolls();

            Thread thread = new Thread(gra.movingBall);                             //set up ball animation to run in background
            thread.IsBackground = true;
            thread.Start();

            gra.startPosition();                                     //set up start position for all objects 
            while (true)                                               //game 
            {

                gra.move();
                gra.board();


            }
        }
    }


    class PingPong
    {
        private int padsize = 5;
        private int pad1y, pad2y, pad1x = Console.WindowWidth - 2, pad2x = 2;
        private int player2Score, player1Score;
        private int i = 0, ballx = 1, bally = 1;
        public String direction = "downright";



        public int Player2Score { get => player2Score; set => player2Score = value; }
        public int Player1Score { get => player1Score; set => player1Score = value; }

        public class Point                                                                                    //creation and point drawing
        {
            private int x, y;
            private char dot;


            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public int X { get => x; set => x = value; }
            public int Y { get => y; set => y = value; }
            public char Dot { get => dot; set => dot = value; }

            public void draw(char dot)                                                              //draw points
            {

                this.Dot = dot;
                if (this.X < Console.WindowWidth && this.X > 0 && this.Y > 0 && this.Y < Console.WindowHeight)
                {
                    Console.SetCursorPosition(this.X, this.Y);
                    Console.Write(Dot);

                }

            }
        }


        public class Line                                           // line declaration and draw
        {

            Point p1;
            Point p2;
            Point[] inlinePoints;



            public void draw(char dot)                                     // draw line
            {
                p2.draw(dot);
                p1.draw(dot);

                for (int i = 0; i < this.inlinePoints.Length; i++)
                {
                    this.inlinePoints[i].draw(dot);
                }
            }

            public Line(Point p1, Point p2)                                                    // line definition and points definition between p1 and p2
            {
                int lineLength;
                this.p1 = p1;
                this.p2 = p2;


                this.inlinePoints = new Point[lineLength = (int)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2))];

                for (int i = 0; i < this.inlinePoints.Length; i++)
                {
                    this.inlinePoints[i] = new Point(p1.X + i * (p2.X - p1.X) / lineLength, p1.Y + i * (p2.Y - p1.Y) / lineLength);

                }


            }

        }


        public void RemoveScrolls()                                                                  // scrolls removal
        {

            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }

        public void ballReset()                                                      // place ball in the center
        {

            this.ballx = Console.WindowWidth / 2;
            this.bally = Console.WindowHeight / 2;

        }

        public void playerReset()                                          // place players at start position
        {

            this.pad1y = Console.WindowHeight / 2;
            this.pad2y = Console.WindowHeight / 2;
        }

        public void startPosition()                                                          // start position for each
        {
            Console.Clear();
            ballReset();
            board();
            playerReset();
            player1(pad1y, padsize);
            player2(pad2y, padsize);


        }

        public void restartGame()
        {
            Console.Clear();
            ballReset();
            board();
            playerReset();
            player1(pad1y, padsize);
            player2(pad2y, padsize);
            this.Player1Score = 0;
            this.Player2Score = 0;
        }

        public void player1(int pad1y, int padsize)                          //draw player 1 pad
        {

            Line padPlayer1 = new Line(new Point(pad1x, pad1y), new Point(pad1x, pad1y + padsize));
            padPlayer1.draw('#');
        }

        public void player2(int pad2y, int padsize)                                    // draw player 2 pad
        {
            Line padPlayer2 = new Line(new Point(pad2x, pad2y), new Point(pad2x, pad2y + padsize));
            padPlayer2.draw('#');
        }

        public void ball(int x, int y)                                             //draw ball
        {
            Point ball = new Point(x, y);
            ball.draw('O');

        }

        public void board()                                                                //draw game board
        {

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(3, 2);
            Console.Write("Player 2 Score: {0}", this.Player2Score);
            Console.SetCursorPosition(Console.WindowWidth - 20, 2);
            Console.Write("Player 1 Score: {0}", this.Player1Score);
            Console.SetCursorPosition(Console.WindowWidth / 2 - 12, 2);
            Console.Write("Press ESC to exit");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 12, 3);
            Console.Write("Press Space to restart");
            Line top = new Line(new Point(1, 4), new Point(Console.WindowWidth - 1, 4));

            if (this.Player2Score == 21)
            {
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
                Console.Write("Player 2 Won");

            }
            else if (this.Player1Score == 21)
            {
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
                Console.Write("Player 1 Won");

            }


            top.draw('-');



        }

        public void movingBall()
        {
            ballReset();
            Random rnd = new Random();
            while (true)
            {

                if (ballx == 1)       // collision with left wall 
                {

                    startPosition();
                    this.Player1Score++;

                }
                else if (ballx == Console.WindowWidth - 1) //collision with right wall 
                {

                    startPosition();
                    this.Player2Score++;


                }

                if (bally == 4 && direction == "upright")                                       // collision with top
                {
                    direction = "downright";
                }
                else if (bally == 4 && direction == "upleft")
                {
                    direction = "downleft";
                }

                if (bally == Console.WindowHeight - 1 && direction == "downright")              // collision with bottom
                {
                    direction = "upright";

                }
                else if (bally == Console.WindowHeight - 1 && direction == "downleft")
                {

                    direction = "upleft";

                }


                if (ballx == pad1x) // collision with player1 pad 
                {

                    if (bally >= pad1y && bally < pad1y + padsize)
                    {

                        if (rnd.Next(1, 2) == 1)
                        {
                            direction = "upleft";
                        }
                        else direction = "downleft";

                    }

                }
                else if (ballx == pad2x)                 // collision with player2 pad 
                {
                    if (bally < pad2y + padsize && bally >= pad2y)
                    {
                        if (rnd.Next(1, 2) == 1)
                        {
                            direction = "upright";
                        }
                        else direction = "downright";
                    }

                }



                switch (direction)                                                            //update position of the ball
                {
                    case "downright":

                        Console.MoveBufferArea(ballx, bally, 1, 1, ++ballx, ++bally);

                        break;
                    case "upright":

                        Console.MoveBufferArea(ballx, bally, 1, 1, ++ballx, --bally);

                        break;

                    case "upleft":
                        Console.MoveBufferArea(ballx, bally, 1, 1, --ballx, --bally);

                        break;

                    case "downleft":
                        Console.MoveBufferArea(ballx, bally, 1, 1, --ballx, ++bally);

                        break;

                }
                ball(ballx, bally);
                Thread.Sleep(50);                                             //speed of the ball
            }

        }








        public void move()                                               //player pad direction control 
        {

            ConsoleKeyInfo Keyinfo;
            Keyinfo = Console.ReadKey(true);
            Console.Clear();
            player2(pad2y, padsize);
            player1(pad1y, padsize);
            switch (Keyinfo.Key)
            {

                case ConsoleKey.UpArrow:
                    if (pad1y > 4)
                    {
                        pad1y--;
                        player1(pad1y, padsize);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (pad1y < Console.WindowHeight - padsize)
                    {
                        pad1y++;
                        player1(pad1y, padsize);
                    }
                    break;

                case ConsoleKey.W:
                    if (pad2y > 4)
                    {

                        pad2y--;
                        player2(pad2y, padsize);
                    }
                    break;
                case ConsoleKey.S:
                    if (pad2y < Console.WindowHeight - padsize)
                    {
                        pad2y++;
                        player2(pad2y, padsize);
                    }
                    break;

                case ConsoleKey.Escape:
                    {
                        Environment.Exit(0);
                    }
                    break;
                case ConsoleKey.Spacebar:
                    {
                        restartGame();
                    }
                    break;
            }

        }





    }






}
