using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadApp
{
    public class Road1
    {

        List<int> m_Bands = new List<int>();
        int m_color = 0;

        int m_Current = 0;
        public Road1(ref Func<int, int> hInteruptCallback, ref Action updateCallback)
        {
            hInteruptCallback = Hint;
            updateCallback = Update;

            int size = 1;
            int place = 112;

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



        public void Update()
        {
            m_Current = 0;
            m_color = 0;
        }


    }
}
