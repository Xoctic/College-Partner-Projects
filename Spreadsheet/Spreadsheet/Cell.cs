using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{  
        public struct cell
        {
            public object content { get; set; }
            public object value { get; set; }

            public Type type { get; set; }

            

            public cell(object _content, object _value, Type _t)
            { 
                content = _content;
                value = _value;
                type = _t;
            }
        }
}
