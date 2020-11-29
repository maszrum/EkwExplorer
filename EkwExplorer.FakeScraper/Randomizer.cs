using System;
using System.Collections.Generic;

namespace EkwExplorer.FakeScraper
{
    internal class Randomizer<T>
    {
        private readonly List<T> _elements = new List<T>();
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public Randomizer<T> AddElement(T element)
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
    }
}
