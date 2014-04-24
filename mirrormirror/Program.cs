using System;
using System.Linq;

namespace mirrormirror
{
    class Program
    {
        static void Main(string[] args)
        {
            var one = new A
            {
                colone = 1,
                coltwo = 2,
                colthree = 3,
                colfour = 4
            };

            var two = new B();

            PrettyPrint(one);
            PrettyPrint(two);

            Console.WriteLine("init two");

            two.init(one);

            PrettyPrint(one);
            PrettyPrint(two);

            Console.ReadLine();
        }

        static void PrettyPrint<T>(T t)
        {
            var props = t.GetType().GetProperties();

            foreach(var prop in props)
            {
                var val = prop.GetValue(t, null);
                Console.WriteLine("Prop: " + prop.Name + " Val: " + val);
            }

            Console.WriteLine();
        }
    }

    class A
    {
        public int colone { get; set; }
        public int coltwo { get; set; }
        public int colthree { get; set; }
        public int colfour { get; set; }
    }

    class B : Dto
    {
        public int colone { get; set; }
        public int coltwo { get; set; }
        public string hello { get; set; }
    }

    abstract class Dto
    {
        public void init<T>(T t)
        {
            var otherprops = t.GetType().GetProperties();
            var thisprops = this.GetType().GetProperties();

            foreach (var prop in thisprops)
            {
                if (prop.CanWrite)
                {
                    var otherprop = otherprops.SingleOrDefault(x => x.Name == prop.Name);
                    if (otherprop != null && otherprop.GetType() == prop.GetType())
                    {
                        prop.SetValue(this, otherprop.GetValue(t, null));
                    }
                }
            }
        }
    }
}
