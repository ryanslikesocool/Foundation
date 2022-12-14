using System;
using System.Collections.Generic;

namespace Foundation {
    public static partial class Extensions {
        /// <summary>
        /// Returns an array containing the results of mapping the given closure over the sequence’s elements.
        /// </summary>
        /// <param name="transform">A mapping closure. <paramref name="transform"/> accepts an element of this sequence as its parameter and returns a transformed value of the same or of a different type.</param>
        /// <returns>An array containing the transformed elements of this sequence.</returns>
        public static Result[] Map<Element, Result>(this Element[] collection, Func<Element, Result> transform) {
            Result[] result = new Result[collection.Length];
            for (int i = 0; i < collection.Length; i++) {
                result[i] = transform(collection[i]);
            }
            return result;
        }

        /// <summary>
        /// Returns an array containing the concatenated results of calling the given transformation with each element of this sequence.
        /// </summary>
        /// <param name="transform">A closure that accepts an element of this sequence as its argument and returns a sequence or collection.</param>
        /// <returns>The resulting flattened array.</returns>
        public static Result[] FlatMap<Element, Result>(this Element[] collection, Func<Element, Result[]> transform) {
            int count = 0;
            Result[][] intermediate = new Result[collection.Length][];
            for (int i = 0; i < collection.Length; i++) {
                intermediate[i] = transform(collection[i]);
                count += intermediate[i].Length;
            }

            Result[] result = new Result[count];
            int accumulated = 0;
            for (int i = 0; i < intermediate.Length; i++) {
                for (int j = 0; j < intermediate[i].Length; j++) {
                    result[accumulated + j] = intermediate[i][j];
                }
                accumulated += intermediate[i].Length;
            }
            return result;
        }

        public static Element[] FlatMap<Element>(this Element[][] collection) {
            int count = 0;
            for (int i = 0; i < collection.Length; i++) {
                count += collection[i].Length;
            }
            Element[] result = new Element[count];
            int accumulated = 0;
            for (int i = 0; i < collection.Length; i++) {
                for (int j = 0; j < collection[i].Length; j++) {
                    result[accumulated + j] = collection[i][j];
                }
                accumulated += collection[i].Length;
            }
            return result;
        }

        /// <summary>
        /// Returns an array containing the non-nil results of calling the given transformation with each element of this sequence.
        /// </summary>
        /// <param name="transform">A closure that accepts an element of this sequence as its argument and returns an optional value.</param>
        /// <returns>An array of the non-nil results of calling transform with each element of the sequence.</returns>
        public static Result[] CompactMap<Element, Result>(this Element[] collection, Func<Element, Result> transform) where Result : class {
            int count = 0;
            Result[] result = new Result[collection.Length];
            for (int i = 0; i < collection.Length; i++) {
                Result newElement = transform(collection[i]);

                if (newElement != null) {
                    result[count] = newElement;
                    count++;
                }
            }
            Array.Resize(ref result, count);
            return result;
        }

        public static Element[] CompactMap<Element>(this Element[] collection) where Element : class {
            int count = 0;
            Element[] result = new Element[collection.Length];
            for (int i = 0; i < collection.Length; i++) {
                if (collection[i] != null) {
                    result[count] = collection[i];
                    count++;
                }
            }
            Array.Resize(ref result, count);
            return result;
        }

        /// <summary>
        /// Returns an array containing, in order, the elements of the sequence that satisfy the given predicate.
        /// </summary>
        /// <param name="isIncluded">A closure that takes an element of the sequence as its argument and returns a Boolean value indicating whether the element should be included in the returned array.</param>
        /// <returns>An array of the elements that isIncluded allowed.</returns>
        public static Element[] Filter<Element>(this Element[] collection, Func<Element, bool> isIncluded) {
            int count = 0;
            Element[] result = new Element[collection.Length];
            for (int i = 0; i < collection.Length; i++) {
                if (isIncluded(collection[i])) {
                    result[count] = collection[i];
                    count++;
                }
            }
            Array.Resize(ref result, count);
            return result;
        }

        public static Element[] Fill<Element>(this Element[] collection, Element value) {
            for (int i = 0; i < collection.Length; i++) {
                collection[i] = value;
            }
            return collection;
        }

        /*
        /// <summary>
        /// Adds the elements of a sequence to the end of the array.
        /// </summary>
        /// <param name="newElements">The elements to append to the array.</param>
        public static Element[] Append<Element>(this Element[] collection, params Element[] newElements)
            => Append(collection, newElements);
        */

        /// <summary>
        /// Adds the elements of a sequence to the end of the array.
        /// </summary>
        /// <param name="newElements">The elements to append to the array.</param>
        public static Element[] Append<Element>(this Element[] collection, Element[] newElements) {
            int lowerBound = collection.Length;
            int upperBound = collection.Length + newElements.Length;

            Element[] result = new Element[upperBound];

            for (int i = 0; i < lowerBound; i++) {
                result[i] = collection[i];
            }
            for (int i = lowerBound; i < upperBound; i++) {
                result[i] = newElements[i - lowerBound];
            }

            return result;
        }

        /// <summary>
        /// Calls the given closure on each element in the sequence in the same order as a for-in loop.
        /// </summary>
        /// <remarks>
        /// Using the forEach method is distinct from a for-in loop in two important ways:
        /// <br/>1. You cannot use a break or continue statement to exit the current call of the body closure or skip subsequent calls.
        /// <br/>2. Using the return statement in the body closure will exit only from the current call to body, not from any outer scope, and won’t skip subsequent calls.
        /// </remarks>
        /// <param name="body">A closure that takes an element of the sequence as a parameter.</param>
        public static void ForEach<Element>(this Element[] collection, Action<Element> body) {
            for (int i = 0; i < collection.Length; i++) {
                body(collection[i]);
            }
        }

        /// <summary>
        /// Returns an array containing the elements of this sequence in reverse order.
        /// </summary>
        /// <returns>An array containing the elements of this sequence in reverse order.</returns>
        public static Element[] Reversed<Element>(this Element[] collection) {
            Array.Reverse(collection);
            return collection;
        }
    }
}