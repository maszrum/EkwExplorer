using System;
using System.Collections.Generic;

namespace EkwExplorer.FakeScraper
{
    internal class WeightedRandomizer<T>
    {
        private readonly List<WeightedElement> _elements = new List<WeightedElement>();
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        private int _weightsSum;

        public WeightedRandomizer<T> AddElement(T element, int weight)
        {
            if (weight <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weight), "must be positive number");
            }

            _elements.Add(new WeightedElement(element, weight));
            _weightsSum += weight;

            return this;
        }

        public T Next()
        {
            var randomValue = _random.Next(_weightsSum);

            foreach (var element in _elements)
            {
                randomValue -= element.Weight;

                if (randomValue < 0)
                {
                    return element.Element;
                }
            }

            throw new InvalidOperationException(
                "random element was not found");
        }

        private class WeightedElement
        {
            public WeightedElement(T element, int weight)
            {
                Element = element;
                Weight = weight;
            }

            public T Element { get; }
            public int Weight { get; }
        }
    }
}
