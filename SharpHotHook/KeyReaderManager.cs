using SharpHook;
using SharpHook.Native;

namespace SharpHotHook;

public class KeyReaderManager: KeyReaderBase
{
    public virtual KeyCode CurrentKey { get; set; } = KeyCode.VcUndefined;

    public override bool OnKeyPressed(KeyboardHookEventArgs e)
    {
        if (!base.OnKeyPressed(e)) return false;
        CurrentKey = e.Data.KeyCode;
        return true;

    }
}