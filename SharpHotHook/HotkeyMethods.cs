using SharpHook.Native;
using SharpHotHook.Interfaces;

namespace SharpHotHook;

public static class HotkeyMethods
{
    public static void Reset(this IHotkey hotkey)
    {
        for (int i = 0; i < hotkey.IsActivated.Count; i++)
        {
            hotkey.IsActivated[i] = false;
        }
        hotkey.ActivatedKeys = 0;
    }

    public static bool Equal(this IHotkey hotkey, IList<KeyCode> keyCodes)
    {
        var set1 = new HashSet<KeyCode>(hotkey.KeyCodes);
        var set2 = new HashSet<KeyCode>(keyCodes);
        return set1.Equals(set2);
    }
    public static void ActivateKey(this IHotkey hotkey, KeyCode key, IList<KeyCode> pressedCodes)
    {
        var index = hotkey.KeyCodes.IndexOf(key);
        if (index == -1) return;
        if(hotkey.IsActivated[index]) return;
        
        hotkey.ActivatedKeys++;
        hotkey.IsActivated[index] = true;
        
        if (hotkey.ActivatedKeys != hotkey.KeyCodes.Count) return;
        if (hotkey.KeyCodes.Count != pressedCodes.Count) return;
        hotkey.OnHotkey();
    }
    
    public static void DeactivateKey(this IHotkey hotkey, KeyCode key)
    {
        var index = hotkey.KeyCodes.IndexOf(key);
        if (index == -1) return;
        if(!hotkey.IsActivated[index]) return;
        
        hotkey.ActivatedKeys--;
        hotkey.IsActivated[index] = false;
    }
}