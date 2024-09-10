using SharpHook;
using SharpHook.Native;
using SharpHotHook.Defaults;

namespace SharpHotHook;

public class KeyReaderManager: KeyReaderBase
{
    public IKeyReadContainer KeyReadContainer { get; set; } = new KeyContainerDefault();

    public KeyCode CurrentKey
    {
        get => KeyReadContainer.CurrentKey;
        set => KeyReadContainer.CurrentKey = value;
    }
    protected override bool OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if (!base.OnKeyPressed(sender, e)) return false;
        CurrentKey = e.Data.KeyCode;
        return true;

    }
}