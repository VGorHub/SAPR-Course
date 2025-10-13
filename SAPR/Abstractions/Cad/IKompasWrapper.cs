namespace WeightPlatePlugin.Abstractions.Cad
{
    /// <summary>Обёртка над API КОМПАС-3D: низкоуровневые операции построения.</summary>
    public interface IKompasWrapper
    {
        /// <summary>Инициализация/подключение к КОМПАС-3D.</summary>
        void OpenCad();

        /// <summary>Создать документ детали.</summary>
        void CreatePartDocument(string name = null);

        /// <summary>Начать эскиз на плоскости.</summary>
        void BeginSketch(string planeName);

        /// <summary>Завершить текущий эскиз.</summary>
        void EndSketch();

        /// <summary>Отрезок в текущем эскизе.</summary>
        void SketchLine(double x1, double y1, double x2, double y2);

        /// <summary>Дуга окружности в текущем эскизе.</summary>
        void SketchArc(double cx, double cy, double radius, double startAngleDeg, double endAngleDeg);

        /// <summary>Вращение эскиза вокруг оси.</summary>
        void Revolve(string axisName, double angleDeg);

        /// <summary>Вырезание выдавливанием по текущему эскизу.</summary>
        void ExtrudeCut(double depth);

        /// <summary>Скругление кромок модели.</summary>
        void Fillet(double radius);

        /// <summary>Сохранить документ в файл.</summary>
        void SaveAs(string filePath);
    }
}
