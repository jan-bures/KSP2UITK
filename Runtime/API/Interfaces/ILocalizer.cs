using System;

namespace UitkForKsp2.API.Interfaces;

public interface ILocalizer
{
    public static ILocalizer Instance;
    public event Action OnLocalize;

    public string? GetTranslation(string key, params object[] p);
}