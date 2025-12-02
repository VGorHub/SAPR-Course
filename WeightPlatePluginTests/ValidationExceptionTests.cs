using System.Collections.Generic;
using NUnit.Framework;
using WeightPlatePlugin.Model;

namespace WeightPlatePlugin.Tests
{
    //TODO: XML
    [TestFixture]
    public class ValidationExceptionTests
    {
        //TODO: description
        [Test]
        public void Constructor_WithErrorsList_UsesFirstMessageAndExposesErrors()
        {
            var errors = new List<ValidationError>
            {
                new ValidationError(ParameterId.OuterDiameterD, "Первая ошибка"),
                new ValidationError(ParameterId.ThicknessT, "Вторая ошибка")
            };

            var ex = new ValidationException(errors);

            Assert.AreEqual("Первая ошибка", ex.Message);
            Assert.AreEqual(2, ex.Errors.Count);
            Assert.AreSame(errors, ex.Errors);
            Assert.IsFalse(ex.IsValid);

            var fromGetter = ex.GetErrors();
            Assert.AreEqual(2, fromGetter.Count);
        }

        [Test]
        public void Constructor_WithNullErrorsList_CreatesEmptyErrorsCollection()
        {
            List<ValidationError> errors = null;

            var ex = new ValidationException(errors);

            Assert.IsNotNull(ex.Errors);
            Assert.AreEqual(0, ex.Errors.Count);
            Assert.AreEqual(string.Empty, ex.Message);
            Assert.IsTrue(ex.IsValid);
        }

        [Test]
        public void Constructor_WithSingleError_SetsMessageAndSingleError()
        {
            var error = new ValidationError(ParameterId.ChamferRadiusR, "Ошибка фаски");

            var ex = new ValidationException(error);

            Assert.AreEqual("Ошибка фаски", ex.Message);
            Assert.AreEqual(1, ex.Errors.Count);
            Assert.IsFalse(ex.IsValid);
            Assert.AreEqual(error, ex.Errors[0]);
        }

        [Test]
        public void Constructor_WithNullError_CreatesEmptyErrorsCollection()
        {
            ValidationError error = null;

            var ex = new ValidationException(error);

            Assert.AreEqual(0, ex.Errors.Count);
            Assert.AreEqual(string.Empty, ex.Message);
            Assert.IsTrue(ex.IsValid);
        }
    }
}
