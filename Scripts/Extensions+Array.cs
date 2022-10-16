using System;
using System.Collections.Generic;

namespace Foundation {
    public static partial class Extensions {
        public static bool Any<Element>(this Element[] collection, Func<Element, bool> body) {
            for (int i = 0; i < collection.Length; i++) {
                if (body(collection[i])) {
                    return true;
                }
            }
            return false;
        }

        public static bool All<Element>(this Element[] collection, Func<Element, bool> body) {
            for (int i = 0; i < collection.Length; i++) {
                if (!body(collection[i])) {
                    return false;
                }
            }
            return true;
        }

        public static Result[] Map<Element, Result>(this Element[] collection, Func<Element, Result> body) {
            Result[] result = new Result[collection.Length];
            for (int i = 0; i < collection.Length; i++) {
                result[i] = body(collection[i]);
            }
            return result;
        }

        public static Element[] Filter<Element>(this Element[] collection, Func<Element, bool> body) {
            List<Element> result = new List<Element>(collection.Length);
            for (int i = 0; i < collection.Length; i++) {
                if (body(collection[i])) {
                    result.Add(collection[i]);
                }
            }
            return result.ToArray();
        }
    }
}