using SharpHook;
using SharpHook.Native;

namespace SharpHotHook;

public class HotkeyManager: IDisposable
{
    private TaskPoolGlobalHook? _hook;
    private int _pauseKeysAmount = 0;
    public virtual ICollection<KeyCode> Codes { get; set; } = [];
    public KeyCode[] PauseHotkey { get; set; } = [];
    public virtual ICollection<Hotkey> Hotkeys { get; set; } = [];
    public virtual bool IsPaused { get; set; } = true;

    public void AddHotkey(KeyCode[] keyCodes, Action onHotkey) => Hotkeys.Add(new(onHotkey, keyCodes));
    public void AddHotkey(Hotkey hotkey) => Hotkeys.Add(hotkey);

    public void AddStopHotkey(KeyCode[] keyCodes) =>
        Hotkeys.Add(new(Stop, keyCodes));

    public void AddPauseHotkey(KeyCode[] keyCodes)
    {
        PauseHotkey = keyCodes;
        _pauseKeysAmount = 0;
    }

    public async void Run()
    {
        _hook = new TaskPoolGlobalHook(globalHookType:GlobalHookType.Keyboard);
        _hook.KeyPressed += OnKeyPressed;     
        _hook.KeyReleased += OnKeyReleased;  
        IsPaused = false;
        _pauseKeysAmount = 0;
        await _hook.RunAsync();
    }

    public void Stop()
    {
        _hook?.Dispose();
        IsPaused = true;
    }

    public void Pause()
    {
        IsPaused = true;
    }

    private void ActivateKey(KeyCode key) =>
        Task.Run(() =>
        {
            foreach (var hotkey in Hotkeys)
            {
                hotkey.ActivateKey(key);
            }
        });
    
    private void DeactivateKey(KeyCode key) =>
        Task.Run(() =>
        {
            foreach (var hotkey in Hotkeys)
            {
                hotkey.DeactivateKey(key);
            }
        });
    private bool IsPauseActivated()
    {
        var set1 = new HashSet<KeyCode>(Codes);
        var set2 = new HashSet<KeyCode>(PauseHotkey);
        return set1.SetEquals(set2);
    }
    protected virtual void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        Codes.Remove(e.Data.KeyCode);
        if(IsPaused) return;
        DeactivateKey(e.Data.KeyCode);
    }
   
    protected virtual void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if(Codes.Contains(e.Data.KeyCode)) return;
        Codes.Add(e.Data.KeyCode);
        if (IsPauseActivated())
        {
            IsPaused = !IsPaused;
            Console.WriteLine("Paused: " + IsPaused);
        }

        if (IsPaused) return;
        ActivateKey(e.Data.KeyCode);
    }

    public void Dispose()
    {
        _hook?.Dispose();
        
        //Added this line only because of the IDE's hint. I am not aware about importance of it.
        GC.SuppressFinalize(this);
    }
}