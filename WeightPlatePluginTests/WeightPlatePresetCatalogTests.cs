using System;
using NUnit.Framework;
using WeightPlatePluginCore.Presets;

namespace WeightPlatePlugin.Tests
{
    /// <summary>
    /// Модульные тесты для каталога пресетов блинов,
    /// предоставляющего доступ к предопределённым конфигурациям.
    /// </summary>
    [TestFixture]
    public sealed class WeightPlatePresetCatalogTests
    {
        [Test]
        [Description("Проверяет, что идентификатор пресета по умолчанию установлен корректно.")]
        public void DefaultPresetId_IsStandardTraining()
        {
            Assert.That(
                WeightPlatePresetCatalog.DefaultPresetId,
                Is.EqualTo(WeightPlatePresetId.StandardTraining));
        }

        [Test]
        [Description("Проверяет, что каталог пресетов содержит элементы и включает пользовательский пресет.")]
        public void GetAll_ReturnsNonEmptyList_AndContainsCustom()
        {
            var presets = WeightPlatePresetCatalog.GetAll();

            Assert.That(presets, Is.Not.Null);
            Assert.That(presets.Count, Is.GreaterThanOrEqualTo(1));

            bool hasCustom = false;

            for (int i = 0; i < presets.Count; i++)
            {
                if (presets[i].Id == WeightPlatePresetId.Custom)
                {
                    hasCustom = true;
                    break;
                }
            }

            Assert.That(hasCustom, Is.True);
        }

        [Test]
        [Description("Проверяет корректное получение пресетов по идентификатору.")]
        public void GetById_ReturnsPresetWithSameId_ForKnownIds()
        {
            Assert.That(
                WeightPlatePresetCatalog.GetById(WeightPlatePresetId.StandardTraining).Id,
                Is.EqualTo(WeightPlatePresetId.StandardTraining));

            Assert.That(
                WeightPlatePresetCatalog.GetById(WeightPlatePresetId.Olympic50).Id,
                Is.EqualTo(WeightPlatePresetId.Olympic50));

            Assert.That(
                WeightPlatePresetCatalog.GetById(WeightPlatePresetId.Compact300).Id,
                Is.EqualTo(WeightPlatePresetId.Compact300));

            Assert.That(
                WeightPlatePresetCatalog.GetById(WeightPlatePresetId.Dumbbell200).Id,
                Is.EqualTo(WeightPlatePresetId.Dumbbell200));

            Assert.That(
                WeightPlatePresetCatalog.GetById(WeightPlatePresetId.Custom).Id,
                Is.EqualTo(WeightPlatePresetId.Custom));
        }

        [Test]
        [Description("Проверяет, что при запросе неизвестного идентификатора выбрасывается исключение.")]
        public void GetById_WhenUnknownId_ThrowsArgumentOutOfRangeException()
        {
            const WeightPlatePresetId unknownId = (WeightPlatePresetId)9999;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                WeightPlatePresetCatalog.GetById(unknownId));
        }

        [Test]
        [Description("Проверяет, что все предопределённые пресеты содержат валидные параметры.")]
        public void AllNonCustomPresets_HaveNonNullParameters_AndAreValid()
        {
            var presets = WeightPlatePresetCatalog.GetAll();

            for (int i = 0; i < presets.Count; i++)
            {
                var preset = presets[i];

                if (preset.Id == WeightPlatePresetId.Custom)
                {
                    Assert.That(preset.Parameters, Is.Null);
                    continue;
                }

                Assert.That(preset.Parameters, Is.Not.Null);
                Assert.DoesNotThrow(() => preset.Parameters.ValidateAll());
            }
        }
    }
}
