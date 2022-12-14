using System;
using System.Collections;

namespace Foundation {
    /// <summary>
    /// A half-open interval from a lower bound up to, but not including, an upper bound.
    /// </summary>
    [Serializable]
    public readonly struct Range<Bound> : IEquatable<Range<Bound>>, CustomStringConvertible where Bound : IComparable<Bound>, IEquatable<Bound> {
        /// <summary>
        /// The range’s lower bound.
        /// </summary>
        public readonly Bound lowerBound;

        /// <summary>
        /// The range’s upper bound.
        /// </summary>
        public readonly Bound upperBound;

        //public bool isEmpty => lowerBound.Equals(upperBound);

        public string description => $"Range<{typeof(Bound)}>({lowerBound.ToString()} ..< {upperBound.ToString()})";

        public Range(in Bound lowerBound, in Bound upperBound) {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        /// <summary>
        /// Returns a <see langword="bool"/> value indicating whether the given element is contained within the range.
        /// </summary>
        /// <remarks>
        /// Because <see cref="Range"/> represents a half-open range, a <see cref="Range"/> instance does not contain its upper bound. element is contained in the range if it is greater than or equal to the lower bound and less than the upper bound.
        /// </remarks>
        /// <param name="value">The element to check for containment.</param>
        /// <returns><see langword="true"/> if <paramref name="element"/> is contained in the range; otherwise, <see langword="false"/>.</returns>
        public bool Contains(in Bound element) => (element.CompareTo(lowerBound) >= 0) && (element.CompareTo(upperBound) < 0);

        public bool Contains(in Range<Bound> other) => (other.lowerBound.CompareTo(lowerBound) >= 0) && (other.upperBound.CompareTo(upperBound) <= 0);

        /// <summary>
        /// Returns a <see langword="bool"/> value indicating whether this range and the given range contain an element in common.
        /// </summary>
        /// <param name="other">A range to check for elements in common.</param>
        /// <returns><see langword="true"/> if this range and <paramref name="other"/> have at least one element in common; otherwise, <see langword="false"/>.</returns>
        public bool Overlaps(in Range<Bound> other) {
            bool lower = Contains(other.lowerBound) || other.Contains(lowerBound);
            bool upper = Contains(other.upperBound) || other.Contains(upperBound);
            return lower || upper;
        }

        /// <summary>
        /// Returns a copy of this range clamped to the given limiting range.
        /// </summary>
        /// <param name="limits">The range to clamp the bounds of this range.</param>
        /// <returns>A new range clamped to the bounds of <paramref name="limits"/>.</returns>
        public Range<Bound> ClampedTo(in Range<Bound> limits) {
            Bound lower = lowerBound.CompareTo(limits.lowerBound) >= 0 ? lowerBound : limits.lowerBound;
            Bound upper = upperBound.CompareTo(limits.upperBound) <= 0 ? upperBound : limits.upperBound;
            return new Range<Bound>(lower, upper);
        }

        public static Range<int> Create(in System.Range range) => new Range<int>(
            range.Start.Value,
            range.End.Value
        );

        public bool Equals(Range<Bound> other) => lowerBound.Equals(other.lowerBound) && upperBound.Equals(other.upperBound);
        //public static Range<Bound> operator ..<(Bound lowerBound, Bound upperBound) => new Range(lowerBound, upperBound);
    }

    public static partial class Extensions {
        public static System.Range ConvertToSystemRange(this Range<int> range) => new System.Range(new Index(range.lowerBound), new Index(range.upperBound));

        public static IEnumerator GetEnumerator(this Range<int> range) => new IntRangeEnumerator(range);
    }

    internal class IntRangeEnumerator : IEnumerator {
        public Range<int> _range;

        private int position;

        public IntRangeEnumerator(Range<int> range) {
            _range = range;
            position = range.lowerBound - 1;
        }

        public bool MoveNext() {
            position++;
            return (position < _range.upperBound);
        }

        public void Reset() {
            position = _range.lowerBound - 1;
        }

        object IEnumerator.Current => Current;

        public int Current => position;
    }
}