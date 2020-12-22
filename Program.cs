using System;
using System.Threading;

namespace tetris
{
    class Program
    {
        static Random random = new Random();
        static bool fix = true, gameover = false;
        static int fignum, tmp;
        static char[,] gamefield = new char[18, 12];
        static int x1, x2, x3, x4, y1, y2, y3, y4, droptime = 600, rotates = 0, fulliterator = 0;
        static ConsoleKeyInfo keyinfo = new ConsoleKeyInfo();
        static Thread backgroundGame = new Thread(backgroundTetris);
        static void figselect() 
        {
            rotates = 0;
            fignum = random.Next(1, 8);
        }
        static void figinstall()
        {
            switch (fignum)
            {
                case 1://I
                    y1 = 0; x1 = 5;
                    y2 = 1; x2 = 5;
                    y3 = 2; x3 = 5;
                    y4 = 3; x4 = 5;
                    break;
                case 2://square
                    y1 = 0; x1 = 5;
                    y2 = 0; x2 = 6;
                    y3 = 1; x3 = 5;
                    y4 = 1; x4 = 6;
                    break;
                case 3://T
                    y1 = 0; x1 = 4;
                    y2 = 0; x2 = 5;
                    y3 = 0; x3 = 6;
                    y4 = 1; x4 = 5;
                    break;
                case 4://J or reverse L
                    y1 = 0; x1 = 6;
                    y2 = 1; x2 = 6;
                    y3 = 2; x3 = 6;
                    y4 = 2; x4 = 5;
                    break;
                case 5://L
                    y1 = 0; x1 = 5;
                    y2 = 1; x2 = 5;
                    y3 = 2; x3 = 5;
                    y4 = 2; x4 = 6;
                    break;
                case 6://Z
                    y1 = 0; x1 = 5;
                    y2 = 1; x2 = 5;
                    y3 = 1; x3 = 6;
                    y4 = 2; x4 = 6;
                    break;
                case 7://reverse z
                    y1 = 0; x1 = 6;
                    y2 = 1; x2 = 6;
                    y3 = 1; x3 = 5;
                    y4 = 2; x4 = 5;
                    break;
            }
            gamefield[y1, x1] = '#';
            gamefield[y2, x2] = '#';
            gamefield[y3, x3] = '#';
            gamefield[y4, x4] = '#';
        }
        static void figview()
        {
            for (int i = 0; i < 18; i++)
            {
                Console.SetCursorPosition(0, i);
                for (int j = 0; j < 12; j++)
                {
                    Console.Write(gamefield[i, j]);
                }
            }
        }
        static void leftArrow()
        {
            if (x1 != 0 && x2 != 0 && x3 != 0 && x4 != 0)
            {
                gamefield[y1, x1] = ' ';
                gamefield[y2, x2] = ' ';
                gamefield[y3, x3] = ' ';
                gamefield[y4, x4] = ' ';
                x1--; x2--; x3--; x4--;
                gamefield[y1, x1] = '#';
                gamefield[y2, x2] = '#';
                gamefield[y3, x3] = '#';
                gamefield[y4, x4] = '#';
                figview();
            }
        }
        static void rightArrow()
        {
            if (x1 != 11 && x2 != 11 && x3 != 11 && x4 != 11)
            {
                gamefield[y1, x1] = ' ';
                gamefield[y2, x2] = ' ';
                gamefield[y3, x3] = ' ';
                gamefield[y4, x4] = ' ';
                x1++; x2++; x3++; x4++;
                gamefield[y1, x1] = '#';
                gamefield[y2, x2] = '#';
                gamefield[y3, x3] = '#';
                gamefield[y4, x4] = '#';
                figview();
            }
        }
        static void downArrow()
        {
            droptime = 50;
        }
        static void upArrow() //поворот
        {
            rotates++;
            gamefield[y1, x1] = ' ';
            gamefield[y2, x2] = ' ';
            gamefield[y3, x3] = ' ';
            gamefield[y4, x4] = ' ';
            if (fignum == 1 && y2 != 0 && x2 != 0 && x2 != 11) //фігура I;
            {
                switch (rotates)
                {
                    case 1:
                        y1++; x1--;
                        y3--; x3++;
                        y4 -= 2; x4 += 2;
                        break;
                    case 2:
                        y1--; x1++;
                        y3++; x3--;
                        y4 += 2; x4 -= 2;
                        rotates = 0;
                        break;
                }
            }
            else if (fignum == 1 && (y2 == 0 || x2 == 0 || x2 == 11)) rotates--;
            else if (fignum == 3 && y2 != 0 && x2 != 0 && x2 != 11) //фігура T;
            {
                switch (rotates) 
                {
                    case 1:
                    y1--; x1++;
                    y3++; x3--;
                    y4--; x4--;
                        break;
                    case 2:
                        y1++; x1++;
                        y3--; x3--;
                        y4--; x4++;
                        break;
                    case 3:
                        y1++; x1--;
                        y3--; x3++;
                        y4++; x4++;
                        break;
                    case 4:
                        y1--; x1--;
                        y3++; x3++;
                        y4++; x4--;
                        rotates = 0;
                        break;
                }
            }
            else if (fignum == 3 && (y2 == 0 || x2 == 0 || x2 == 11)) rotates--;
            else if (fignum == 4 && y2 != 0 && x2 != 0 && x2 != 11) //фігура J
            {
                switch(rotates)
                {
                    case 1:
                        y1++; x1++;
                        y3--; x3--;
                        y4 -= 2;
                        break;
                    case 2:
                        y1++; x1--;
                        y3--; x3++;
                        x4 += 2;
                        break;
                    case 3:
                        y1--; x1--;
                        y3++; x3++;
                        y4 += 2;
                        break;
                    case 4:
                        y1--; x1++;
                        y3++; x3--;
                        x4 -= 2;
                        rotates = 0;
                        break;
                }
            }
            else if (fignum == 4 && (y2 == 0 || x2 == 0 || x2 == 11)) rotates--;
            else if (fignum == 5 && y2 != 0 && x2 != 0 && x2 != 11) //фігура L
            {
                switch (rotates)
                {
                    case 1:
                        y1++; x1++;
                        y3--; x3--;
                        x4 -= 2;
                        break;
                    case 2:
                        y1++; x1--;
                        y3--; x3++;
                        y4 -= 2;
                        break;
                    case 3:
                        y1--; x1--;
                        y3++; x3++;
                        x4 += 2;
                        break;
                    case 4:
                        y1--; x1++;
                        y3++; x3--;
                        y4 += 2;
                        rotates = 0;
                        break;
                }
            }
            else if (fignum == 5 && (y2 == 0 || x2 == 0 || x2 == 11)) rotates--;
            else if (fignum == 6 && y2 != 0 && x2 != 0 && x2 != 11) //фігура Z;
            {
                switch (rotates)
                {
                    case 1:
                        y1++; x1++;
                        y3++; x3--;
                        x4 -= 2;
                        break;
                    case 2:
                        y1--; x1--;
                        y3--; x3++;
                        x4 += 2;
                        rotates = 0;
                        break;
                }
            }
            else if (fignum == 6 && (y2 == 0 || x2 == 0 || x2 == 11)) rotates--;
            else if (fignum == 7 && y2 != 0 && x2 != 0 && x2 != 11) //фігура reverse Z;
            {
                switch (rotates)
                {
                    case 1:
                        y1++; x1++;
                        y3--; x3++;
                        y4 -= 2;
                        break;
                    case 2:
                        y1--; x1--;
                        y3++; x3--;
                        y4 += 2;
                        rotates = 0;
                        break;
                }
            }
            else if (fignum == 7 && (y2 == 0 || x2 == 0 || x2 == 11)) rotates--;
            gamefield[y1, x1] = '#';
            gamefield[y2, x2] = '#';
            gamefield[y3, x3] = '#';
            gamefield[y4, x4] = '#';
            figview();
        }
        static void drawfield()
        {
            for (int i = 0; i < 18; i++)
            {
                Console.SetCursorPosition(12, i);
                Console.WriteLine('|');
            }
            Console.SetCursorPosition(0, 18);
            Console.Write("------------");
        }
        static void tetrisinit()
        {
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    gamefield[i, j] = ' ';
                }
            }
        }
        static bool isverticalFixed()
        {
            if (fignum == 1)
            {
                if (rotates == 0 && gamefield[y4 + 1, x4] == '#') return true;
                else if (rotates == 1 && (gamefield[y1 + 1, x1] == '#' || gamefield[y2 + 1, x2] == '#' || gamefield[y3 + 1, x3] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else return false;
            }
            else if(fignum == 2)
            {
                if (gamefield[y3 + 1, x3] == '#' || gamefield[y4 + 1, x4] == '#') return true;
                else return false;
            }
            else if (fignum == 3)
            {
                if (rotates == 0 && (gamefield[y4 + 1, x4] == '#' || gamefield[y1 + 1, x1] == '#' || gamefield[y3 + 1, x3] == '#')) return true;
                else if (rotates == 1 && (gamefield[y3 + 1, x3] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else if (rotates == 2 && (gamefield[y1 + 1, x1] == '#' || gamefield[y2 + 1, x2] == '#' || gamefield[y3 + 1, x3] == '#')) return true;
                else if (rotates == 3 && (gamefield[y1 + 1, x1] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else return false;
            }
            else
            if (fignum == 4)
            {
                if (rotates == 0 && (gamefield[y4 + 1, x4] == '#' || gamefield[y3 + 1, x3] == '#')) return true;
                else if (rotates == 1 && (gamefield[y3 + 1, x3] == '#' || gamefield[y2 + 1, x2] == '#' || gamefield[y1 + 1, x1] == '#')) return true;
                else if (rotates == 2 && (gamefield[y1 + 1, x1] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else if (rotates == 3 && (gamefield[y1 + 1, x1] == '#' || gamefield[y2 + 1, x2] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else return false;
            }
            else
            if (fignum == 5)
            {
                if (rotates == 0 && (gamefield[y4 + 1, x4] == '#' || gamefield[y3 + 1, x3] == '#')) return true;
                else if (rotates == 1 && (gamefield[y1 + 1, x1] == '#' || gamefield[y2 + 1, x2] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else if (rotates == 2 && (gamefield[y1 + 1, x1] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else if (rotates == 3 && (gamefield[y1 + 1, x1] == '#' || gamefield[y2 + 1, x2] == '#' || gamefield[y3 + 1, x3] == '#')) return true;
                else return false;
            }
            else
            if (fignum == 6)
            {
                if (rotates == 0 && (gamefield[y4 + 1, x4] == '#' || gamefield[y2 + 1, x2] == '#')) return true;
                else if (rotates == 1 && (gamefield[y1 + 1, x1] == '#' || gamefield[y3 + 1, x3] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else return false;
            }
            else
            if (fignum == 7)
            {
                if (rotates == 0 && (gamefield[y4 + 1, x4] == '#' || gamefield[y2 + 1, x2] == '#')) return true;
                else if (rotates == 1 && (gamefield[y1 + 1, x1] == '#' || gamefield[y2 + 1, x2] == '#' || gamefield[y4 + 1, x4] == '#')) return true;
                else return false;
            }
            else return false;
        }
        static void falling()
        {
            fix = false;
            figview();
            Thread.Sleep(droptime);

            while (y1 != 17 && y2 != 17 && y3 != 17 && y4 != 17)
            {
                Thread.Sleep(droptime);
                if (isverticalFixed() == true)
                {
                    for (int f = 0; f < 12; f++)
                    {
                        if (gamefield[0, f] == '#')
                        {
                            gameover = true;
                            break;
                        }
                    }

                    break;
                }
                gamefield[y1, x1] = ' ';
                gamefield[y2, x2] = ' ';
                gamefield[y3, x3] = ' ';
                gamefield[y4, x4] = ' ';
                y1++; y2++; y3++; y4++;
                gamefield[y1, x1] = '#';
                gamefield[y2, x2] = '#';
                gamefield[y3, x3] = '#';
                gamefield[y4, x4] = '#';
                figview();
            }
            fix = true;
            droptime = 600;
        }
        static void gameOver()
        {
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    gamefield[i, j] = ' ';
                }
            }
            figview();
            gameover = true;
        }
        static void fullCleaner()
        {
            fulliterator = 0;
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (gamefield[i, j] == '#') fulliterator++;
                }
                if (fulliterator == 12)
                {
                    for (int k = 0; k < 12; k++) gamefield[i, k] = ' ';
                    if (i > 0)
                    {
                        tmp = i;
                        for (int m = tmp - 1; m >= 0; m--)
                        {
                            for (int g = 0; g < 12; g++)
                            {
                                gamefield[m + 1, g] = gamefield[m, g];
                            }
                        }
                    }
                }
                fulliterator = 0;
            }
        }
        static void backgroundTetris()
        {
            while (!gameover)
            {
                Console.Clear();
                drawfield();
                if (fix == true)
                {
                    fullCleaner();
                    figselect();
                    figinstall();
                }
                falling();
                if (gameover) gameOver();
            }
        }
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            tetrisinit();
            backgroundGame.Start();
            backgroundGame.IsBackground = true;
            while (!gameover)
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.RightArrow) rightArrow();
                else if (keyinfo.Key == ConsoleKey.LeftArrow) leftArrow();
                else if (keyinfo.Key == ConsoleKey.DownArrow) downArrow();
                else if (keyinfo.Key == ConsoleKey.UpArrow) upArrow();
            }
        }
    }
}
