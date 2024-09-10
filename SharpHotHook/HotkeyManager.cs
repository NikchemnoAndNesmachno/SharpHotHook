using SharpHook;
using SharpHook.Native;
using SharpHotHook.Defaults;

namespace SharpHotHook;
public class HotkeyManager: KeyReaderBase
{
    public IHotkeyContainer HotkeyContainer { get; set; } = new HotkeyContainerDefault();

    public void AddHotkey(KeyCode[] keyCodes, Action onHotkey) =>
        HotkeyContainer.Add(keyCodes, onHotkey);
    public void AddStopHotkey(KeyCode[] keyCodes) =>
        HotkeyContainer.Add(keyCodes, Stop);

    public void AddPauseHotkey(KeyCode[] keyCodes) => 
        HotkeyContainer.PauseHotkey = keyCodes;

    public override async void Run()
    {
        HotkeyContainer.IsPaused = false;
        await ResetKeys();
        base.Run();
    }

    public override void Stop()
    {
        base.Stop();
        HotkeyContainer.IsPaused = true;
    }

    public void Pause() => HotkeyContainer.IsPaused = true;

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
        var set1 = new HashSet<KeyCode>(PressedKeys);
        var set2 = new HashSet<KeyCode>(HotkeyContainer.PauseHotkey);
        return set1.SetEquals(set2);
    }
    protected override void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        base.OnKeyReleased(sender, e);
        if(HotkeyContainer.IsPaused) return;
        DeactivateKey(e.Data.KeyCode);
    }
   
    protected override bool OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if (!base.OnKeyPressed(sender, e)) return false;
        if (IsPauseActivated())
        {
            HotkeyContainer.IsPaused = !HotkeyContainer.IsPaused;
        }

        if (!HotkeyContainer.IsPaused)
        {
            ActivateKey(e.Data.KeyCode);    
        }
        
        return true;
    }
}