using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace The_Game_of_Life_2
{
    class Board
    {
        public int width = 78;
        public int height = 36;
        public int scrollValue = 1;
        public List<List<bool>> board = new List<List<bool>>();

        public Board()
        {
            resize();
        }

        public void resize()
        {
            board.Clear();

            for (int i = 0; i < width; i++)
            {
                List<bool> newCol = new List<bool>();
                board.Add(newCol);
                for (int j = 0; j < height; j++)
                {
                    board[i].Add(false);

                }
            }
        }

        public void Tick()
        {
            List<List<bool>> newBoard = DeepCopy(board);

            for(int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(board[i][j])
                    {
                        if (getNumAdjacent(i,j) <= 1 || getNumAdjacent(i,j) >= 4)
                        {
                            newBoard[i][j] = false;
                        }
                    }
                    else
                    {
                        if(getNumAdjacent(i,j) == 3)
                        {
                            newBoard[i][j] = true;
                        }
                    }
                    
                }
            }
            board = newBoard;
        }

        public int getNumAdjacent(int x, int y)
        {
            int numAdj = 0;

            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) 
                        continue;

                    if (y + j < 0 || x + i < 0 || y + j >= height ||x + i >= width)
                    {

                    }
                    else
                    {
                        if( board[x+i][y+j] )
                        {
                            numAdj++;
                        }
                    }
                }
            }

            if(numAdj != 0)
                Console.WriteLine(numAdj);
            return numAdj;


        }

        public void changeSize(int h, int w)
        {
            width = h;
            height = w;

            resize();
        }

        public static T DeepCopy<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }

    }

    
}

