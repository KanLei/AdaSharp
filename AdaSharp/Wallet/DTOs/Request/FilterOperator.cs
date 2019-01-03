using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp.Wallet.DTOs.Request
{
    public enum FilterOperator
    {
        None,
        EQ,     // EQ[value] : only allow values equal to value
        LT,     // LT[value] : allow resource with attribute less than the value
        GT,     // GT[value] : allow objects with an attribute greater than the value
        GTE,    // GTE[value] : allow objects with an attribute at least the value
        LTE,    // LTE[value] : allow objects with an attribute at most the value
        RANGE,  // RANGE[lo,hi] : allow objects with the attribute in the range between lo and hi
        IN      // IN[a,b,c,d] : allow objects with the attribute belonging to one provided
    }
}
