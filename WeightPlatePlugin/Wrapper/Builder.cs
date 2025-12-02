using System;
using WeightPlatePlugin.Model;

namespace WeightPlatePlugin.Wrapper
{
    /// <summary>
    /// Оркестратор построения модели блина.
    /// </summary>
    public class Builder
    {
        //TODO: XML
        private readonly Wrapper _wrapper;

        //TODO: XML
        private Parameters _parameters = null!;

        //TODO: XML
        public Builder(Wrapper wrapper)
        {
            _wrapper = wrapper ?? throw new ArgumentNullException(nameof(wrapper));
        }

        //TODO: XML
        public Parameters CurrentParameters => _parameters;

        /// <summary>
        /// Главный сценарий построения модели диска.
        /// </summary>
        public void Build(Parameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

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

            //TODO: refactor
            if (depth <= 0)
                //TODO: RSDN
                throw new ArgumentException("Глубина углубления должна быть > 0.", nameof(_parameters.RecessDepthG));

            //TODO: refactor
            if (depth >= thickness)
                //TODO: RSDN
                throw new ArgumentException("Глубина углубления G должна быть меньше толщины диска T.");

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

        //TODO: XML
        public void SaveModel(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Путь к файлу не задан.", nameof(path));
            }

            _wrapper.SaveAs(path);
        }
    }
}
