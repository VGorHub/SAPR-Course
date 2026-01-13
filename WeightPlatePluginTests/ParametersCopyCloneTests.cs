using System;
using NUnit.Framework;
using WeightPlatePluginCore.Model;

namespace WeightPlatePlugin.Tests
{
    /// <summary>
    /// Набор модульных тестов для проверки корректности
    /// копирования и клонирования параметров блина.
    /// </summary>
    [TestFixture]
    public sealed class ParametersCopyCloneTests
    {
        [Test]
        //TODO: RSDN +
        [Description("Проверяет, что метод CopyFrom " +
            "выбрасывает исключение при передаче null.")]
        public void CopyFrom_WhenSourceIsNull_ThrowsArgumentNullException()
        {
            var target = new Parameters();

            Assert.Throws<ArgumentNullException>(() => target.CopyFrom(null));
        }

        [Test]
        //TODO: RSDN +
        [Description("Проверяет, что метод CopyFrom " +
            "корректно копирует все значения параметров.")]
        public void CopyFrom_CopiesAllValues()
        {
            var source = new Parameters();
            source.SetOuterDiameterD(450);
            source.SetThicknessT(45);
            source.SetHoleDiameterd(28);
            source.SetChamferRadiusR(5);
            source.SetRecessRadiusL(120);
            source.SetRecessDepthG(15);

            var target = new Parameters();
            target.SetOuterDiameterD(200);
            target.SetThicknessT(20);
            target.SetHoleDiameterd(26);
            target.SetChamferRadiusR(2);
            target.SetRecessRadiusL(60);
            target.SetRecessDepthG(8);

            target.CopyFrom(source);

            Assert.That(target.OuterDiameterD, Is.EqualTo(source.OuterDiameterD));
            Assert.That(target.ThicknessT, Is.EqualTo(source.ThicknessT));
            Assert.That(target.HoleDiameterd, Is.EqualTo(source.HoleDiameterd));
            Assert.That(target.ChamferRadiusR, Is.EqualTo(source.ChamferRadiusR));
            Assert.That(target.RecessRadiusL, Is.EqualTo(source.RecessRadiusL));
            Assert.That(target.RecessDepthG, Is.EqualTo(source.RecessDepthG));
        }

        [Test]
        //TODO: RSDN +
        [Description("Проверяет, что метод Clone возвращает новый экземпляр" +
            " с идентичными значениями параметров.")]
        public void Clone_ReturnsNewInstanceWithSameValues()
        {
            var source = new Parameters();
            source.SetOuterDiameterD(450);
            source.SetThicknessT(45);
            source.SetHoleDiameterd(28);
            source.SetChamferRadiusR(5);
            source.SetRecessRadiusL(120);
            source.SetRecessDepthG(15);

            var clone = source.Clone();

            Assert.That(clone, Is.Not.SameAs(source));
            Assert.That(clone.OuterDiameterD, Is.EqualTo(source.OuterDiameterD));
            Assert.That(clone.ThicknessT, Is.EqualTo(source.ThicknessT));
            Assert.That(clone.HoleDiameterd, Is.EqualTo(source.HoleDiameterd));
            Assert.That(clone.ChamferRadiusR, Is.EqualTo(source.ChamferRadiusR));
            Assert.That(clone.RecessRadiusL, Is.EqualTo(source.RecessRadiusL));
            Assert.That(clone.RecessDepthG, Is.EqualTo(source.RecessDepthG));
        }

        [Test]
        //TODO: RSDN +
        [Description("Проверяет, что изменения клона не влияют " +
            "на исходный объект параметров.")]
        public void Clone_IsIndependentFromSource()
        {
            var source = new Parameters();
            source.SetOuterDiameterD(450);
            source.SetThicknessT(45);
            source.SetHoleDiameterd(28);
            source.SetChamferRadiusR(5);
            source.SetRecessRadiusL(120);
            source.SetRecessDepthG(15);

            var clone = source.Clone();

            clone.SetThicknessT(10);
            clone.SetRecessDepthG(1);

            Assert.That(source.ThicknessT, Is.EqualTo(45));
            Assert.That(source.RecessDepthG, Is.EqualTo(15));
        }
    }
}
