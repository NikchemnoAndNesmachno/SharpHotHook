using SharpHook.Native;

namespace SharpHotHook;

public static class HotkeyMethods
{
    public static void Reset(this IHotkey hotkey)
    {
        hotkey.IsActivated = Enumerable.Repeat(false, hotkey.KeyCodes.Length).ToArray();
        hotkey.ActivatedKeys = 0;
    }


    public static void ActivateKey(this IHotkey hotkey, KeyCode key)
    {
        var index = Array.IndexOf( hotkey.KeyCodes, key);
        if (index == -1) return;
        if(hotkey.IsActivated[index]) return;
        
        hotkey.ActivatedKeys++;
        hotkey.IsActivated[index] = true;
        
        if (hotkey.ActivatedKeys != hotkey.KeyCodes.Length) return;
        hotkey.OnHotkey();
    }
    
    public static void DeactivateKey(this IHotkey hotkey, KeyCode key)
    {
        var index = Array.IndexOf(hotkey.KeyCodes, key);
        if (index == -1) return;
        if(!hotkey.IsActivated[index]) return;
        
        hotkey.ActivatedKeys--;
        hotkey.IsActivated[index] = false;
    }
}