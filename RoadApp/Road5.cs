using Sentia.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadApp
{




    

    public class Road5
    {

        public class Segment
        {
            public int Colour { get; set; }
            public int Size { get; set; }
            public int Position { get; set; }
            public float Curve { get; set; }
        }


        int m_Size = 11;
        int[] m_Lines = new int[112];
        int[] m_HLines = new int[112];
        int[] m_Index = new int[112];
        List<Segment> m_Segments = new List<Segment>();
        int m_Position = 0;
        int m_Offset = 0;
        int m_CurrentSegment;




        int m_CurrentColour = 0;



        private void AddCurve(int count, float curve)
        {
            for (int i = 0; i < count; i++)
            {
                AddSegment(TimeCurves.EaseIn(0, curve, i / (float)count));
            }
            for (int i = 0; i < count * 2; i++)
            {
                AddSegment(curve);
            }
            for (int i = 0; i < count; i++)
            {
                AddSegment(TimeCurves.EaseIn(0, curve, 1.0f -( i / (float)count)));
            }
        }


        private void AddSegment(float curve)
        {
            m_Segments.Add(new Segment() { Size = m_Size, Colour = m_CurrentColour, Curve = curve });
            m_CurrentColour = m_CurrentColour == 0 ? 1 : 0;
        }




        public Road5(ref Func<int, Tuple<int,int>> hInteruptCallback, ref Action updateCallback)
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


            m_CurrentColour = 0;

            AddCurve(10, 0);
            AddCurve(4, 1);            
            AddCurve(4, -1);
            AddCurve(3, 1);
            AddCurve(3, -1);
            AddCurve(3, 1);
            AddCurve(3, -1);
            AddCurve(10, 0);
            AddCurve(14, 1);
            AddCurve(9, -1);

            for(i=0;i<m_Segments.Count;i++)
            {
               Console.WriteLine($"m_Segments[{i}] = (Segment){{{(m_Segments[i].Colour == 0? 16 : - 112)},FIX16({m_Segments[i].Curve})}};");
            }
        }


        
        public Tuple<int, int> Hint(int line)
        {
            if (line > 111)
            {
                return new Tuple<int, int>(m_Lines[line - 112], m_HLines[line - 112]);
            }
            return new Tuple<int,int>(-1, 0);
        }



        public void Update()
        {
            m_Position++;
            m_Offset++;

            
            if (m_Offset == m_Size)
            {
                m_Offset = 0;
                m_CurrentSegment++;
                if (m_CurrentSegment >= m_Segments.Count)
                {
                    m_CurrentSegment = 0;
                    m_Position = 0;
                }
            }


            float hOffset = 0.0f;
            int indexSegment = m_CurrentSegment;

            float dx = 1.0f;            
            for (int i = 111; i >= 0; --i)
            {
                int pos = m_Index[i] + m_Position;
                int index = (pos / m_Size) % m_Segments.Count;
                if (index >= m_Segments.Count)
                {
                    index = index % m_Segments.Count;
                }
                
                m_Lines[i] = m_Segments[index].Colour;

                m_HLines[i] = (int)hOffset;

                float timeCurve = TimeCurves.EaseOut(1.0f, 1.0f, 1.0f - (i / 111.0f));

                hOffset += (m_Segments[index].Curve * timeCurve);
                //dx += 0.03f;
            }
        }
    }
}
