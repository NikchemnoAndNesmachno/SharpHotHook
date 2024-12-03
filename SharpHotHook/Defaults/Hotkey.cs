using SharpHook.Native;
using SharpHotHook.Interfaces;

namespace SharpHotHook.Defaults;

public class Hotkey: IHotkey
{
    public Hotkey()
    {
    }

    public Hotkey(IList<KeyCode> keyCodes, Action action)
    {
        KeyCodes = keyCodes;
        OnHotkey = action;
    }

    private IList<KeyCode> _keyCodes = [KeyCode.VcUndefined];
    public int ActivatedKeys { get; set; }
    public IList<bool> IsActivated { get; set; } = new bool[1];

    public IList<KeyCode> KeyCodes
    {
        get => _keyCodes;
        set
        {
            _keyCodes = value;
            ActivatedKeys = 0;
            IsActivated = new bool[_keyCodes.Count];
        }
    }

    public Action OnHotkey { get; set; } = () => { Console.WriteLine("Щось зчитано"); };
}