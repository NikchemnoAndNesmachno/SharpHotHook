using SharpHook.Native;

namespace SharpHotHook;

public class Hotkey
{
    public Hotkey()
    {
    }
    
    public Hotkey(Action onHotkey, KeyCode[] keyCodes)
    {
        _keyCodes = keyCodes;
        OnHotkey = onHotkey;
        Reset();
    }
    
    private int _activatedKeys = 0;
    private bool[] _isActivated = [];
    private KeyCode[] _keyCodes = [];

    public void Reset()
    {
        _isActivated = Enumerable.Repeat(false, KeyCodes.Length).ToArray();
        _activatedKeys = 0;
    }
    public KeyCode[] KeyCodes
    {
        get => _keyCodes;
        set
        {
            _keyCodes = value;
            Reset();
        }
    } 
    public Action OnHotkey { get; set; } = () => { };

    public void ActivateKey(KeyCode key)
    {
        var index = Array.IndexOf(KeyCodes, key);
        if (index == -1) return;
        if(_isActivated[index]) return;
        
        _activatedKeys++;
        _isActivated[index] = true;
        
        if (_activatedKeys != KeyCodes.Length) return;
        OnHotkey();
    }
    
    public void DeactivateKey(KeyCode key)
    {
        var index = Array.IndexOf(KeyCodes, key);
        if (index == -1) return;
        if(!_isActivated[index]) return;
        
        _activatedKeys--;
        _isActivated[index] = false;
    }
}