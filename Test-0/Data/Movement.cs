using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_0.Data
{
    public class Movement
    {
        public string Date { get; set; }

        public string Object_A { get; set; }

        public string Type_A { get; set; }

        public string Object_B { get; set; }

        public string Type_B { get; set; }

        public string Direction { get; set; }

        public string Color { get; set; }

        public int Intensity { get; set; }

        public double LatitudeA { get; set; }

        public double LongitudeA { get; set; }

        public double LatitudeB { get; set; }

        public double LongitudeB { get; set; }
        public ObservableCollection<Movement> WallItems { get; set; }

        //public static explicit operator Movement(object v)
        //{
        //    throw new NotImplementedException();
        //}

        //public static Movement[] GetClass1()
        //{
        //    var result = new Movement[]
        //        {
        //            new Movement() { Last = "B", First = "C"},
        //            new Movement() { Last = "A", First = "B"},
        //            new Movement() { Last = "A", First = "C"}
        //        };
        //    return result;
        //}
    }
}