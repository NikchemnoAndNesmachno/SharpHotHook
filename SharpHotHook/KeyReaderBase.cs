using SharpHook;
using SharpHook.Native;
using SharpHotHook.Interfaces;

namespace SharpHotHook;

public class KeyReaderBase: IDisposable, IKeyReader
{
    private TaskPoolGlobalHook? _hook;
    public IList<KeyCode> PressedKeys { get; set; } = [];
    public void Dispose()
    {
        _hook?.Dispose();
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
    
    public virtual async void Start()
    {
        _hook = new TaskPoolGlobalHook(globalHookType: GlobalHookType.Keyboard);
        _hook.KeyPressed += (o, e)=> OnKeyPressed(e);     
        _hook.KeyReleased +=(o, e) => OnKeyReleased(e);  
        await _hook.RunAsync();
    }

    public virtual void Stop()
    {
        _hook?.Dispose();
    }
}