using System;

namespace EkwClicker
{
    internal interface IClicker : IDisposable
    {
        void FillTextbox(string textboxId, string text);
        void ClickButton(string buttonId);
        string GetValueFromTable(string rowCaption);
        bool CheckIfAnyError();
        void CloseCookiesInfo(string xPath);
    }
}