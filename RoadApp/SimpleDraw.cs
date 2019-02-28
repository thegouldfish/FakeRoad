using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadApp
{
    public class SimpleDraw
    {

        public SimpleDraw(ref Func<int, int> hInteruptCallback, ref Action updateCallback)
        {
            hInteruptCallback = Hint;
            updateCallback = Update;
        }



        public int Hint(int line)
        {
            if (line % 4 == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }



        public void Update()
        {

        }

    }
}
