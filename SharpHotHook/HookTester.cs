using SharpHook;
using SharpHook.Native;

namespace SharpHotHook;

public class HookTester
{
    private TaskPoolGlobalHook? hook;
    public virtual ICollection<KeyCode> Codes { get; set; } = [];
    public virtual ICollection<Hotkey> Hotkeys { get; set; } = [];
    public virtual bool IsPaused { get; set; } = true;

    public void AddHotkey(KeyCode[] keyCodes, Action onHotkey)
    {
        Hotkeys.Add(new(onHotkey, keyCodes));
    }
    
    public async void Run()
    {
        hook = new TaskPoolGlobalHook(globalHookType:GlobalHookType.Keyboard);
        hook.KeyPressed += OnKeyPressed;     
        hook.KeyReleased += OnKeyReleased;  
        IsPaused = false;
        await hook.RunAsync();
    }

    public void Stop()
    {
        hook?.Dispose();
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

    protected virtual void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        if(IsPaused) return;
        Codes.Remove(e.Data.KeyCode);
        DeactivateKey(e.Data.KeyCode);
    }

    protected virtual void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if (IsPaused) return;
        if(Codes.Contains(e.Data.KeyCode)) return;
        Codes.Add(e.Data.KeyCode);
        ActivateKey(e.Data.KeyCode);
    }
}