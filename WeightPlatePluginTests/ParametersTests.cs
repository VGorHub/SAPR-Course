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
            var p = new Parameters();
            p.SetOuterDiameterD(200);
            p.SetThicknessT(20);
            p.SetHoleDiameterd(30);
            p.SetChamferRadiusR(5);
            p.SetRecessRadiusL(100);
            p.SetRecessDepthG(5);
            return p;
        }

        [Test]
        public void ValidateAll_ValidParameters_DoesNotThrow()
        {
            var p = CreateValidParameters();

            Assert.DoesNotThrow(() => p.ValidateAll());

            Assert.AreEqual(200, p.OuterDiameterD);
            Assert.AreEqual(20, p.ThicknessT);
            Assert.AreEqual(30, p.HoleDiameterd);
            Assert.AreEqual(5, p.ChamferRadiusR);
            Assert.AreEqual(100, p.RecessRadiusL);
            Assert.AreEqual(5, p.RecessDepthG);
        }

        [Test]
        public void ValidateAll_OuterDiameter_OutOfRange_AddsError()
        {
            var p = CreateValidParameters();
            p.SetOuterDiameterD(-1);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());
            Assert.IsNotNull(ex);
            Assert.IsFalse(ex.IsValid);

            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("Наружный диаметр D должен быть в диапазоне 100–500 мм")));
        }

        [Test]
        public void ValidateAll_Thickness_OutOfRange_AddsError()
        {
            var p = CreateValidParameters();
            p.SetThicknessT(5);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ThicknessT &&
                     e.Message.Contains("Толщина T должна быть в диапазоне 10–80 мм")));
        }

        [Test]
        public void ValidateAll_HoleDiameter_OutOfRange_AddsError()
        {
            var p = CreateValidParameters();
            p.SetHoleDiameterd(10);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.HoleDiameterd &&
                     e.Message.Contains("Диаметр отверстия d должен быть в диапазоне 26–51 мм")));
        }

        [Test]
        public void ValidateAll_ChamferRadius_OutOfRange_AddsError()
        {
            var p = CreateValidParameters();
            p.SetChamferRadiusR(1);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ChamferRadiusR &&
                     e.Message.Contains("Радиус скругления фаски R должен быть в диапазоне 2–10 мм")));
        }

        [Test]
        public void ValidateAll_RecessRadius_NonPositive_AddsError()
        {
            var p = CreateValidParameters();
            p.SetRecessRadiusL(0);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessRadiusL &&
                     e.Message.Contains("Радиус внутреннего углубления L должен быть больше 0")));
        }

        [Test]
        public void ValidateAll_RecessDepth_NonPositive_AddsError()
        {
            var p = CreateValidParameters();
            p.SetRecessDepthG(0);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessDepthG &&
                     e.Message.Contains("Глубина внутреннего углубления G должна быть больше 0")));
        }

        [Test]
        public void ValidateAll_ThicknessGreaterThanDOver10_AddsErrorsForTAndD()
        {
            var p = CreateValidParameters();
            p.SetThicknessT(30);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());

            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ThicknessT &&
                     e.Message.Contains("T должна удовлетворять условию T ≤ D/10")));
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("T должна удовлетворять условию T ≤ D/10")));
        }

        [Test]
        public void ValidateAll_HoleDiameterNotLessThanOuterDiameter_AddsErrorsForDAndd()
        {
            var p = CreateValidParameters();

            p.SetHoleDiameterd(220);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());

            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.HoleDiameterd &&
                     e.Message.Contains("d должен быть меньше наружного диаметра D")));
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("d должен быть меньше наружного диаметра D")));
        }

        [Test]
        public void ValidateAll_RecessRadiusDoesNotSatisfy_dLessLlessD_AddsErrors()
        {
            var p = CreateValidParameters();

            p.SetHoleDiameterd(30);
            p.SetRecessRadiusL(20);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());

            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessRadiusL &&
                     e.Message.Contains("L должен удовлетворять неравенству d < L < D")));
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.HoleDiameterd &&
                     e.Message.Contains("L должен удовлетворять неравенству d < L < D")));
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.OuterDiameterD &&
                     e.Message.Contains("L должен удовлетворять неравенству d < L < D")));
        }

        [Test]
        public void ValidateAll_RecessDepthGreaterOrEqualThickness_AddsErrors()
        {
            var p = CreateValidParameters();
            p.SetThicknessT(20);
            p.SetRecessDepthG(25);

            var ex = Assert.Throws<ValidationException>(() => p.ValidateAll());

            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.RecessDepthG &&
                     e.Message.Contains("G должна удовлетворять неравенству 0 < G < T")));
            Assert.That(ex.Errors, Has.Some.Matches<ValidationError>(
                e => e.Parameter == ParameterId.ThicknessT &&
                     e.Message.Contains("G должна удовлетворять неравенству 0 < G < T")));
        }
    }
}
