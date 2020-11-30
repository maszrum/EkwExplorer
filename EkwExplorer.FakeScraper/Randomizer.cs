using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EkwExplorer.FakeScraper
{
    internal class Randomizer<T> : IEnumerable<T>
    {
        private readonly List<T> _elements = new List<T>();
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public Randomizer<T> Add(T element)
        {
            _elements.Add(element);

            return this;
        }

        public T Next()
        {
            if (!_elements.Any())
            {
                throw new InvalidOperationException(
                    "no item has been added");
            }

            var index = _random.Next(_elements.Count);
            return _elements[index];
        }

        public IEnumerator<T> GetEnumerator() =>
            _elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
