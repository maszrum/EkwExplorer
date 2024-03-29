﻿namespace EkwExplorer.Core;

public interface IClicker : IDisposable
{
    void GotoHome();

    void FillTextbox(string textboxId, string text);

    void ClickButtonById(string buttonId);

    void ClickButtonByName(string buttonName);

    string GetValueFromTable(string rowCaption);

    bool CheckIfAnyError();

    bool CheckIfNotFound();

    void CloseCookiesInfo();

    IReadOnlyList<string> GetPropertyNumbers();
}
