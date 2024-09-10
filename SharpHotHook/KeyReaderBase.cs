using SharpHook;
using SharpHook.Native;

namespace SharpHotHook;

public class KeyReaderBase: IDisposable
{
    private TaskPoolGlobalHook? _hook;
    public IList<KeyCode> PressedKeys { get; set; } = [];
    public void Dispose()
    {
        _hook?.Dispose();
        //Added this line only because of the IDE's hint. I am not aware about importance of it.
        GC.SuppressFinalize(this);
    }

    protected virtual void OnKeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        PressedKeys.Remove(e.Data.KeyCode);
    }

    protected virtual bool OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if(PressedKeys.Contains(e.Data.KeyCode)) return false;
        PressedKeys.Add(e.Data.KeyCode);
        return true;
    }
    
    public virtual async void Run()
    {
        _hook = new TaskPoolGlobalHook(globalHookType: GlobalHookType.Keyboard);
        _hook.KeyPressed += (o, e)=> (o as KeyReaderBase)?.OnKeyPressed(o, e);     
        _hook.KeyReleased += (o, e)=>(o as KeyReaderBase)?.OnKeyReleased(o,e);  
        await _hook.RunAsync();
    }

    public virtual async void Stop()
    {
        _hook?.Dispose();
    }
}