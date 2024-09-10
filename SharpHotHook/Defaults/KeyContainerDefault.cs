using SharpHook.Native;

namespace SharpHotHook.Defaults;

public class KeyContainerDefault: IKeyReadContainer
{
    public KeyCode CurrentKey { get; set; }
}