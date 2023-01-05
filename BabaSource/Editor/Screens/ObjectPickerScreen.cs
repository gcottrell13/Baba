using Core.Content;
using Core.Screens;
using Core.Utils;

namespace Editor.Screens
{
    internal class ObjectPickerScreen : FiltererModal<ObjectInfoItem>
    {
        public ObjectPickerScreen(string? current) : base(
            ObjectInfo.Info.Values, 
            currentValue: current != null && ObjectInfo.Info.TryGetValue(current, out var v) ? v : null,
            maxDisplay: 15, 
            filterBy: o => o.sprite, 
            display: o => $"{ObjectDefaultSprite(o.sprite)} {o.sprite}")
        {

        }

        public static string ObjectDefaultSprite(string name) => $"{ThemeInfo.GetObjectColor("default", name).ToHexTriple()}[{name}]";
    }
}
