using SharpHook;

namespace SharpHotHook.Interfaces;

public interface IKeyReader
{
    void OnKeyReleased(KeyboardHookEventArgs e);

    bool OnKeyPressed(KeyboardHookEventArgs e);
    void Start();
    void Stop();
}