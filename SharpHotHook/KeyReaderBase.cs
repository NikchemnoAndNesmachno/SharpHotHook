using SharpHook;
using SharpHook.Native;
using SharpHotHook.Interfaces;

namespace SharpHotHook;

public class KeyReaderBase: IDisposable, IKeyReader
{
    protected SimpleGlobalHook? Hook;
    public IList<KeyCode> PressedKeys { get; set; } = [];
    public void Dispose()
    {
        Stop();
        //Added this line only because of the IDE's hint. I am not aware about importance of it.
        GC.SuppressFinalize(this);
    }

    public virtual void OnKeyReleased(KeyboardHookEventArgs e)
    {
        PressedKeys.Remove(e.Data.KeyCode);
    }

    public virtual bool OnKeyPressed(KeyboardHookEventArgs e)
    {
        if(PressedKeys.Contains(e.Data.KeyCode)) return false;
        PressedKeys.Add(e.Data.KeyCode);
        return true;
    }

    private void OnKeyPressed(object? o, KeyboardHookEventArgs e) => OnKeyPressed(e);
    private void OnKeyReleased(object? o, KeyboardHookEventArgs e) => OnKeyReleased(e);
    public async virtual void Start()
    {
        Hook = new SimpleGlobalHook(globalHookType: GlobalHookType.Keyboard);
        Hook.KeyPressed += OnKeyPressed;
        Hook.KeyReleased += OnKeyReleased;  
        await Hook.RunAsync();
    }

    public virtual void Stop()
    {
        if(Hook is null || Hook.IsDisposed) return; 
        Hook.KeyPressed -= OnKeyPressed;
        Hook.KeyReleased -= OnKeyReleased;
        Hook.Dispose();
    }
}