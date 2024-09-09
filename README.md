# SharpHotHook
Проста бібліотека для зчитування комбінацій глобальних клавіш, що використовує бібліотеку \
[SharpHook](https://github.com/TolikPylypchuk/SharpHook) \
Nuget [тут](https://www.nuget.org/packages/SharpHotHook).

## Використання
Зчитує клавіші HotkeyManager. \
Перед цим додайте по черзі гарячі клавіші і метод Action, що буде виконуватися після натискання комбінацій. \

```csharp
var hook = new HotkeyManager(); // create hotkey reader
hook.AddHotkey([KeyCode.VcLeftControl, KeyCode.Vc1], () =>
{
Console.WriteLine("Pressed v1");
}); // add first hotkey
hook.AddHotkey([KeyCode.VcLeftAlt, KeyCode.Vc5], () =>
{
Console.WriteLine("Pressed alt 5");
}); // add second one
hook.AddHotkey([KeyCode.VcLeftControl, KeyCode.Vc2], () =>
{
Console.WriteLine("Pressed v2");
}); // another one
hook.AddPauseHotkey([KeyCode.VcLeftControl, KeyCode.Vc0]); // add pause 
hook.Run(); // launch
```
### Трохи ще
- ``KeyCode`` береться з бібліотеки [SharpHook](https://sharphook.tolik.io/v5.3.7/articles/keycodes.html). Детальніше там же.
- Комбінація зчитується в будь-якому порядку. Наприклад, ``ctrl+c`` = ``c+ctrl``
- При паузі клавіші все ще зчитуються (щоб можна було зняти паузу), але гарячі клавіші не працюють
- При запуску внутрішній зчитувач клавіш ``TaskPoolGlobalHook`` перестворюється.
- Код не виконується далі ``hook.Run()``. Щоб зупинити, викоритовуйте метод ``Stop`` з іншого потоку або додайте через метод ``AddStopHotkey`` комбінацію клавіш для завершення
- Можливо, потім додам парсер для перекладу з ``string`` в ``KeyCode``
  
### Про nuget
0.1 та 0.2 це пакети для різних гілок. 0.1 це вже робоча версія, тоді як 0.2 дозволяє визначати контейнери ``IHotkey`` та ``IHotkeyContainer``

