using System;

namespace ThirtyFiveG.Commons.Event
{
    public class DataEventArgs<T> : EventArgs
    {
        public DataEventArgs(T data)
        {
            Data = data;
        }

        public T Data { get; private set; }
    }
}
