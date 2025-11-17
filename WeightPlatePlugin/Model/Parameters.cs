using System;
using System.Collections.Generic;

namespace WeightPlatePlugin.Model
{
    /// <summary>
    /// Параметры диска и их валидация.
    /// </summary>
    public class Parameters
    {
        private double _chamferRadiusR;
        private double _holeDiameterd;
        private double _outerDiameterD;
        private double _recessDepthG;
        private double _recessRadiusL;
        private double _thicknessT;

        public double ChamferRadiusR => _chamferRadiusR;
        public double HoleDiameterd => _holeDiameterd;
        public double OuterDiameterD => _outerDiameterD;
        public double RecessDepthG => _recessDepthG;
        public double RecessRadiusL => _recessRadiusL;
        public double ThicknessT => _thicknessT;

        public void SetOuterDiameterD(double value) => _outerDiameterD = value;
        public void SetThicknessT(double value) => _thicknessT = value;
        public void SetHoleDiameterd(double value) => _holeDiameterd = value;
        public void SetChamferRadiusR(double value) => _chamferRadiusR = value;
        public void SetRecessRadiusL(double value) => _recessRadiusL = value;
        public void SetRecessDepthG(double value) => _recessDepthG = value;

        /// <summary>
        /// Полная проверка параметров:
        /// диапазоны + взаимосвязи. При наличии хотя бы одной ошибки
        /// бросает ValidationException со списком всех ошибок.
        /// </summary>
        public void ValidateAll()
        {
            var errors = new List<ValidationError>();

            // --- Диапазоны по ТЗ ---

            if (_outerDiameterD < 100 || _outerDiameterD > 500)
            {
                errors.Add(new ValidationError(
                    ParameterId.OuterDiameterD,
                    "Наружный диаметр D должен быть в диапазоне 100–500 мм."));
            }

            if (_thicknessT < 10 || _thicknessT > 80)
            {
                errors.Add(new ValidationError(
                    ParameterId.ThicknessT,
                    "Толщина T должна быть в диапазоне 10–80 мм."));
            }

            if (_holeDiameterd < 26 || _holeDiameterd > 51)
            {
                errors.Add(new ValidationError(
                    ParameterId.HoleDiameterd,
                    "Диаметр отверстия d должен быть в диапазоне 26–51 мм."));
            }

            if (_chamferRadiusR < 2 || _chamferRadiusR > 10)
            {
                errors.Add(new ValidationError(
                    ParameterId.ChamferRadiusR,
                    "Радиус скругления фаски R должен быть в диапазоне 2–10 мм."));
            }

            if (_recessRadiusL <= 0)
            {
                errors.Add(new ValidationError(
                    ParameterId.RecessRadiusL,
                    "Радиус внутреннего углубления L должен быть больше 0."));
            }

            if (_recessDepthG <= 0)
            {
                errors.Add(new ValidationError(
                    ParameterId.RecessDepthG,
                    "Глубина внутреннего углубления G должна быть больше 0."));
            }

            // T ≤ D/10
            if (_outerDiameterD > 0 && _thicknessT > _outerDiameterD / 10.0)
            {
                const string message = "Толщина T должна удовлетворять условию T ≤ D/10.";
                errors.Add(new ValidationError(ParameterId.ThicknessT, message));
                errors.Add(new ValidationError(ParameterId.OuterDiameterD, message));
            }

            // d < D
            if (_outerDiameterD > 0 && _holeDiameterd >= _outerDiameterD)
            {
                const string message = "Диаметр отверстия d должен быть меньше наружного диаметра D (d < D).";
                errors.Add(new ValidationError(ParameterId.HoleDiameterd, message));
                errors.Add(new ValidationError(ParameterId.OuterDiameterD, message));
            }

            // d < L < D
            if (_outerDiameterD > 0 && _recessRadiusL > 0 && _holeDiameterd > 0)
            {
                bool ok = _holeDiameterd < _recessRadiusL && _recessRadiusL < _outerDiameterD;
                if (!ok)
                {
                    const string message = "Радиус внутреннего углубления L должен удовлетворять неравенству d < L < D.";
                    errors.Add(new ValidationError(ParameterId.RecessRadiusL, message));
                    errors.Add(new ValidationError(ParameterId.HoleDiameterd, message));
                    errors.Add(new ValidationError(ParameterId.OuterDiameterD, message));
                }
            }

            // 0 < G < T
            if (_recessDepthG > 0 && _thicknessT > 0)
            {
                bool ok = _recessDepthG > 0 && _recessDepthG < _thicknessT;
                if (!ok)
                {
                    const string message = "Глубина внутреннего углубления G должна удовлетворять неравенству 0 < G < T.";
                    errors.Add(new ValidationError(ParameterId.RecessDepthG, message));
                    errors.Add(new ValidationError(ParameterId.ThicknessT, message));
                }
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
