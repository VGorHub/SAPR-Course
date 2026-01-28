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
        ///
        /// <summary>
        /// Создаёт уникальную временную директорию для выполнения тестов.
        /// </summary>
        /// <returns>
        /// Полный путь к созданной временной директории.
        /// </returns>
        private static string CreateTempDirectory()
        {
            const string testRootDirectoryName = "WeightPlatePluginTests";

            return Path.Combine(
                Path.GetTempPath(),
                testRootDirectoryName,
                Guid.NewGuid().ToString("N"));
        }


        /// <summary>
        /// Формирует путь к файлу параметров внутри указанной директории.
        /// </summary>
        /// <param name="directory">
        /// Путь к директории, в которой должен располагаться файл параметров.
        /// </param>
        /// <returns>
        /// Полный путь к файлу параметров.
        /// </returns>
        private static string CreateParametersFilePath(string directory)
        {
            const string parametersFileName = "user-parameters.json";

            return Path.Combine(directory, parametersFileName);
        }

        [Test]
        [Description("Проверяет, что конструктор ParametersFileStore " +
            "выбрасывает исключение при пустом пути к файлу.")]
        public void Ctor_WhenFilePathIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ParametersFileStore("  "));
        }

        [Test]
        [Description("Проверяет, что TryLoad возвращает false " +
            "и null-параметры, если файл отсутствует.")]
        public void TryLoad_WhenFileDoesNotExist_ReturnsFalse()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                var store = new ParametersFileStore(parametersFilePath);

                var loaded = store.TryLoad(out var parameters);

                Assert.That(loaded, Is.False);
                Assert.That(parameters, Is.Null);
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
            
        }

        [Test]
        [Description("Проверяет, что Save создаёт директорию и файл, " +
            "если они отсутствуют.")]
        public void Save_WhenDirectoryDoesNotExist_CreatesDirectoryAndFile()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                var store = new ParametersFileStore(parametersFilePath);

                var p = CreateValidParameters();

                Assert.That(Directory.Exists(temporaryDirectoryPath), Is.False);
                Assert.That(File.Exists(parametersFilePath), Is.False);

                store.Save(p);

                Assert.That(Directory.Exists(temporaryDirectoryPath), Is.True);
                Assert.That(File.Exists(parametersFilePath), Is.True);
                Assert.That(new FileInfo(parametersFilePath).Length, Is.GreaterThan(0));
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
        }

        [Test]
        [Description("Проверяет, что Save выбрасывает исключение при передаче "
            + "null-параметров.")]
        public void Save_WhenParametersIsNull_ThrowsArgumentNullException()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                var store = new ParametersFileStore(parametersFilePath);

                Assert.Throws<ArgumentNullException>(() => store.Save(null));
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
        }

        [Test]
        [Description("Проверяет, что Save не сохраняет невалидные параметры " +
            "и выбрасывает исключение валидации.")]
        public void Save_WhenParametersAreInvalid_ThrowsValidationException()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                var store = new ParametersFileStore(parametersFilePath);

                var invalid = CreateInvalidParameters();

                Assert.Throws<ValidationException>(() => store.Save(invalid));
                Assert.That(File.Exists(parametersFilePath), Is.False);
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
        }

        [Test]
        [Description("Проверяет, что TryLoad возвращает false " +
            "при повреждённом файле (некорректный JSON).")]
        public void TryLoad_WhenFileIsCorrupted_ReturnsFalse()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                Directory.CreateDirectory(temporaryDirectoryPath);
                File.WriteAllText(parametersFilePath, "{ this is not valid json }");

                var store = new ParametersFileStore(parametersFilePath);

                var loaded = store.TryLoad(out var parameters);

                Assert.That(loaded, Is.False);
                Assert.That(parameters, Is.Null);
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
        }

        [Test]
        [Description("Проверяет, что после Save можно выполнить TryLoad " +
            "и получить эквивалентные значения параметров.")]
        public void Save_ThenTryLoad_RoundTripRestoresValues()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                var store = new ParametersFileStore(parametersFilePath);

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
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
        }

        [Test]
        [Description("Проверяет ветку: если целевой файл уже существует, " +
            "Save удаляет его перед перемещением temp-файла.")]
        public void Save_WhenTargetFileAlreadyExists_DeletesOldFileAndOverwrites()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                var store = new ParametersFileStore(parametersFilePath);

                Directory.CreateDirectory(temporaryDirectoryPath);

                File.WriteAllText(parametersFilePath, "OLD_CONTENT");
                var oldInfo = new FileInfo(parametersFilePath);
                Assert.That(oldInfo.Length, Is.GreaterThan(0));
                
                var parameter = CreateValidParameters();

                store.Save(parameter);

                Assert.That(File.Exists(parametersFilePath), Is.True);
                var newText = File.ReadAllText(parametersFilePath);
                Assert.That(newText, Does.Not.Contain("OLD_CONTENT"));
                Assert.That(newText, Does.Contain("OuterDiameterD"));
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
            
        }


        [Test]
        [Description("Проверяет ветку: TryLoad возвращает false, если JSON валиден, " +
            "но десериализация даёт null (json = 'null').")]
        public void TryLoad_WhenJsonIsNullLiteral_ReturnsFalse()
        {
            var temporaryDirectoryPath = CreateTempDirectory();
            var parametersFilePath = CreateParametersFilePath(temporaryDirectoryPath);

            try
            {
                Directory.CreateDirectory(temporaryDirectoryPath);
                File.WriteAllText(parametersFilePath, "null");

                var store = new ParametersFileStore(parametersFilePath);

                var loaded = store.TryLoad(out var parameters);

                Assert.That(loaded, Is.False);
                Assert.That(parameters, Is.Null);
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    Directory.Delete(temporaryDirectoryPath, recursive: true);
                }
            }
        }

        /// <summary>
        /// Создаёт валидный набор параметров для тестов.
        /// </summary>
        private static Parameters CreateValidParameters()
        {
            var parameter = new Parameters();

            parameter.SetOuterDiameterD(450);
            parameter.SetThicknessT(45);
            parameter.SetHoleDiameterd(28);
            parameter.SetChamferRadiusR(5);
            parameter.SetRecessRadiusL(120);
            parameter.SetRecessDepthG(15);

            parameter.ValidateAll();

            return parameter;
        }

        /// <summary>
        /// Создаёт не валидный набор параметров для тестов.
        /// </summary>
        private static Parameters CreateInvalidParameters()
        {
            var parameter = new Parameters();

            parameter.SetOuterDiameterD(100);
            parameter.SetThicknessT(50);

            parameter.SetHoleDiameterd(28);
            parameter.SetChamferRadiusR(5);
            parameter.SetRecessRadiusL(40);
            parameter.SetRecessDepthG(5);

            return parameter;
        }
    }
}

