using WeightPlatePlugin.Abstractions.Building;
using WeightPlatePlugin.Abstractions.Cad;
using WeightPlatePlugin.Abstractions.Core;
using WeightPlatePlugin.Abstractions.Presentation;

namespace WeightPlatePlugin.Abstractions.Composition
{
    /// <summary>Фабрика для сборки зависимостей плагина.</summary>
    public interface IPluginFactory
    {
        IKompasWrapper CreateKompasWrapper();
        IWeightPlateParameters CreateParameters();
        IWeightPlateBuilder CreateBuilder(IKompasWrapper wrapper);
        IMainView CreateMainView();
        IMainPresenter CreatePresenter(IMainView view, IWeightPlateBuilder builder, IWeightPlateParameters parameters);
    }
}
