using System;
using System.Globalization;
using WeightPlatePluginCore.Model;

namespace WeightPlatePlugin.Wrapper
{
    /// <summary>
    /// Оркестратор построения модели блина.
    /// </summary>
    public class Builder
    {
        /// <summary>
        /// Обёртка над Kompas 3D API.
        /// </summary>
        private readonly Wrapper _wrapper;

        /// <summary>
        /// Текущие параметры модели.
        /// </summary>
        private Parameters _parameters = null!;

        /// <summary>
        /// Создаёт новый объект построителя модели.
        /// </summary>
        /// <param name="wrapper">Обёртка над Kompas 3D API.</param>
        public Builder(Wrapper wrapper)
        {
            _wrapper = wrapper ?? throw new ArgumentNullException(nameof(wrapper));
        }

        /// <summary>
        /// Параметры, по которым была построена текущая модель.
        /// </summary>
        public Parameters CurrentParameters => _parameters;

        /// <summary>
        /// Главный сценарий построения модели диска.
        /// </summary>
        public void Build(Parameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _parameters = parameters;

            // 1. Подключаемся к KOMPAS и создаём новый документ
            _wrapper.AttachOrRunCAD();
            _wrapper.CreateDocument3D();

            // 2. Создаём заготовку
            BuildBlankDisk();

            // 3. Вырезаем центральное отверстие
            CutCenterHole();

            // 4. Вырезаем внутреннее углубление
            CutInnerRecess();

            // 5. Фаски / скругления
            ApplyChamferOrFillet();
            
            try
            {
                //6. Создаем уникальное имя
                var directory = GetModelsDirectory();
                var filePath = Path.Combine(directory, CreateFileName(_parameters));


                //7. Сохраняем в файл
                SaveModel(filePath);
            }
            finally
            {
                _wrapper.CloseActiveDocument();
            }
        }

        /// <summary>
        /// Создаёт заготовку диска по D и T.
        /// Реализовано через выдавливание окружности на толщину T.
        /// </summary>
        public void BuildBlankDisk()
        {
            // Эскиз на плоскости XOY
            var sketch = _wrapper.CreateSketchOnPlane("XOY");

            // Наружный радиус D/2
            var radius = _parameters.OuterDiameterD / 2.0;

            // Центр в (0, 0)
            _wrapper.DrawCircle(0.0, 0.0, radius);

            // Завершаем эскиз
            _wrapper.FinishSketch(sketch);

            // Создаём тело вращения в нашем случае эквивалентно:
            // выдавливаем окружность на толщину T (получаем цилиндр)
            _wrapper.BossByRevolve(sketch, "OZ", _parameters.ThicknessT);
        }

        /// <summary>
        /// Вырезает центральное отверстие d сквозным выдавливанием.
        /// </summary>
        public void CutCenterHole()
        {
            // Эскиз на той же плоскости XOY
            var sketch = _wrapper.CreateSketchOnPlane("XOY");

            var radius = _parameters.HoleDiameterd / 2.0;

            _wrapper.DrawCircle(0.0, 0.0, radius);
            _wrapper.FinishSketch(sketch);

            // "Сквозной" вырез через весь диск:
            // глубина берётся из толщины T с запасом
            _wrapper.CutByExtrusionThroughAll(
                sketch,
                _parameters.ThicknessT,
                forward: false);
        }

        /// <summary>
        /// Формирует внутренние углубления радиуса L и глубины G
        /// на обеих сторонах диска.
        /// </summary>
        public void CutInnerRecess()
        {
            var radius = _parameters.RecessRadiusL;
            var depth = _parameters.RecessDepthG;
            var thickness = _parameters.ThicknessT;

            if (depth <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(depth),
                    "Глубина углубления должна быть > 0.");
            }

            if (depth >= thickness)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(depth),
                    "Глубина углубления G должна быть меньше толщины диска T.");
            }

            // 1) Углубление со стороны базовой плоскости XOY
            var sketchBottom = _wrapper.CreateSketchOnPlane("XOY");
            _wrapper.DrawCircle(0.0, 0.0, radius);
            _wrapper.FinishSketch(sketchBottom);
            _wrapper.CutByExtrusionDepth(sketchBottom, depth, forward: false);

            // 2) Углубление с противоположной стороны:
            // плоскость на расстоянии (T - G) от XOY, т.е. внутри тела
            var offset = thickness - depth;

            var sketchTop = _wrapper.CreateSketchOnOffsetPlaneFromXOY(thickness);
            _wrapper.DrawCircle(0.0, 0.0, radius);
            _wrapper.FinishSketch(sketchTop);
            _wrapper.CutByExtrusionDepth(sketchTop, depth, forward: true);
        }

        /// <summary>
        /// Применяет фаски/скругления радиуса R.
        /// Пока без привязки к конкретным рёбрам — оставлено под расширение.
        /// </summary>
        public void ApplyChamferOrFillet()
        {
            _wrapper.ApplyChamferOrFillet(_parameters.ChamferRadiusR, null);
        }

        /// <summary>
        /// Сохраняет построенную модель в файл.
        /// </summary>
        /// <param name="path">Путь к файлу модели.</param>
        public void SaveModel(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Путь к файлу не задан.", nameof(path));
            }

            _wrapper.SaveAs(path);
        }

        /// <summary>
        /// Формирует уникальное имя файла по параметрам и времени.
        /// </summary>
        private static string CreateFileName(Parameters p)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                //TODO: RSDN
                "WeightPlate_D{0:0}_T{1:0}_d{2:0}_R{3:0}_L{4:0}_G{5:0}_{6:yyyyMMdd_HHmmss_fff}.m3d",
                p.OuterDiameterD,
                p.ThicknessT,
                p.HoleDiameterd,
                p.ChamferRadiusR,
                p.RecessRadiusL,
                p.RecessDepthG,
                DateTime.Now);
        }

        /// <summary>
        /// Возвращает путь к каталогу, в который сохраняются модели.
        /// </summary>
        private static string GetModelsDirectory()
        {
            var documents = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);

            var directory = Path.Combine(
                documents, "WeightPlatePlugin", "Models");

            Directory.CreateDirectory(directory);

            return directory;
        }

    }
}
