using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils;

public class CallbackCollector<TReturn>
{
    public TReturn latestReturn { get; private set; }

    public CallbackCollector(TReturn initial)
    {
        latestReturn = initial;
    }

    public Action<T> cb<T>(Func<T, TReturn> action)
    {
        var newCallback = (T item) =>
        {
            latestReturn = action(item);
        };
        return newCallback;
    }

    public Action cb(Func<TReturn> action)
    {
        var newCallback = () =>
        {
            
            latestReturn = action();
        };
        return newCallback;
    }
}
