using SharpHook.Native;

namespace SharpHotHook;

public interface IKeyReadContainer
{ 
    KeyCode CurrentKey { get; set; }
}