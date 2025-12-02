using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WeightPlatePlugin.Model;
using WeightPlatePlugin.Wrapper;

namespace WeightPlatePlugin
{
    /// <summary>
    /// Форма ввода параметров и запуска построения модели диска.
    /// </summary>
    public partial class WeightPlatePlugin : Form
    {
        //TODO: XML
        private readonly ErrorProvider _errorProvider = new ErrorProvider();
        private readonly Dictionary<ParameterId, TextBox> _parameterInputs;
        private readonly Parameters _parameters = new Parameters();
        private readonly Builder _builder;

        public WeightPlatePlugin()
        {
            InitializeComponent();

            _builder = new Builder(new Wrapper.Wrapper());

            _parameterInputs = new Dictionary<ParameterId, TextBox>
            {
                { ParameterId.OuterDiameterD, textBoxD },
                { ParameterId.ThicknessT,    textBoxT },
                { ParameterId.HoleDiameterd, textBoxHoleDiameter },
                { ParameterId.ChamferRadiusR,textBoxR },
                { ParameterId.RecessRadiusL, textBoxL },
                { ParameterId.RecessDepthG,  textBoxG }
            };

            foreach (var pair in _parameterInputs)
            {
                pair.Value.Tag = pair.Key;
                pair.Value.Validating += textBox_Validating;
            }

            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            ResetToDefaults();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetToDefaults();
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            BuildModel();
        }

        /// <summary>
        /// Один общий обработчик Validating для всех полей.
        /// </summary>
        private void textBox_Validating(object sender, CancelEventArgs e)
        {
            if (sender is not TextBox textBox || textBox.Tag is not ParameterId parameterId)
            {
                return;
            }

            if (!TryParseDouble(textBox.Text, out var value))
            {
                SetTextBoxError(textBox, "Введите числовое значение.");
                return;
            }

            try
            {
                ApplyParameterValue(parameterId, value);

                ClearAllErrors();
                _parameters.ValidateAll();
            }
            catch (ValidationException ex)
            {
                ClearAllErrors();
                ApplyValidationError(ex);
            }
        }

        /// <summary>
        /// Валидация всех полей и запуск построения.
        /// </summary>
        private void BuildModel()
        {
            ClearAllErrors();

            foreach (var pair in _parameterInputs)
            {
                var parameterId = pair.Key;
                var textBox = pair.Value;

                if (!TryParseDouble(textBox.Text, out var value))
                {
                    SetTextBoxError(textBox, "Введите числовое значение.");
                    return;
                }

                try
                {
                    ApplyParameterValue(parameterId, value);
                }
                catch (ValidationException ex)
                {
                    ApplyValidationError(ex);
                    return;
                }
            }

            try
            {
                _parameters.ValidateAll();
                _builder.Build(_parameters);

                MessageBox.Show(
                    "Модель успешно построена в КОМПАС-3D.",
                    "Готово",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            }
            catch (ValidationException ex)
            {
                ClearAllErrors();
                ApplyValidationError(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось построить модель: {ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Записывает значение параметра в модель.
        /// Все проверки диапазонов выполняются внутри Parameters через ValidationException.
        /// </summary>
        private void ApplyParameterValue(ParameterId parameterId, double value)
        {
            switch (parameterId)
            {
                case ParameterId.OuterDiameterD:
                    _parameters.SetOuterDiameterD(value);
                    break;
                case ParameterId.ThicknessT:
                    _parameters.SetThicknessT(value);
                    break;
                case ParameterId.HoleDiameterd:
                    _parameters.SetHoleDiameterd(value);
                    break;
                case ParameterId.ChamferRadiusR:
                    _parameters.SetChamferRadiusR(value);
                    break;
                case ParameterId.RecessRadiusL:
                    _parameters.SetRecessRadiusL(value);
                    break;
                case ParameterId.RecessDepthG:
                    _parameters.SetRecessDepthG(value);
                    break;
                default:
                    //TODO: RSDN
                    throw new ArgumentOutOfRangeException(nameof(parameterId), parameterId, "Неизвестный параметр.");
            }
        }

        /// <summary>
        /// Подсвечивает все поля, для которых модель вернула ошибки валидации.
        /// </summary>
        private void ApplyValidationError(ValidationException exception)
        {
            foreach (var error in exception.GetErrors())
            {
                if (_parameterInputs.TryGetValue(error.Parameter, out var textBox))
                {
                    SetTextBoxError(textBox, error.Message);
                }
            }
        }

        //TODO: XML
        private void SetTextBoxError(TextBox textBox, string message)
        {
            textBox.BackColor = Color.LightCoral;
            _errorProvider.SetError(textBox, message);
        }

        //TODO: XML
        private void ClearAllErrors()
        {
            foreach (var textBox in _parameterInputs.Values)
            {
                textBox.BackColor = Color.White;
                _errorProvider.SetError(textBox, string.Empty);
            }
        }

        /// <summary>
        /// Устанавливает значения параметров по умолчанию и синхронизирует их с моделью.
        /// </summary>
        private void ResetToDefaults()
        {
            textBoxD.Text = "450";
            textBoxT.Text = "45";
            textBoxHoleDiameter.Text = "28";
            textBoxR.Text = "5";
            textBoxL.Text = "120";
            textBoxG.Text = "15";

            ApplyParameterValue(ParameterId.OuterDiameterD, 450);
            ApplyParameterValue(ParameterId.ThicknessT, 45);
            ApplyParameterValue(ParameterId.HoleDiameterd, 28);
            ApplyParameterValue(ParameterId.ChamferRadiusR, 5);
            ApplyParameterValue(ParameterId.RecessRadiusL, 120);
            ApplyParameterValue(ParameterId.RecessDepthG, 15);

            ClearAllErrors();
        }

        /// <summary>
        /// Пытается распарсить число.
        /// </summary>
        private static bool TryParseDouble(string text, out double value)
        {
            var styles = NumberStyles.Float | NumberStyles.AllowThousands;

            //TODO: RSDN
            return double.TryParse(text, styles, CultureInfo.CurrentCulture, out value) ||
                   double.TryParse(text, styles, CultureInfo.InvariantCulture, out value);
        }
    }
}

