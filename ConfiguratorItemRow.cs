using GObject;

[Subclass<GObject.Object>]
partial class ConfiguratorItemRow
{
    /// <summary>
    /// Назва
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Об'єкт
    /// </summary>
    public object? Obj { get; set; } = null;
}