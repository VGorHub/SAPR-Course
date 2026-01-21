using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using WeightPlatePluginCore.Model;

namespace WeightPlatePluginCore.Persistence
{
    /// <summary>
    /// Файловое хранилище пользовательских параметров блина.
    /// Сохраняет и загружает только валидные значения.
    /// </summary>
    public sealed class ParametersFileStore
    {
        /// <summary>
        /// Полный путь к файлу хранения параметров.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Инициализирует файловое хранилище параметров.
        /// </summary>
        /// <param name="filePath">
        /// Путь к файлу, используемому для сохранения и загрузки параметров.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если <paramref name="filePath"/> не задан или содержит
        /// только пробельные символы.
        /// </exception>
        public ParametersFileStore(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Путь к файлу не задан.", 
                    nameof(filePath));
            }

            _filePath = filePath;
        }

        /// <summary>
        /// Загружает параметры из файла.
        /// </summary>
        public bool TryLoad(out Parameters parameters)
        {
            parameters = null;

            if (!File.Exists(_filePath))
            {
                return false;
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var dto = JsonSerializer.Deserialize<ParametersDto>(json);

                if (dto == null)
                {
                    return false;
                }

                var p = new Parameters();
                dto.ApplyTo(p);
                p.ValidateAll();

                parameters = p;
                return true;
            }
            catch
            {
                // Если файл битый/нечитаемый — просто считаем, что данных нет.
                return false;
            }
        }

        /// <summary>
        /// Сохраняет параметры в файл (атомарно).
        /// </summary>
        public void Save(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            parameters.ValidateAll();

            var dto = ParametersDto.From(parameters);

            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var tempPath = _filePath + ".tmp";

            File.WriteAllText(tempPath, json);

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            File.Move(tempPath, _filePath);
        }

        /// <summary>
        /// DTO для сериализации параметров.
        /// </summary>
        private sealed class ParametersDto
        {
            /// <summary>
            /// Наружный диаметр D, мм.
            /// </summary>
            public double OuterDiameterD { get; set; }

            /// <summary>
            /// Толщина T, мм.
            /// </summary>
            public double ThicknessT { get; set; }

            /// <summary>
            /// Диаметр отверстия d, мм.
            /// </summary>
            public double HoleDiameterd { get; set; }

            /// <summary>
            /// Радиус фаски/скругления R, мм.
            /// </summary>
            public double ChamferRadiusR { get; set; }

            /// <summary>
            /// Радиус внутреннего углубления L, мм.
            /// </summary>
            public double RecessRadiusL { get; set; }

            /// <summary>
            /// Глубина внутреннего углубления G, мм.
            /// </summary>
            public double RecessDepthG { get; set; }

            //TODO: XML
            public static ParametersDto From(Parameters p)
            {
                return new ParametersDto
                {
                    OuterDiameterD = p.OuterDiameterD,
                    ThicknessT = p.ThicknessT,
                    HoleDiameterd = p.HoleDiameterd,
                    ChamferRadiusR = p.ChamferRadiusR,
                    RecessRadiusL = p.RecessRadiusL,
                    RecessDepthG = p.RecessDepthG
                };
            }

            //TODO: XML
            public void ApplyTo(Parameters p)
            {
                p.SetOuterDiameterD(OuterDiameterD);
                p.SetThicknessT(ThicknessT);
                p.SetHoleDiameterd(HoleDiameterd);
                p.SetChamferRadiusR(ChamferRadiusR);
                p.SetRecessRadiusL(RecessRadiusL);
                p.SetRecessDepthG(RecessDepthG);
            }
        }
    }
}
