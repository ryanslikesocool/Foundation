using System;

namespace Foundation {
    public static class Guard {
        internal class GuardClauseException : Exception {
            public GuardClauseException() { }
            public GuardClauseException(string message) : base(message) { }
            public GuardClauseException(string message, Exception inner) : base(message, inner) { }
        }

        public static void NotNull<Value>(in Value value, in string name) {
            if (!value.Equals((Value)default)) { return; }

            throw new GuardClauseException("Was expecting a non-null object.", new ArgumentNullException(name ?? nameof(value)));
        }

        public static void Require(bool condition, in string message) {
            if (condition) { return; }

            throw new GuardClauseException(message);
        }

        public static void Require(Func<bool> predicate, in string message) {
            if (predicate()) { return; }

            throw new GuardClauseException(message);
        }

        public static void NotImplemented() {
            throw new NotImplementedException();
        }
    }
}