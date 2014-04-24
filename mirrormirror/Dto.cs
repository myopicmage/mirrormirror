using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mirrormirror
{
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
