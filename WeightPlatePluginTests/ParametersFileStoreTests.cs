using System;
using System.IO;
using NUnit.Framework;
using WeightPlatePluginCore.Model;
using WeightPlatePluginCore.Persistence;

namespace WeightPlatePluginTests
{
    /// <summary>
    /// Набор модульных тестов для файлового хранилища пользовательских параметров.
    /// Проверяется загрузка/сохранение, обработка ошибок и создание файла при отсутствии.
    /// </summary>
    [TestFixture]
    public sealed class ParametersFileStoreTests
    {
        //TODO: XML
        private string _tempDirectory;
        private string _filePath;

        [SetUp]
        public void SetUp()
        {
            //TODO: refactor
            _tempDirectory = Path.Combine(Path.GetTempPath(), "WeightPlatePluginTests", Guid.NewGuid().ToString("N"));
            _filePath = Path.Combine(_tempDirectory, "user-parameters.json");
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, recursive: true);
                }
            }
            catch
            {
                //TODO: ??
                // Игнорируем: тесты не должны падать из-за проблем с очисткой временных файлов
            }
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет, что конструктор ParametersFileStore выбрасывает исключение при пустом пути к файлу.")]
        public void Ctor_WhenFilePathIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ParametersFileStore("  "));
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет, что TryLoad возвращает false и null-параметры, если файл отсутствует.")]
        public void TryLoad_WhenFileDoesNotExist_ReturnsFalse()
        {
            var store = new ParametersFileStore(_filePath);

            var loaded = store.TryLoad(out var parameters);

            Assert.That(loaded, Is.False);
            Assert.That(parameters, Is.Null);
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет, что Save создаёт директорию и файл, если они отсутствуют.")]
        public void Save_WhenDirectoryDoesNotExist_CreatesDirectoryAndFile()
        {
            var store = new ParametersFileStore(_filePath);

            var p = CreateValidParameters();

            Assert.That(Directory.Exists(_tempDirectory), Is.False);
            Assert.That(File.Exists(_filePath), Is.False);

            store.Save(p);

            Assert.That(Directory.Exists(_tempDirectory), Is.True);
            Assert.That(File.Exists(_filePath), Is.True);
            Assert.That(new FileInfo(_filePath).Length, Is.GreaterThan(0));
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет, что Save выбрасывает исключение при передаче null параметров.")]
        public void Save_WhenParametersIsNull_ThrowsArgumentNullException()
        {
            var store = new ParametersFileStore(_filePath);

            Assert.Throws<ArgumentNullException>(() => store.Save(null));
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет, что Save не сохраняет невалидные параметры и выбрасывает исключение валидации.")]
        public void Save_WhenParametersAreInvalid_ThrowsValidationException()
        {
            var store = new ParametersFileStore(_filePath);

            var invalid = CreateInvalidParameters();

            Assert.Throws<ValidationException>(() => store.Save(invalid));
            Assert.That(File.Exists(_filePath), Is.False);
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет, что TryLoad возвращает false при повреждённом файле (некорректный JSON).")]
        public void TryLoad_WhenFileIsCorrupted_ReturnsFalse()
        {
            Directory.CreateDirectory(_tempDirectory);
            File.WriteAllText(_filePath, "{ this is not valid json }");

            var store = new ParametersFileStore(_filePath);

            var loaded = store.TryLoad(out var parameters);

            Assert.That(loaded, Is.False);
            Assert.That(parameters, Is.Null);
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет, что после Save можно выполнить TryLoad и получить эквивалентные значения параметров.")]
        public void Save_ThenTryLoad_RoundTripRestoresValues()
        {
            var store = new ParametersFileStore(_filePath);

            var original = CreateValidParameters();

            store.Save(original);

            var loadedOk = store.TryLoad(out var loaded);

            Assert.That(loadedOk, Is.True);
            Assert.That(loaded, Is.Not.Null);

            Assert.That(loaded.OuterDiameterD, Is.EqualTo(original.OuterDiameterD));
            Assert.That(loaded.ThicknessT, Is.EqualTo(original.ThicknessT));
            Assert.That(loaded.HoleDiameterd, Is.EqualTo(original.HoleDiameterd));
            Assert.That(loaded.ChamferRadiusR, Is.EqualTo(original.ChamferRadiusR));
            Assert.That(loaded.RecessRadiusL, Is.EqualTo(original.RecessRadiusL));
            Assert.That(loaded.RecessDepthG, Is.EqualTo(original.RecessDepthG));
        }

        [Test]
        //TODO: RSDN
        [Description("Проверяет ветку: если целевой файл уже существует, Save удаляет его перед перемещением temp-файла.")]
        public void Save_WhenTargetFileAlreadyExists_DeletesOldFileAndOverwrites()
        {
            var store = new ParametersFileStore(_filePath);

            Directory.CreateDirectory(_tempDirectory);

            File.WriteAllText(_filePath, "OLD_CONTENT");
            var oldInfo = new FileInfo(_filePath);
            Assert.That(oldInfo.Length, Is.GreaterThan(0));

            var p = CreateValidParameters();

            store.Save(p);

            Assert.That(File.Exists(_filePath), Is.True);
            var newText = File.ReadAllText(_filePath);
            Assert.That(newText, Does.Not.Contain("OLD_CONTENT"));
            Assert.That(newText, Does.Contain("OuterDiameterD"));
        }


        [Test]
        //TODO: RSDN
        [Description("Проверяет ветку: TryLoad возвращает false, если JSON валиден, но десериализация даёт null (json = 'null').")]
        public void TryLoad_WhenJsonIsNullLiteral_ReturnsFalse()
        {
            Directory.CreateDirectory(_tempDirectory);
            File.WriteAllText(_filePath, "null");

            var store = new ParametersFileStore(_filePath);

            var loaded = store.TryLoad(out var parameters);

            Assert.That(loaded, Is.False);
            Assert.That(parameters, Is.Null);
        }

        //TODO: XML
        private static Parameters CreateValidParameters()
        {
            //TODO: RSDN
            var p = new Parameters();

            p.SetOuterDiameterD(450);
            p.SetThicknessT(45);
            p.SetHoleDiameterd(28);
            p.SetChamferRadiusR(5);
            p.SetRecessRadiusL(120);
            p.SetRecessDepthG(15);

            p.ValidateAll();

            return p;
        }

        //TODO: XML
        private static Parameters CreateInvalidParameters()
        {
            //TODO: RSDN
            var p = new Parameters();

            p.SetOuterDiameterD(100);
            p.SetThicknessT(50);

            p.SetHoleDiameterd(28);
            p.SetChamferRadiusR(5);
            p.SetRecessRadiusL(40);
            p.SetRecessDepthG(5);

            return p;
        }
    }
}

