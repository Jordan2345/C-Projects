using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertiesPractice
{
    class Box
    {
        private int length;
        private int width;
        private int height;

        //create constructor
        public Box(int length, int width, int height)
        {
            this.length = length;
            this.width = width;
            this.height = height;
        }
        //create properties
        public int Length { get=>length; set=>length=value; }
        public int Width { get=>width; set=>width=value; }
        public int Height { get=>height; set=>height=value; }
        public int Volume { get=>Length*Width*Height;}//read only
        public int FrontSurface { get=>Length*Height;}//read only

        //create method/s
        public void DisplayInfo()
        {
            Console.WriteLine("The length is {0}, the width is {1}, and the height is {2}",length,width,height);
        }
    }
}
