using System;
using System.Collections;
using ThirtyFiveG.Commons.Event;

namespace ThirtyFiveG.Commons.Collections
{
    public interface IObservableRangeCollection
    {
        event EventHandler<DataEventArgs<IEnumerable>> Added;
        event EventHandler<DataEventArgs<IEnumerable>> Removed;
    }
}
