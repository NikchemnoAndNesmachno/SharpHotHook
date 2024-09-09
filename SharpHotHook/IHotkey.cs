using SharpHook.Native;

namespace SharpHotHook;

public interface IHotkey
{
    int ActivatedKeys { get; set; }
    bool[] IsActivated { get; set; }
    KeyCode[] KeyCodes { get; set; }
    
    public Action OnHotkey { get; set; }
}