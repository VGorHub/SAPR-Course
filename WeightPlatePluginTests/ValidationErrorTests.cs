using NUnit.Framework;
using WeightPlatePluginCore.Model;

namespace WeightPlatePlugin.Tests
{
    /// <summary>
    /// Набор тестов для класса <see cref="ValidationError"/>.
    /// Проверяет корректность создания объектов ошибок валидации и работу их свойств.
    /// </summary>
    [TestFixture]
    public class ValidationErrorTests
    {
        [Test]
        [Description("Проверка конструктора ValidationError и его геттеров")]
        public void Constructor_SetsPropertiesAndGetters()
        {
            var error = new ValidationError(ParameterId.ThicknessT, "Некорректная толщина");

            Assert.AreEqual(ParameterId.ThicknessT, error.Parameter);
            Assert.AreEqual("Некорректная толщина", error.Message);

            Assert.AreEqual("Некорректная толщина", error.GetMessage());
            Assert.AreEqual(ParameterId.ThicknessT, error.GetParameter());
        }
    }
}
