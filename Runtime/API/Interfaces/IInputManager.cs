using System;

namespace UitkForKsp2.API.Interfaces;

public interface IInputManager
{
    public static IInputManager Instance;
    
    public bool Ready { get; }

    public void SetUitkInputLocks();

    public void RestoreUitkInputLocks();

    public void BindHideAction(Action onHide);
}