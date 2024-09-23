using SharpHook;
using SharpHook.Native;
using SharpHotHook.Interfaces;

namespace SharpHotHook;
public class HotkeyManager: KeyReaderBase, IHotkeyContainer
{
    public virtual IList<IHotkey> Hotkeys { get; set; } = [];
    public override async void Start()
    {
        await ResetKeys();
        base.Start();
    }
    

    private Task ResetKeys() =>
        Task.Run(() =>
        {
            foreach (var hotkey in Hotkeys)
            {
                hotkey.Reset();
            }
        });

    private void ActivateKey(KeyCode key) =>
        Task.Run(() =>
        {
            foreach (var hotkey in Hotkeys)
            {
                hotkey.ActivateKey(key, PressedKeys);
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
    
    public override void OnKeyReleased(KeyboardHookEventArgs e)
    {
        base.OnKeyReleased(e);
        DeactivateKey(e.Data.KeyCode);
    }
   
    public override bool OnKeyPressed(KeyboardHookEventArgs e)
    {
        if (!base.OnKeyPressed(e)) return false;
        ActivateKey(e.Data.KeyCode);    
        return true;
    }
    
    public void Add(IHotkey hotkey) => Hotkeys.Add(hotkey);

    public void Remove(IList<KeyCode> codes)
    {
        for (var i = 0; i < Hotkeys.Count; i++)
        {
            if (!Hotkeys[i].Equal(codes)) continue;
            Hotkeys.RemoveAt(i);
            return;
        }
    }
}