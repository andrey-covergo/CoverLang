using System;
using FluentAssertions.Specialized;

namespace CoverLang.Test
{
    public static class ExceptionAssertionExtensions
    {
        public static ExceptionAssertions<T> WithMessageContaining<T>(this ExceptionAssertions<T> assertion,
            string messagePart) where T : Exception
        {
            return assertion.WithMessage("*" + messagePart + "*");
        }
    }
}