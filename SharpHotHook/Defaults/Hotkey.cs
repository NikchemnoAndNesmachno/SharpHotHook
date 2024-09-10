using SharpHook.Native;

namespace SharpHotHook.Defaults;

public class Hotkey: IHotkey
{
    public Hotkey()
    {
    }
    
    public Hotkey(Action onHotkey, KeyCode[] keyCodes)
    {
        KeyCodes = keyCodes;
        OnHotkey = onHotkey;
    }


    public int ActivatedKeys { get; set; }
    public bool[] IsActivated { get; set; }
    public KeyCode[] KeyCodes { get; set; }
    public Action OnHotkey { get; set; }
}