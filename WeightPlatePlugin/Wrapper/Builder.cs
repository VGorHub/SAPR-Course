using System;
using WeightPlatePlugin.Model;

namespace WeightPlatePlugin.Wrapper
{
    /// <summary>
    /// Оркестратор построения модели
    /// </summary>
    public class Builder
    {
        private readonly Wrapper _wrapper;
        private Parameters _parameters;

        public Builder(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public Parameters CurrentParameters => _parameters;

        /// <summary>
        /// Главный сценарий построения модели диска.
        /// </summary>
        public void Build(Parameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            if (!IsWrapperReady())
            {
                return;
            }

            BuildBlankDisk();
            CutCenterHole();
            CutInnerRecess();
            ApplyChamferOrFillet();
        }

        /// <summary>Создаёт заготовку диска по D и T.</summary>
        public void BuildBlankDisk()
        {
            if (!IsWrapperReady())
            {
                return;
            }

            _wrapper.AttachOrRunCAD();

            // Здесь в будущем будет последовательность:
            // CreateDocument3D -> GetTopPart -> CreateSketchOnPlane -> DrawCircle -> FinishSketch -> BossByRevolve
        }

        /// <summary>Вырезает центральное отверстие d сквозным выдавливанием.</summary>
        public void CutCenterHole()
        {
            if (!IsWrapperReady())
            {
                return;
            }

            // TODO: реализовать через CutByExtrusionThroughAll после реализации Wrapper.
        }

        /// <summary>Формирует внутреннее углубление L, G.</summary>
        public void CutInnerRecess()
        {
            if (!IsWrapperReady())
            {
                return;
            }

            // TODO: реализовать через CutByExtrusionDepth после реализации Wrapper.
        }

        /// <summary>Применяет фаски/скругления радиуса R.</summary>
        public void ApplyChamferOrFillet()
        {
            if (!IsWrapperReady())
            {
                return;
            }

            _wrapper.ApplyChamferOrFillet(_parameters.ChamferRadiusR, null);
        }

        public void SaveModel(string path)
        {
            if (!IsWrapperReady())
            {
                return;
            }

            _wrapper.SaveAs(path);
        }

        private bool IsWrapperReady() => _wrapper != null;
    }
}
