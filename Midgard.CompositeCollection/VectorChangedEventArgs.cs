using System;
using Windows.Foundation.Collections;

namespace Midgard.CompositeCollection
{
    internal class VectorChangedEventArgs : IVectorChangedEventArgs
    {
        public CollectionChange CollectionChange { get; set; }

        public uint Index { get; set; }
    }
}