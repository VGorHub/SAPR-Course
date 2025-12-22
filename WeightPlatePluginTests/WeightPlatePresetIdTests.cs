using System;
using NUnit.Framework;
using WeightPlatePluginCore.Presets;

namespace WeightPlatePlugin.Tests
{
    /// <summary>
    /// Модульные тесты для перечисления идентификаторов пресетов блинов.
    /// </summary>
    [TestFixture]
    public sealed class WeightPlatePresetIdTests
    {
        [Test]
        [Description("Проверяет соответствие числовых значений элементов перечисления WeightPlatePresetId.")]
        public void PresetIds_HaveExpectedNumericValues()
        {
            Assert.That((int)WeightPlatePresetId.StandardTraining, Is.EqualTo(1));
            Assert.That((int)WeightPlatePresetId.Olympic, Is.EqualTo(2));
            Assert.That((int)WeightPlatePresetId.Compact, Is.EqualTo(3));
            Assert.That((int)WeightPlatePresetId.Dumbbell, Is.EqualTo(4));
            Assert.That((int)WeightPlatePresetId.Custom, Is.EqualTo(1000));
        }

        [Test]
        [Description("Проверяет, что перечисление содержит все ожидаемые идентификаторы пресетов.")]
        public void Enum_IsDefined_ForKnownValues()
        {
            Assert.That(Enum.IsDefined(typeof(WeightPlatePresetId), 
                WeightPlatePresetId.StandardTraining), Is.True);
            Assert.That(Enum.IsDefined(typeof(WeightPlatePresetId), 
                WeightPlatePresetId.Custom), Is.True);
        }
    }
}
