using System;
using System.Linq;

namespace FFTransformace
{
    public class Generator
    {
        private readonly int _size;
        public Generator(int size)
        {
            this._size = size;
        }
        
        private enum Ways
        {
            Up,
            Down,
            Right,
            Left
        }
        
        public int[,] Generate()
        {
            int[,] array = new int[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                array[i, 0] = 1;
                array[0, i] = 1;
                array[i, _size - 1] = 1;
                array[_size - 1, i] = 1;
            }

            for (int y = 2; y < _size - 2; y += 2)
            for (int x = 2; x < _size - 2; x += 2)
            {
                array[x, y] = 2;
            }

            while (array.Cast<int>().Where(x => x == 2).Count() > 0)
            {
                while (true)
                {
                    int randomX = _R.Next(_size), randomY = _R.Next(_size);
                    if (array[randomX, randomY] == 2)
                    {
                        array[randomX, randomY] = 1;
                        Ways way = RandomWay();
                        while (true)
                        {
                            switch (way)
                            {
                                case Ways.Down:
                                    randomY++;
                                    break;
                                case Ways.Up:
                                    randomY--;
                                    break;
                                case Ways.Right:
                                    randomX++;
                                    break;
                                case Ways.Left:
                                    randomX--;
                                    break;
                            }

                            if (array[randomX, randomY] != 1)
                            {
                                array[randomX, randomY] = 1;
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;
                    }
                }
            }


            return array;
        }

        private static Random _R = new Random();

        private static Ways RandomWay()
        {
            var v = Enum.GetValues(typeof(Ways));
            return (Ways) v.GetValue(_R.Next(v.Length));
        }
    }
}