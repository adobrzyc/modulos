using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    public static class KindUtils
    {
        private static IEnumerable<Kind> kinds;
        private static IEnumerable<KindInfo> kindsWithGroup;

        public static IEnumerable<KindInfo> KindInformation
        {
            get
            {
                return kindsWithGroup ?? (kindsWithGroup = Enum.GetValues(typeof(Kind))
                           .OfType<Kind>()
                           .Select(e => new KindInfo(e))).ToArray();
            }
        }

        public static IEnumerable<Kind> Kinds => kinds ?? (kinds = Enum.GetValues(typeof(Kind))
                                                     .OfType<Kind>()).ToArray();
    }
}