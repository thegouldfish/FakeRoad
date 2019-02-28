using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadApp
{
    public class Road3
    {
        class Seg
        {
            public int Place { get; set; }
            public int Size { get; set; }
            public int Color { get; set; }
        }

        List<Seg> m_Segments = new List<Seg>();


        List<int> m_Bands = new List<int>();
        int m_color = 0;
        int m_Current = 0;


        int m_place = 0;
        int m_CurrentSegment = 0;
        int m_Progress = 0;
        int sSize = 9;

        public Road3(ref Func<int, int> hInteruptCallback, ref Action updateCallback)
        {
            hInteruptCallback = Hint;
            updateCallback = Update;

            int col = 0;
            int place = sSize;
            for (int i = 0; i < 30; i++)
            {
                m_Segments.Add(new Seg() { Place=place, Size = sSize, Color= 0 });
                m_Segments.Add(new Seg() { Place = place +sSize, Size = sSize, Color=1 });

                place += (sSize * 2);                
            }         
        }


        public void GenerateBands()
        {
            int place =  224 - (sSize - m_Progress);
            int col = m_Segments[m_CurrentSegment].Color == 0? 1:0 ;

            int size = sSize;
            m_Bands.Clear();
            
            m_Bands.Add(place);
            m_Bands.Insert(0,place - sSize);
            place = place - (size * 2);
            size--;
            int count = m_CurrentSegment + 1;
            do
            {
                m_Bands.Insert(0, place);
                m_Bands.Insert(0, place - size);
                place = place - (size * 2);
                size--;
                if (size < 1)
                {
                    size = 1;
                }

                col = col == 0 ? 1 : 0;
                col = col == 0 ? 1 : 0;
                count += 2;
            } while (place > 112);

            m_color = col;
            //m_color = m_Segments[count-1].Color == 0 ? 1: 0;
            if (m_Bands[0] > 112)
            {
                m_Bands[0] = 112;
            }

            m_Bands.Add(226);
        }


        public int Hint(int line)
        {
            
            if (line == m_Bands[m_Current])
            {
                int col = m_color;
                m_color = (m_color == 0) ? 1 : 0;

                m_Current++;

                return col;
            }

            return -1;
        }

        public void Increment()
        {
            m_Progress++;

            if (m_Progress >= sSize)
            {
                m_Progress = 1;
                m_CurrentSegment++;
            }
        }

        public void Update()
        {
            Increment();
            GenerateBands();
            m_Current = 0;
            //m_color = 0;
        }


    }
}
