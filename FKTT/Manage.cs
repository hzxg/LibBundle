using LibBundle;
using LibBundle.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FKTT
{
    class Manage
    {
        //  public static IndexContainer mainic, ic;

        public static readonly HashSet<BundleRecord> changed = new HashSet<BundleRecord>();
        public static List<BundleRecord> loadedBundles = new List<BundleRecord>();
    }
}
