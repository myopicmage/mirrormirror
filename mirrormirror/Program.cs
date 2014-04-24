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

    class A
    {
        public int colone { get; set; }
        public int coltwo { get; set; }
        public int colthree { get; set; }
        public int colfour { get; set; }
    }

    class B
    {
        public int coltwo { get; set; }
        public string hello { get; set; }
        public string mom { get; set; }
    }

    class C : Dto
    {
        public int colone { get; set; }
        public int coltwo { get; set; }
        public string hello { get; set; }
    }

    public static class Extension
    {
        /* Create a DTO type you want to join into
        *  Create a list with that type
        *  See main for usage
        */
        public static void JoinToDto<T, R, S>(this List<T> list1, List<R> list2, string joinon, out List<S> newlist) where S : new()
        {
            if (list1 == null || list2 == null)
            {
                throw new ArgumentNullException("Lists cannot be null");
            }

            if (string.IsNullOrWhiteSpace(joinon))
            {
                throw new ArgumentNullException("You must specify a property to join on");
            }

            newlist = new List<S>();

            foreach (var item in list1)
            {
                var itemprops = item.GetType().GetProperties();
                var propval = item.GetType().GetProperty(joinon).GetValue(item, null);
                var joinlist = list2.Where(x => x.GetType().GetProperty(joinon).GetValue(x, null).Equals(propval));

                if (joinlist != null)
                {
                    foreach (var joined in joinlist)
                    {
                        var toadd = new S();

                        var otherprops = item.GetType().GetProperties();
                        foreach (var prop in toadd.GetType().GetProperties())
                        {
                            if (prop.CanWrite)
                            {
                                var otherprop = otherprops.SingleOrDefault(x => x.Name == prop.Name);
                                if (otherprop != null && otherprop.GetType() == prop.GetType())
                                {
                                    prop.SetValue(toadd, otherprop.GetValue(item, null));
                                }
                            }
                        }

                        otherprops = joined.GetType().GetProperties();
                        foreach (var prop in toadd.GetType().GetProperties())
                        {
                            if (prop.CanWrite)
                            {
                                var otherprop = otherprops.SingleOrDefault(x => x.Name == prop.Name);
                                if (otherprop != null && otherprop.GetType() == prop.GetType())
                                {
                                    prop.SetValue(toadd, otherprop.GetValue(joined, null));
                                }
                            }
                        }

                        newlist.Add(toadd);
                    }
                }
            }
        }
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

        public void initMult<T, R>(T t, R r)
        {
            init(t);
            init(r);
        }
    }
}
