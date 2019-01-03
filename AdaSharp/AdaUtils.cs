using System;
using System.Collections.Generic;
using System.Text;

namespace AdaSharp
{
    public enum Units
    {
        ADA, Lovelace
    }

    public static class AdaUtils
    {
        public static decimal ToLovelace(decimal amount, Units fromUnits)
        {
            switch (fromUnits)
            {
                case Units.ADA:
                    return amount * 1_000_000;
                case Units.Lovelace:
                    return amount;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fromUnits));
            }
        }

        public static decimal FromLovelace(decimal amount, Units toUnits)
        {
            switch (toUnits)
            {
                case Units.ADA:
                    return amount / 1_000_000;
                case Units.Lovelace:
                    return amount;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toUnits));
            }
        }
    }
}
