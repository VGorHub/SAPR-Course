using NUnit.Framework;
using WeightPlatePlugin.Model;

namespace WeightPlatePlugin.Tests
{
    //TODO: XML
    [TestFixture]
    public class ValidationErrorTests
    {
        //TODO: description
        [Test]
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
