using SharpHook.Native;

namespace SharpHotHook;

public class HotkeyContainerDefault: IHotkeyContainer
{
    public IList<KeyCode> Codes { get; set; } = [];
    public KeyCode[] PauseHotkey { get; set; } = [];
    public IList<IHotkey> Hotkeys { get; set; } = [];
    public bool IsPaused { get; set; }
    public void Add(KeyCode[] codes, Action action)
    {
        Hotkeys.Add(new Hotkey(action, codes));
    }
}