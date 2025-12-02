using System;
using NUnit.Framework;
using WeightPlatePlugin.Model;

namespace WeightPlatePlugin.Tests
{
    [TestFixture]
    public class ParametersTests
    {
        private Parameters CreateValidParameters()
        {
            //TODO: RSDN +
            var parameters = new Parameters();
            parameters.SetOuterDiameterD(200);
            parameters.SetThicknessT(20);
            parameters.SetHoleDiameterd(30);
            parameters.SetChamferRadiusR(5);
            parameters.SetRecessRadiusL(100);
            parameters.SetRecessDepthG(5);
            return parameters;
        }

        [Test]
        public void ValidateAll_ValidParameters_DoesNotThrow()
        {
            //TODO: RSDN +
            var parameters = CreateValidParameters();

            Assert.DoesNotThrow(() => parameters.ValidateAll());

            Assert.AreEqual(200, parameters.OuterDiameterD);
            Assert.AreEqual(20, parameters.ThicknessT);
            Assert.AreEqual(30, parameters.HoleDiameterd);
            Assert.AreEqual(5, parameters.ChamferRadiusR);
            Assert.AreEqual(100, parameters.RecessRadiusL);
            Assert.AreEqual(5, parameters.RecessDepthG);
        }

        [Test]
        public void ValidateAll_OuterDiameter_OutOfRange_AddsError()
        {
            var parameters = CreateValidParameters();
            parameters.SetOuterDiameterD(-1);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());
            Assert.IsNotNull(exception);
            Assert.IsFalse(exception.IsValid);

            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("Наружный диаметр D должен быть в диапазоне 100–500 мм")));
        }

        [Test]
        public void ValidateAll_Thickness_OutOfRange_AddsError()
        {
            //TODO: RSDN +
            var parameters = CreateValidParameters();
            parameters.SetThicknessT(5);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ThicknessT &&
                     e.Message.Contains("Толщина T должна быть в диапазоне 10–80 мм")));
        }

        [Test]
        public void ValidateAll_HoleDiameter_OutOfRange_AddsError()
        {
            var parameters = CreateValidParameters();
            parameters.SetHoleDiameterd(10);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.HoleDiameterd &&
                     e.Message.Contains("Диаметр отверстия d должен быть в диапазоне 26–51 мм")));
        }

        [Test]
        public void ValidateAll_ChamferRadius_OutOfRange_AddsError()
        {
            //TODO: RSDN +
            var parameters = CreateValidParameters();
            parameters.SetChamferRadiusR(1);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ChamferRadiusR &&
                     e.Message.Contains("Радиус скругления фаски R должен быть в диапазоне 2–10 мм")));
        }

        [Test]
        public void ValidateAll_RecessRadius_NonPositive_AddsError()
        {
            //TODO: RSDN +
            var parameters = CreateValidParameters();
            parameters.SetRecessRadiusL(0);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessRadiusL &&
                     e.Message.Contains("Радиус внутреннего углубления L должен быть больше 0")));
        }

        [Test]
        public void ValidateAll_RecessDepth_NonPositive_AddsError()
        {
            var parameters = CreateValidParameters();
            parameters.SetRecessDepthG(0);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessDepthG &&
                     e.Message.Contains("Глубина внутреннего углубления G должна быть больше 0")));
        }

        [Test]
        public void ValidateAll_ThicknessGreaterThanDOver10_AddsErrorsForTAndD()
        {
            var parameters = CreateValidParameters();
            parameters.SetThicknessT(30);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());

            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ThicknessT &&
                     e.Message.Contains("T должна удовлетворять условию T ≤ D/10")));
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("T должна удовлетворять условию T ≤ D/10")));
        }

        [Test]
        public void ValidateAll_HoleDiameterNotLessThanOuterDiameter_AddsErrorsForDAndd()
        {
            var parameters = CreateValidParameters();

            parameters.SetHoleDiameterd(220);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());

            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.HoleDiameterd &&
                     e.Message.Contains("d должен быть меньше наружного диаметра D")));
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("d должен быть меньше наружного диаметра D")));
        }

        [Test]
        public void ValidateAll_RecessRadiusDoesNotSatisfy_dLessLlessD_AddsErrors()
        {
            var parameters = CreateValidParameters();

            parameters.SetHoleDiameterd(30);
            parameters.SetRecessRadiusL(20);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());

            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessRadiusL &&
                     e.Message.Contains("L должен удовлетворять неравенству d < L < D")));
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.HoleDiameterd &&
                     e.Message.Contains("L должен удовлетворять неравенству d < L < D")));
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("L должен удовлетворять неравенству d < L < D")));
        }

        [Test]
        public void ValidateAll_RecessDepthGreaterOrEqualThickness_AddsErrors()
        {
            var parameters = CreateValidParameters();
            parameters.SetThicknessT(20);
            parameters.SetRecessDepthG(25);

            var exception = Assert.Throws<ValidationException>(() => parameters.ValidateAll());

            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessDepthG &&
                     e.Message.Contains("G должна удовлетворять неравенству 0 < G < T")));
            Assert.That(exception.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ThicknessT &&
                     e.Message.Contains("G должна удовлетворять неравенству 0 < G < T")));
        }
    }
}
