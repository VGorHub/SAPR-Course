using System;
using WeightPlatePlugin.Abstractions.Core;

namespace WeightPlatePlugin.Abstractions.Presentation
{
    /// <summary>Абстракция UI, независимая от WinForms/WPF.</summary>
    public interface IMainView
    {
        /// <summary>Параметры, привязанные к полям ввода.</summary>
        IWeightPlateParameters Parameters { get; set; }

        /// <summary>Событие: пользователь нажал «Построить».</summary>
        event EventHandler BuildRequested;

        /// <summary>Показать сообщение пользователю.</summary>
        void ShowMessage(string message, string title = null);

        /// <summary>Подсветить ошибку поля параметра.</summary>
        void HighlightError(ParamType type, string message);

        /// <summary>Снять подсветку ошибок.</summary>
        void ClearErrors();
    }
}
