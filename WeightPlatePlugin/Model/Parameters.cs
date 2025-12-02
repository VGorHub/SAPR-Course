using System;
using System.Collections.Generic;

namespace WeightPlatePlugin.Model
{
    /// <summary>
    /// Параметры диска и их валидация.
    /// </summary>
    public class Parameters
    {
        //TODO: XML +
        /// <summary>
        /// Радиус фаски/скругления кромок (R).
        /// </summary>
        private double _chamferRadiusR;

        /// <summary>
        /// Диаметр центрального отверстия (d).
        /// </summary>
        private double _holeDiameterd;

        /// <summary>
        /// Наружный диаметр диска (D).
        /// </summary>
        private double _outerDiameterD;

        /// <summary>
        /// Глубина внутреннего углубления (G).
        /// </summary>
        private double _recessDepthG;

        /// <summary>
        /// Радиус внутреннего углубления (L).
        /// </summary>
        private double _recessRadiusL;

        /// <summary>
        /// Толщина диска (T).
        /// </summary>
        private double _thicknessT;

        private const double OuterDiameterMin = 100.0;
        private const double OuterDiameterMax = 500.0;

        private const double ThicknessMin = 10.0;
        private const double ThicknessMax = 80.0;

        private const double HoleDiameterMin = 26.0;
        private const double HoleDiameterMax = 51.0;

        private const double ChamferRadiusMin = 2.0;
        private const double ChamferRadiusMax = 10.0;

        /// <summary>
        /// Радиус фаски/скругления кромок (R).
        /// </summary>
        public double ChamferRadiusR => _chamferRadiusR;

        /// <summary>
        /// Диаметр центрального отверстия (d).
        /// </summary>
        public double HoleDiameterd => _holeDiameterd;

        /// <summary>
        /// Наружный диаметр диска (D).
        /// </summary>
        public double OuterDiameterD => _outerDiameterD;

        /// <summary>
        /// Глубина внутреннего углубления (G).
        /// </summary>
        public double RecessDepthG => _recessDepthG;

        /// <summary>
        /// Радиус внутреннего углубления (L).
        /// </summary>
        public double RecessRadiusL => _recessRadiusL;

        /// <summary>
        /// Толщина диска (T).
        /// </summary>
        public double ThicknessT => _thicknessT;

        /// <summary>
        /// Устанавливает наружный диаметр диска D без проверки.
        /// Проверки выполняются в <see cref="ValidateAll"/>.
        /// </summary>
        public void SetOuterDiameterD(double value) => _outerDiameterD = value;

        /// <summary>
        /// Устанавливает толщину диска T без проверки.
        /// Проверки выполняются в <see cref="ValidateAll"/>.
        /// </summary>
        public void SetThicknessT(double value) => _thicknessT = value;

        /// <summary>
        /// Устанавливает диаметр центрального отверстия d без проверки.
        /// Проверки выполняются в <see cref="ValidateAll"/>.
        /// </summary>
        public void SetHoleDiameterd(double value) => _holeDiameterd = value;

        /// <summary>
        /// Устанавливает радиус фаски/скругления R без проверки.
        /// Проверки выполняются в <see cref="ValidateAll"/>.
        /// </summary>
        public void SetChamferRadiusR(double value) => _chamferRadiusR = value;

        /// <summary>
        /// Устанавливает радиус внутреннего углубления L без проверки.
        /// Проверки выполняются в <see cref="ValidateAll"/>.
        /// </summary>
        public void SetRecessRadiusL(double value) => _recessRadiusL = value;

        /// <summary>
        /// Устанавливает глубину внутреннего углубления G без проверки.
        /// Проверки выполняются в <see cref="ValidateAll"/>.
        /// </summary>
        public void SetRecessDepthG(double value) => _recessDepthG = value;

        /// <summary>
        /// Полная проверка параметров:
        /// диапазоны + взаимосвязи. При наличии хотя бы одной ошибки
        /// бросает ValidationException со списком всех ошибок.
        /// </summary>
        public void ValidateAll()
        {
            var errors = new List<ValidationError>();

            // --- Диапазоны по ТЗ (простые, без зависимостей) ---

            //TODO: to const +
            if (_outerDiameterD < OuterDiameterMin || _outerDiameterD > OuterDiameterMax)
            {
                errors.Add(new ValidationError(
                    ParameterId.OuterDiameterD,
                    $"Наружный диаметр D должен быть в диапазоне {OuterDiameterMin:0}–{OuterDiameterMax:0} мм."));
            }

            //TODO: to const +
            if (_thicknessT < ThicknessMin || _thicknessT > ThicknessMax)
            {
                errors.Add(new ValidationError(
                    ParameterId.ThicknessT,
                    $"Толщина T должна быть в диапазоне {ThicknessMin:0}–{ThicknessMax:0} мм."));
            }

            //TODO: to const +
            if (_holeDiameterd < HoleDiameterMin || _holeDiameterd > HoleDiameterMax)
            {
                errors.Add(new ValidationError(
                    ParameterId.HoleDiameterd,
                    $"Диаметр отверстия d должен быть в диапазоне {HoleDiameterMin:0}–{HoleDiameterMax:0} мм."));
            }

            //TODO: to const +
            if (_chamferRadiusR < ChamferRadiusMin || _chamferRadiusR > ChamferRadiusMax)
            {
                errors.Add(new ValidationError(
                    ParameterId.ChamferRadiusR,
                    $"Радиус скругления фаски R должен быть в диапазоне {ChamferRadiusMin:0}–{ChamferRadiusMax:0} мм."));
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

            // --- Взаимосвязи параметров ---

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
                //TODO: RSDN +
                const string message = "Диаметр отверстия d должен быть меньше наружного диаметра D (d < D).";
                errors.Add(new ValidationError(ParameterId.HoleDiameterd, message));
                errors.Add(new ValidationError(ParameterId.OuterDiameterD, message));
            }

            // d < 2L < D  (L — радиус углубления, d и D — диаметры)
            if (_outerDiameterD > 0 && _recessRadiusL > 0 && _holeDiameterd > 0)
            {
                // нижняя и верхняя границы L, выведенные из d < 2L < D
                var minL = _holeDiameterd / 2.0;      // L > d/2
                var maxL = _outerDiameterD / 2.0;     // L < D/2

                //TODO: rename +
                bool isRecessRadiusWithinDiameters =
                    _recessRadiusL > minL && _recessRadiusL < maxL;

                if (!isRecessRadiusWithinDiameters)
                {
                    //TODO: RSDN +
                    string message =
                        "Радиус внутреннего углубления L должен удовлетворять условию d < 2L < D " +
                        $"(то есть L должен быть в диапазоне ({minL:0.###}; {maxL:0.###}) мм).";

                    errors.Add(new ValidationError(ParameterId.RecessRadiusL, message));
                    errors.Add(new ValidationError(ParameterId.HoleDiameterd, message));
                    errors.Add(new ValidationError(ParameterId.OuterDiameterD, message));
                }
            }

            // 0 < G < T
            if (_recessDepthG > 0 && _thicknessT > 0)
            {
                //TODO: rename +
                bool isRecessDepthInRange = _recessDepthG > 0 && _recessDepthG < _thicknessT;

                if (!isRecessDepthInRange)
                {
                    //TODO: RSDN +
                    const string message = "Глубина внутреннего углубления G " +
                        "должна удовлетворять неравенству 0 < G < T.";
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
