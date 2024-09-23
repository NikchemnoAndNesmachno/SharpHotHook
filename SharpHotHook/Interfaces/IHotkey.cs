using SharpHook.Native;

namespace SharpHotHook.Interfaces;

public interface IHotkey
{
    int ActivatedKeys { get; set; }
    IList<bool> IsActivated { get; set; }
    IList<KeyCode> KeyCodes { get; set; } 
    
    public Action OnHotkey { get; set; }
}