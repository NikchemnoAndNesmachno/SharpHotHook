# SharpHotHook

A simple library for handling global hotkey combinations. It utilizes the [SharpHook](https://github.com/TolikPylypchuk/SharpHook) library, which allows global key detection. This library simplifies the process by combining key detection and triggering events.

---

## How to Use

The following classes are used for operation:

- **HotkeyManager**: The main class that holds a list of hotkeys and activates them.
- **KeyReaderManager**: A standalone class that simply reads the current key.
- **IHotkey**: An interface defining the structure of a hotkey. Its main features include a list of keys and a method that gets called upon activation.

---

## Basic Hotkey Usage

You can use the predefined `Hotkey` class from `SharpHotHook.Defaults`, which implements `IHotkey`. The `HotkeyManager` class is used to manage and trigger hotkeys.

```csharp
static void Example1()
{
    var manager = new HotkeyManager();

    // Initializing a hotkey via properties
    manager.Add(new Hotkey()
    {
        KeyCodes = [KeyCode.Vc1],
        OnHotkey = () => Console.WriteLine("\n\tKey vc1 pressed")
    });

    // Initializing a hotkey via constructor
    manager.Add(new Hotkey(
        keyCodes: [KeyCode.Vc1, KeyCode.VcOpenBracket],
        action: () => Console.WriteLine("\n\tKey 1 + [ pressed")
    ));

    // Use Stop to stop the manager, Start to run it
    manager.Add(new Hotkey()
    {
        KeyCodes = [KeyCode.VcEscape],
        OnHotkey = manager.Stop
    });

    manager.Start();
}

static void Main(string[] args)
{
    Example1();
    Console.WriteLine("\nWaiting for keys");

    // Due to the inner workings of SharpHook's reader, the program will not terminate and will keep listening for key presses.
}
```

Console output (symbols entered on the left, output displayed on the right):

```console
Waiting for keys
1
Key vc1 pressed
Key vc1 pressed
1
Key vc1 pressed
13
Key vc1 pressed
1ххх
Key vc1 pressed
1
Key 1 + [ pressed
х
Key vc1 pressed
1
Key 1 + [ pressed
[
хх
Key 1 + [ pressed
1х
Key 1 + [ pressed
1[[Key 1 + [ pressed
1
Key vc1 pressed
1
Key 1 + [ pressed
[
```

---

## Key-Only Reading

This feature can be useful if you only want to monitor which keys are being pressed. The `KeyReaderManager` class is designed for this purpose. Here’s an example:

```csharp
class KeyReader : KeyReaderManager
{
    private KeyCode _key;
    public Action OnKeyChanged { get; set; } = Console.WriteLine;

    public override KeyCode CurrentKey
    {
        get => _key;
        set
        {
            _key = value;
            if (_key == KeyCode.VcEscape)
            {
                Stop();
                return;
            }

            Console.Write("Key is changed on: ");
            OnKeyChanged();
        }
    }
}

static void Example2()
{
    var manager = new KeyReader();
    manager.OnKeyChanged = () => Console.WriteLine(manager.CurrentKey);
    manager.Start();
}
```

---

## Creating Custom Classes

The library is flexible. You can inherit from `KeyReaderBase` and its inherits, or implement `IKeyReader`, `IHotkeyContainer`, and `IHotkey`. The `HotkeyMethods` extension methods are available for working with `IHotkey`.

When implementing `IHotkey`, be sure to handle the `IsActivated` boolean array. Its size must match the size of the key list.

---

# License

Note: The use of the library code, the library itself, and the texts of the README are subject to the NIN license (available in the project's repository). Acceptance of the license is mandatory.