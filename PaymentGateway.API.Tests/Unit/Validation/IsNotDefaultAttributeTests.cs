using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using PaymentGateway.API.Validation;
using Xunit;

namespace PaymentGateway.API.Tests.Unit.Validation
{
    public class IsNotDefaultAttributeTests
    {
        private readonly IsNotDefaultAttribute _sut;

        public IsNotDefaultAttributeTests()
        {
            _sut = new IsNotDefaultAttribute();

        }

        // cannot use InlineData for primitive types, see here:
        // https://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata/

        public static IEnumerable<object[]> DefaultValues => GetDefaults().Select(o => new[] { o });
        public static IEnumerable<object[]> NonDefaultValues => GetNonDefaults().Select(o => new[] { o });

        private static IEnumerable<object> GetDefaults()
        {
            yield return Guid.Empty;
            yield return 0;
            yield return 0M;
            yield return 0D;
            yield return 0L;
            yield return DummyEnum.Unknown;
        }

        private enum DummyEnum
        {
            Unknown,
            Gbp
        }

        private static IEnumerable<object> GetNonDefaults()
        {
            yield return Guid.NewGuid();
            yield return 1;
            yield return 1M;
            yield return 1D;
            yield return 1L;
            yield return double.Epsilon;
        }


        [Theory]
        [MemberData(nameof(NonDefaultValues))]
        public void Valid_When_Default_Value_Not_Used(object value)
        {
            _sut.Validate(value, new ValidationContext(value));

        }

        [Theory]
        [MemberData(nameof(DefaultValues))]
        public void Invalid_When_Default_Value_Used(object value)
        {
            Assert.Throws<ValidationException>(() => _sut.Validate(value, new ValidationContext(value)));
        }
    }
}
