using SharpHook;
using SharpHook.Native;

namespace SharpHotHook;

public interface IHotkeyContainer
{
    KeyCode[] PauseHotkey { get; set; }
    IList<IHotkey> Hotkeys { get; set; }
    bool IsPaused { get; set; }

    public void Add(KeyCode[] codes, Action action);
}