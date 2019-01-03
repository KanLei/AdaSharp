using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp
{
    public static class Extensions
    {
        public static string GetName(this Enum @enum)
        {
            return Enum.GetName(@enum.GetType(), @enum);
        }
    }
}
