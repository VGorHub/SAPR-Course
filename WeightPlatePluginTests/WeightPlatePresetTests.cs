using System;
using NUnit.Framework;
using WeightPlatePluginCore.Model;
using WeightPlatePluginCore.Presets;

namespace WeightPlatePlugin.Tests
{
    /// <summary>
    /// Модульные тесты для класса WeightPlatePreset,
    /// описывающего пресет параметров блина.
    /// </summary>
    [TestFixture]
    public sealed class WeightPlatePresetTests
    {
        [Test]
        [Description("Проверяет, что конструктор пресета выбрасывает исключение при отсутствии имени.")]
        public void Ctor_WhenDisplayNameIsNull_ThrowsArgumentException()
        {
            var p = new Parameters();

            Assert.Throws<ArgumentException>(() =>
                new WeightPlatePreset(WeightPlatePresetId.StandardTraining, null, p));
        }

        [Test]
        [Description("Проверяет, что конструктор пресета не принимает пустое или пробельное имя.")]
        public void Ctor_WhenDisplayNameIsWhitespace_ThrowsArgumentException()
        {
            var p = new Parameters();

            Assert.Throws<ArgumentException>(() =>
                new WeightPlatePreset(WeightPlatePresetId.StandardTraining, "   ", p));
        }

        [Test]
        [Description("Проверяет корректную инициализацию свойств пресета при создании.")]
        public void Ctor_SetsProperties()
        {
            var p = new Parameters();
            var preset = new WeightPlatePreset(
                WeightPlatePresetId.StandardTraining, 
                "Стандартный", 
                p);

            Assert.That(preset.Id, Is.EqualTo(WeightPlatePresetId.StandardTraining));
            Assert.That(preset.DisplayName, Is.EqualTo("Стандартный"));
            Assert.That(preset.Parameters, Is.SameAs(p));
        }

        [Test]
        [Description("Проверяет, что строковое представление пресета соответствует его отображаемому имени.")]
        public void ToString_ReturnsDisplayName()
        {
            var preset = new WeightPlatePreset(
                WeightPlatePresetId.StandardTraining,
                "Стандартный",
                new Parameters());

            Assert.That(preset.ToString(), Is.EqualTo("Стандартный"));
        }

        [Test]
        [Description("Проверяет определение пользовательского пресета по идентификатору.")]
        public void IsCustom_WhenIdIsCustom_ReturnsTrue()
        {
            var preset = new WeightPlatePreset(
                WeightPlatePresetId.Custom,
                "Пользовательский",
                parameters: null);

            Assert.That(preset.IsCustom, Is.True);
        }

        [Test]
        [Description("Проверяет определение пользовательского пресета по идентификатору.")]
        public void IsCustom_WhenIdIsNotCustom_ReturnsFalse()
        {
            var preset = new WeightPlatePreset(
                WeightPlatePresetId.StandardTraining,
                "Стандартный",
                new Parameters());

            Assert.That(preset.IsCustom, Is.False);
        }
    }
}
