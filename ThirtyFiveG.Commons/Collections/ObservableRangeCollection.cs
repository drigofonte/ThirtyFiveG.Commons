using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ThirtyFiveG.Commons.Event;

namespace ThirtyFiveG.Commons.Collections
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>, IObservableRangeCollection
    {
        #region Private variables
        private bool _suppressCollectionChangedEvent;
        #endregion

        #region Public events
        public event EventHandler<DataEventArgs<IEnumerable>> Added;
        public event EventHandler<DataEventArgs<IEnumerable>> Removed;
        #endregion

        #region Constructor
        public ObservableRangeCollection()
        {
            _suppressCollectionChangedEvent = false;
        }
        #endregion

        #region Public methods
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            _suppressCollectionChangedEvent = true;
            try
            {
                foreach (T item in items)
                    Add(item);
                Added?.Invoke(this, new DataEventArgs<IEnumerable>(items));
            }
            finally
            {
                _suppressCollectionChangedEvent = false;
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            _suppressCollectionChangedEvent = true;
            try
            {
                foreach (T item in items)
                    Remove(item);
                Removed?.Invoke(this, new DataEventArgs<IEnumerable>(items));
            }
            finally
            {
                _suppressCollectionChangedEvent = false;
            }
        }
        #endregion

        #region Overrides
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressCollectionChangedEvent)
                base.OnCollectionChanged(e);
        }
        #endregion
    }
}