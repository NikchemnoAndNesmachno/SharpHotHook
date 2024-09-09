using SharpHook;
using SharpHook.Native;

namespace SharpHotHook;
public class HotkeyManager: IDisposable
{
    
    private TaskPoolGlobalHook? _hook;
    private int _pauseKeysAmount = 0;
    public IHotkeyContainer HotkeyContainer { get; set; } = new HotkeyContainerDefault();

    public void AddHotkey(KeyCode[] keyCodes, Action onHotkey) =>
        HotkeyContainer.Add(keyCodes, onHotkey);
    public void AddStopHotkey(KeyCode[] keyCodes) =>
        HotkeyContainer.Add(keyCodes, Stop);

    public void AddPauseHotkey(KeyCode[] keyCodes)
    {
        HotkeyContainer.PauseHotkey = keyCodes;
        _pauseKeysAmount = 0;
    }

    public async void Run()
    {
        _hook = new TaskPoolGlobalHook(globalHookType: GlobalHookType.Keyboard);
        _hook.KeyPressed += OnKeyPressed;     
        _hook.KeyReleased += OnKeyReleased;  
        HotkeyContainer.IsPaused = false;
        _pauseKeysAmount = 0;
        await ResetKeys();
        await _hook.RunAsync();
    }

    public void Stop()
    {
        _hook?.Dispose();
        HotkeyContainer.IsPaused = true;
    }

    public void Pause()
    {
        HotkeyContainer.IsPaused = true;
    }
    
    private Task ResetKeys() =>
        Task.Run(() =>
        {
            foreach (var hotkey in HotkeyContainer.Hotkeys)
            {
                hotkey.Reset();
            }
        });

    private void ActivateKey(KeyCode key) =>
        Task.Run(() =>
        {
            foreach (var hotkey in HotkeyContainer.Hotkeys)
            {
                hotkey.ActivateKey(key);
            }
        });
    
    
    private void DeactivateKey(KeyCode key) =>
        Task.Run(() =>
        {
            foreach (var hotkey in HotkeyContainer.Hotkeys)
            {
                hotkey.DeactivateKey(key);
            }
        });
    private bool IsPauseActivated()
    {
        var set1 = new HashSet<KeyCode>(HotkeyContainer.Codes);
        var set2 = new HashSet<KeyCode>(HotkeyContainer.PauseHotkey);
        return set1.SetEquals(set2);
    }
    private void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        HotkeyContainer.Codes.Remove(e.Data.KeyCode);
        if(HotkeyContainer.IsPaused) return;
        DeactivateKey(e.Data.KeyCode);
    }
   
    private void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if(HotkeyContainer.Codes.Contains(e.Data.KeyCode)) return;
        HotkeyContainer.Codes.Add(e.Data.KeyCode);
        if (IsPauseActivated())
        {
            HotkeyContainer.IsPaused = !HotkeyContainer.IsPaused;
        }

        if (HotkeyContainer.IsPaused) return;
        ActivateKey(e.Data.KeyCode);
    }

    public void Dispose()
    {
        _hook?.Dispose();
        
        //Added this line only because of the IDE's hint. I am not aware about importance of it.
        GC.SuppressFinalize(this);
    }
}