using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadApp
{
    public class Road2
    {

        List<int> m_Bands = new List<int>();
        int m_color = 0;

        int m_Current = 0;
        int m_Place = 112;

        int[] sizes = new int[112];

        public Road2(ref Func<int, int> hInteruptCallback, ref Action updateCallback)
        {
            hInteruptCallback = Hint;
            updateCallback = Update;

            int size = 1;
            for (int i = 0; i < sizes.Length / 2; i+=2)
            {
                sizes[i] = size;
                sizes[i+1] = size;
                size++;
            }
        }



        public int Hint(int line)
        {
            if (m_Current < m_Bands.Count)
            {
                if (line == m_Bands[m_Current])
                {
                    int col = m_color;
                    m_color = (m_color == 0) ? 1 : 0;

                    m_Current++;
                    return col;
                }
            }
            return -1;
        }

        public void Update()
        {
            m_Place++;
            if (m_Place >= 224)
            {
                m_Place = 112;
            }

            int place = m_Place;
            int size = sizes[place-112];

            m_Bands.Clear();
            do
            {
                m_Bands.Add(place);
                m_Bands.Add(place + size);
                place += (size * 2);
                size++;

                if (place > 224)
                {
                    break;
                }
            } while (true);

            m_Current = 0;
            m_color = 0;
        }


    }
}
