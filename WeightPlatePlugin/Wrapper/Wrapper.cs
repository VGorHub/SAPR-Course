using System;

namespace WeightPlatePlugin.Wrapper
{
    /// <summary>
    /// Обёртка над Kompas API.
    /// Здесь только сигнатуры методов, реализация будет добавлена позже.
    /// </summary>
    public class Wrapper
    {
        public void AttachOrRunCAD()
        {
            throw new NotImplementedException();
        }

        public void CreateDocument3D()
        {
            throw new NotImplementedException();
        }

        public object GetTopPart()
        {
            throw new NotImplementedException();
        }

        public object CreateSketchOnPlane(string plane)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(double xc, double yc, double radius)
        {
            throw new NotImplementedException();
        }

        public void FinishSketch(object sketch)
        {
            throw new NotImplementedException();
        }

        public void BossByRevolve(object sketch, string axis, double angleDeg)
        {
            throw new NotImplementedException();
        }

        public void CutByExtrusionThroughAll(object sketch, bool forward)
        {
            throw new NotImplementedException();
        }

        public void CutByExtrusionDepth(object sketch, double depth, bool forward)
        {
            throw new NotImplementedException();
        }

        public void ApplyChamferOrFillet(double radius, object edges)
        {
            throw new NotImplementedException();
        }

        public void SaveAs(string path)
        {
            throw new NotImplementedException();
        }
    }
}
