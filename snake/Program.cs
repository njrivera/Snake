using System;

namespace snake
{
    public class Grid
    {
        public int yDim;
        public int xDim;
        public char[,] grid;
        public Grid(int y, int x)
        {
            yDim = y;
            xDim = x;
            grid = new char[yDim, xDim];
            for (int i = 0; i < y; i++)
            {
                if (i == 0 || i == y - 1)
                    for (int j = 0; j < x; j++)
                    {
                        grid[i, j] = '#';
                    }
                else
                {
                    grid[i, 0] = grid[i, x - 1] = '#';
                    for (int j = 1; j < x - 1; j++)
                    {
                        grid[i, j] = ' ';
                    }
                }
            }
        }

        public void printGrid()
        {
            for (int i = 0; i < yDim; i++)
            {
                for (int j = 0; j < xDim; j++)
                {
                    Console.Write(grid[i, j]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        public void dropFood()
        {
            Random rand = new Random();
            bool foodDropped = false;
            do
            {
                int y = rand.Next(1, yDim - 2);
                int x = rand.Next(1, xDim - 2);
                if (grid[y, x] != '*')
                {
                    grid[y, x] = '@';
                    foodDropped = true;
                }
            } while (!foodDropped);
        }
    }

    class Segment
    {
        public int y;
        public int x;
        public Segment(int y, int x)
        {
            this.y = y;
            this.x = x;
        }
    }

    class Snake
    {
        public System.Collections.Generic.List<Segment> body;
        public Snake(int y, int x)
        {
            body = new System.Collections.Generic.List<Segment>();
            body.Add(new Segment(y, x));
        }

        public void move(int y, int x)
        {
            body.Add(new Segment(y, x));
            body.RemoveAt(0);
        }

        public void eat(int y, int x)
        {
            body.Add(new Segment(y, x));
        }
    }

    class Program
    {
        public ConsoleKeyInfo ch;
        bool gameOver;

        public Program()
        {
            this.gameOver = false;
        }

        static void Main(string[] args)
        {
            int yDim = 20, xDim = 20;
            Program game = new Program();
            Grid grid = new Grid(yDim, xDim);

            Random rand = new Random();
            int y = rand.Next(1, yDim - 2);
            int x = rand.Next(1, xDim - 2);

            grid.grid[y, x] = '*';
            grid.dropFood();
            Snake snake = new Snake(y, x);
            Segment nextMove = new Segment(y, x);
            char prevMove;
            
            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(game.getMove));
            th.Start();
            
            do
            {
                Console.Clear();
                grid.printGrid();
                System.Threading.Thread.Sleep(300);
                nextMove = game.move(game.ch.KeyChar, snake.body[snake.body.Count - 1]);
                if (nextMove != null)
                {
                    prevMove = game.ch.KeyChar;
                    if (grid.grid[nextMove.y, nextMove.x] == ' ')
                    {
                        Segment s = snake.body[0];
                        grid.grid[s.y, s.x] = ' ';
                        snake.move(nextMove.y, nextMove.x);
                    }
                    else if (grid.grid[nextMove.y, nextMove.x] == '@')
                    {
                        snake.eat(nextMove.y, nextMove.x);
                        grid.dropFood();
                    }
                    else
                    {
                        Console.WriteLine("GAME OVER");
                        game.gameOver = true;
                    }
                }
                foreach (var seg in snake.body)
                {
                    grid.grid[seg.y, seg.x] = '*';
                }
            } while (!game.gameOver);

            Console.ReadKey();
        }

        public void getMove()
        {
            while (!gameOver)
            {
                ch = Console.ReadKey();
            }
        }

        public Segment move(char c, Segment from)
        {
            switch (c)
            {
                case 'a': return new Segment(from.y, from.x - 1);
                case 'w': return new Segment(from.y - 1, from.x);
                case 's': return new Segment(from.y + 1, from.x);
                case 'd': return new Segment(from.y, from.x + 1);
                default: return null;
            }
        }
    }
}