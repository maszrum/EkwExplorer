using System;

namespace EkwClicker
{
    internal interface IClicker : IDisposable
    {
        public string StartingUrl { get; set; }
        
        void GotoHome();
        void FillTextbox(string textboxId, string text);
        void ClickButton(string buttonId);
        string GetValueFromTable(string rowCaption);
        bool CheckIfAnyError();
        void CloseCookiesInfo(string xPath);
    }
}