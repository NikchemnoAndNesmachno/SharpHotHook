# SharpHotHook

Проста бібліотека для зчитування комбінацій глобальних клавіш.
Використовує бібліотеку [SharpHook](https://github.com/TolikPylypchuk/SharpHook), яка дозволяє зчитувати клавіші глобально.
Дана бібліотека просто дозволяє об'єднати зчитування і викликати подію.

___

# Як використовувати

Для роботи використовуються такі класи:
 - HotkeyManager - основний клас, що містить список гарячих клавіш та активує їх
 - KeyReaderManager - окремий клас, що просто зчитує поточну клавішу
 - IHotkey - інтерфейс структури гарячої клавіші. Головне, що містить - список клавіш та метод, що має викликатися.

---

## Звичайне використання гарячих клавіш

Можна використати готовий клас `Hotkey` з `SharpHotHook.Defaults`, що реалізує `IHotkey`.
Для запуску використовується клас HotkeyManager.

```c#
static void Example1()
{
    var manager = new HotkeyManager();
    
    // ініціалізація клавіші через властивості
    manager.Add(new Hotkey()
    {
        KeyCodes = [KeyCode.Vc1],
        OnHotkey = () => Console.WriteLine("\n\tKey vc1 pressed")
    } );
    
    // ініціалізація через конструктор
    manager.Add(new Hotkey(
        keyCodes: [KeyCode.Vc1, KeyCode.VcOpenBracket],
        action: () => Console.WriteLine("\n\tKey 1 + [ pressed")));
    
    // для зупинки використовуєтьяс Stop, для запуску Start
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
    //Через внутрішню будову зчитувача в SharpHook програма не вимкнеться і буде зчитувати клавіші
}
```
Вивід у консолі (зліва введені символи, правіше виведення)
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
[хх
        Key 1 + [ pressed
1х
        Key 1 + [ pressed
1[[
        Key 1 + [ pressed
1
        Key vc1 pressed
1
        Key 1 + [ pressed
[

```
## Зчитування лише клавіш

---

Може бути корисно, якщо просто хочеться зчитати, що натискається.
Для цього є `KeyReaderManager`. Приклад використання наступний:

```csharp
class KeyReader: KeyReaderManager
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

## Створення власних класів

---

Бібліотека є гнучкою. Можна спадкувати `KeyReaderBase` і похідні класи, імплементувати `IKeyReader`, 
`IHotkeyContainer` та `IHotkey`. Для роботи з IHotkey використовуються методи розширення `HotkeyMethods`.

При імплементації `IHotkey` потрібно не забути про `IsActivated` - булевий масив, чий розмір має відповідати розміру списку клавіш.

## Ліцензія
Увага, на використання коду бібліотеки, самої бібліотеки, а також текстів Readme
розповсюджується ліцензія NIN (є в репозиторії проєкту). Прийняття є обов'язковим.

