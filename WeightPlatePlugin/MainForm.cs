using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WeightPlatePluginCore.Model;
using WeightPlatePlugin.Wrapper;
using WeightPlatePluginCore.Presets;

namespace WeightPlatePlugin
{
    /// <summary>
    /// Форма ввода параметров и запуска построения модели диска.
    /// </summary>
    public partial class WeightPlatePlugin : Form
    {
        /// <summary>
        /// Провайдер ошибок для подсветки полей и вывода подсказок.
        /// </summary>
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        //TODO: XML +
        /// <summary>
        /// Словарь для связи идентификаторов параметров с соответствующими текстовыми полями ввода.
        /// Обеспечивает доступ к элементам управления для отображения ошибок валидации.
        /// </summary>
        private readonly Dictionary<ParameterId, TextBox> _parameterInputs;
        /// <summary>
        /// Модель параметров диска, содержащая текущие значения всех введенных пользователем параметров.
        /// Обеспечивает валидацию и управление значениями параметров.
        /// </summary>
        private readonly Parameters _parameters = new Parameters();
        /// <summary>
        /// Построитель 3D-модели диска, использующий API КОМПАС-3D для создания геометрической модели
        /// на основе валидных параметров.
        /// </summary>s
        private readonly Builder _builder;

        /// <summary>
        /// Защита от рекурсивных событий при синхронизации UI.
        /// </summary>
        private bool _isUiSync;

        /// <summary>
        /// Конструктор формы.
        /// </summary>
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
                pair.Value.TextChanged += textBox_TextChanged;
            }

            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            InitializePresetComboBox();
            ApplyPreset(WeightPlatePresetCatalog.GetById(WeightPlatePresetCatalog.DefaultPresetId));

            ResetToDefaults();
        }
        /// <summary>
        /// Обработчик кнопки "Сбросить до значений по умолчанию".
        /// </summary>
        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetToDefaults();
        }

        /// <summary>
        /// Обработчик кнопки "Построить".
        /// </summary>
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
                    throw new ArgumentOutOfRangeException(
                        nameof(parameterId),
                        parameterId,
                        "Неизвестный параметр.");
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

        /// <summary>
        /// Подсвечивает текстовое поле и устанавливает текст ошибки.
        /// </summary>
        private void SetTextBoxError(TextBox textBox, string message)
        {
            textBox.BackColor = Color.LightCoral;
            _errorProvider.SetError(textBox, message);
        }

        /// <summary>
        /// Очищает все ошибки и сбрасывает подсветку полей.
        /// </summary>
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
            ApplyPreset(WeightPlatePresetCatalog.GetById(WeightPlatePresetCatalog.DefaultPresetId));
        }


        /// <summary>
        /// Пытается распарсить число.
        /// </summary>
        private static bool TryParseDouble(string text, out double value)
        {
            var styles = NumberStyles.Float | NumberStyles.AllowThousands;

            return double.TryParse(
                text,
                styles,
                CultureInfo.CurrentCulture,
                out value) ||
                   double.TryParse(
                       text,
                       styles,
                       CultureInfo.InvariantCulture,
                       out value);
        }


        /// <summary>
        /// Настраивает ComboBox с пресетами.
        /// Название контролла: presetComboBox.
        /// </summary>
        private void InitializePresetComboBox()
        {
            presetComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            presetComboBox.BeginUpdate();
            try
            {
                presetComboBox.Items.Clear();

                var presets = WeightPlatePresetCatalog.GetAll();
                for (int i = 0; i < presets.Count; i++)
                {
                    presetComboBox.Items.Add(presets[i]);
                }

                presetComboBox.SelectedIndexChanged -= comboBoxPreset_SelectedIndexChanged;
                presetComboBox.SelectedIndexChanged += comboBoxPreset_SelectedIndexChanged;
            }
            finally
            {
                presetComboBox.EndUpdate();
            }
        }

        private void comboBoxPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUiSync)
            {
                return;
            }

            var preset = presetComboBox.SelectedItem as WeightPlatePreset;
            if (preset == null)
            {
                return;
            }

            if (preset.IsCustom)
            {
                return;
            }

            ApplyPreset(preset);
        }

        private void ApplyPreset(WeightPlatePreset preset)
        {
            if (preset == null)
            {
                throw new ArgumentNullException(nameof(preset));
            }

            if (preset.IsCustom)
            {
                SelectPreset(WeightPlatePresetId.Custom);
                return;
            }

            _isUiSync = true;
            try
            {
                // 1) модель
                _parameters.CopyFrom(preset.Parameters);

                // 2) UI
                SetTextBox(textBoxD, _parameters.OuterDiameterD);
                SetTextBox(textBoxT, _parameters.ThicknessT);
                SetTextBox(textBoxHoleDiameter, _parameters.HoleDiameterd);
                SetTextBox(textBoxR, _parameters.ChamferRadiusR);
                SetTextBox(textBoxL, _parameters.RecessRadiusL);
                SetTextBox(textBoxG, _parameters.RecessDepthG);

                // 3) выбор пресета в UI
                SelectPreset(preset.Id);

                ClearAllErrors();
            }
            finally
            {
                _isUiSync = false;
            }
        }

        private void SelectPreset(WeightPlatePresetId presetId)
        {
            for (int i = 0; i < presetComboBox.Items.Count; i++)
            {
                var item = presetComboBox.Items[i] as WeightPlatePreset;
                if (item != null && item.Id == presetId)
                {
                    presetComboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private static void SetTextBox(TextBox textBox, double value)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException(nameof(textBox));
            }

            textBox.Text = value.ToString("0.###", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Любая ручная правка параметров переводит ComboBox в "Пользовательский".
        /// </summary>
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (_isUiSync)
            {
                return;
            }

            _isUiSync = true;
            try
            {
                SelectPreset(WeightPlatePresetId.Custom);
            }
            finally
            {
                _isUiSync = false;
            }
        }

    }
}

