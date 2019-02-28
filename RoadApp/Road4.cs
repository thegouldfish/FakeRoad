using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadApp
{




    

    public class Road4
    {
        public class Segment
        {
            public int Colour { get; set; }
            public int Size { get; set; }
            public int Position { get; set; }
        }

        int m_Size = 11;
        int[] m_Lines = new int[112];
        int[] m_Index = new int[112];
        List<Segment> m_Segments = new List<Segment>();
        int m_Position = 0;
        int m_Offset = 0;
        int m_CurrentSegment;

        public Road4(ref Func<int, int> hInteruptCallback, ref Action updateCallback)
        {




            hInteruptCallback = Hint;
            updateCallback = Update;

            float size = 1.0f;

            int i = 111;
            int place = -1;
            do
            {
                m_Index[i] = place + (int)size;
                place++;

                if (place > 20)
                {
                    size += size * 0.06f;
                }
                Console.WriteLine($"i {i} index {m_Index[i]}");
                i--;
            } while (i >= 0);


            place = 0;
            int col = 0;
            for (i = 0; i < 50; i++)
            {
                m_Segments.Add(new Segment() { Position = place, Size = m_Size, Colour = col });
                col = col == 0 ? 1 : 0;
                place += m_Size;
            }

            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("-=-=-=-=-=-=-=-");

            Console.WriteLine("");
            Console.WriteLine("");
        }


        
        public int Hint(int line)
        {
            if (line > 111)
            {
                return m_Lines[line - 112];
            }
            return -1;
        }



        public void Update()
        {
            m_Position++;
            m_Offset++;
            if (m_Offset == m_Size)
            {                
                m_CurrentSegment++;
                if (m_CurrentSegment >= m_Segments.Count)
                {
                    m_CurrentSegment = 0;
                    m_Position = 0;
                }
            }


            int indexSegment = m_CurrentSegment;
            for (int i = 0; i < 112; i++)
            {
                int pos = m_Index[i] + m_Position;
                int index = (pos / m_Size) % m_Segments.Count;
                if (index >= m_Segments.Count)
                {
                    index = index % m_Segments.Count;
                }

                m_Lines[i] = m_Segments[index].Colour;
            }
        }
    }
}
