using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace mirrormirror
{
    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new List<A>
            {
                new A
                {
                    colfour = 4,
                    colone = 1,
                    colthree = 3,
                    coltwo = 2
                },
                new A
                {
                    coltwo = 4,
                    colthree = 6,
                    colone = 2,
                    colfour = 8
                },
                new A
                {
                    colfour = 12,
                    colone = 3,
                    colthree = 9,
                    coltwo = 6
                }
            };

            var list2 = new List<B>
            {
                new B
                {
                    coltwo = 2,
                    hello = "hello",
                    mom = "mom"
                },
                new B
                {
                    coltwo = 2,
                    hello = "dolly",
                    mom = "brenda"
                }
            };

            List<C> list3;
            list1.JoinToDto(list2, "coltwo", out list3);

            foreach (var item in list3)
            {
                PrettyPrint(item);
            }

            Console.WriteLine("Reached end");
            Console.ReadLine();
        }

        static void DtoTest()
        {
            var one = new A
            {
                colone = 1,
                coltwo = 2,
                colthree = 3,
                colfour = 4
            };

            var two = new B
            {
                hello = "hello",
                mom = "mom"
            };

            var three = new C();

            PrettyPrint(one);
            PrettyPrint(two);
            PrettyPrint(three);

            three.initMult(one, two);

            PrettyPrint(one);
            PrettyPrint(two);
            PrettyPrint(three);

            Console.ReadLine();
        }

        static void PrettyPrint<T>(T t)
        {
            var props = t.GetType().GetProperties();

            foreach (var prop in props)
            {
                var val = prop.GetValue(t, null);
                Console.WriteLine("Prop: " + prop.Name + " Val: " + val);
            }

            Console.WriteLine();
        }
    }
}
