using SharpHook.Native;

namespace SharpHotHook.Defaults;

public class HotkeyContainerDefault: IHotkeyContainer
{
    public KeyCode[] PauseHotkey { get; set; } = [];
    public IList<IHotkey> Hotkeys { get; set; } = [];
    public bool IsPaused { get; set; }
    public void Add(KeyCode[] codes, Action action)
    {
        Hotkeys.Add(new Hotkey(action, codes));
    }
}