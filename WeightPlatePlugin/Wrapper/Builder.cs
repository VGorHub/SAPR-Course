using System;
using WeightPlatePlugin.Model;

namespace WeightPlatePlugin.Wrapper
{
    /// <summary>
    /// Оркестратор построения модели блина.
    /// </summary>
    public class Builder
    {
        private readonly Wrapper _wrapper;
        private Parameters _parameters = null!;

        public Builder(Wrapper wrapper)
        {
            _wrapper = wrapper ?? throw new ArgumentNullException(nameof(wrapper));
        }

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

            // Сквозной вырез по всему телу
            _wrapper.CutByExtrusionThroughAll(sketch, forward: true);
        }

        /// <summary>
        /// Формирует внутреннее углубление радиуса L и глубины G.
        /// </summary>
        public void CutInnerRecess()
        {
            // Эскиз на плоскости XOY:
            // вырезаем цилиндр на глубину G, радиус L.
            var sketch = _wrapper.CreateSketchOnPlane("XOY");

            var radius = _parameters.RecessRadiusL;

            _wrapper.DrawCircle(0.0, 0.0, radius);
            _wrapper.FinishSketch(sketch);

            // Вырез до глубины G в прямом направлении
            _wrapper.CutByExtrusionDepth(sketch, _parameters.RecessDepthG, forward: true);
        }

        /// <summary>
        /// Применяет фаски/скругления радиуса R.
        /// Пока без привязки к конкретным рёбрам — оставлено под расширение.
        /// </summary>
        public void ApplyChamferOrFillet()
        {
            _wrapper.ApplyChamferOrFillet(_parameters.ChamferRadiusR, null);
        }

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
