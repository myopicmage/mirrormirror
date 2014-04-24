using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mirrormirror
{
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
}
