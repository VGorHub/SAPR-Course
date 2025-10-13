using WeightPlatePlugin.Abstractions.Core;

namespace WeightPlatePlugin.Abstractions.Building
{
    /// <summary>Построитель модели блина (сценарий высокого уровня).</summary>
    public interface IWeightPlateBuilder
    {
        /// <summary>Полное построение модели по параметрам.</summary>
        void Build(IWeightPlateParameters parameters);

        /// <summary>Создать 2D-профиль для операции вращения.</summary>
        void BuildProfile(IWeightPlateParameters parameters);

        /// <summary>Вращение профиля в тело.</summary>
        void RevolvePlate(IWeightPlateParameters parameters);

        /// <summary>Создание центрального отверстия (Ø d).</summary>
        void MakeCenterHole(IWeightPlateParameters parameters);

        /// <summary>Создание внутреннего углубления (диаметр L, глубина G).</summary>
        void MakeRecess(IWeightPlateParameters parameters);

        /// <summary>Применение фасок/скруглений по кромкам (R).</summary>
        void ApplyFillets(IWeightPlateParameters parameters);
    }
}
