using SharpHook.Native;

namespace SharpHotHook.Interfaces;

public interface IHotkeyContainer
{
    IList<IHotkey> Hotkeys { get; set; }
    void Add(IHotkey hotkey);
    
    void Remove(IList<KeyCode> codes);
}