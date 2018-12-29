using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            Box box = new Box(7,4,2);
            box.DisplayInfo();
            //change length,width,and height
            box.Length = 2;
            box.Width = 2;
            box.Height = 3;
            box.DisplayInfo();
            Console.WriteLine("The volume of the box is:  {0}",box.Volume);
            Console.WriteLine("The Front Surface of the box is:  {0}",box.FrontSurface);
            Console.Read();
        }
    }
}
