using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Content
{
    public static class ContentDirectory
    {
        public const string contentDirectory =
#if DEBUG
            "../../../../../"
#else
            "./"
#endif 
            + "content";
    }
}
