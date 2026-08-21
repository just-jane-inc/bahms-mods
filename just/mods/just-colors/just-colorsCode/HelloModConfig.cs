using BaseLib.Config;
using Godot;

namespace JustColors {
    // Enables hover tips for all properties unless the individual property has them disabled
    // by marking them with [ConfigHoverTip(false)]. You still need to add localization strings.
    [ConfigHoverTipsByDefault]
    internal class JustColorsConfig : SimpleModConfig {
        // Starts a new collapsible section with the next property/method. Can be localized the same way as
        // properties. For the example mod, this property's text would be defined as:
        // RANDOMEXPLOSIONS-EXPLOSION_SETTINGS.title
        [ConfigSection("Drawing Color Settings")]

        // Attribute required for string even with default values
        [ConfigColorPicker]
        public static string DrawingColorThing { get; set; } = "#deadffff";

        public static Color ConfiguredColor {
            get => Color.FromString(DrawingColorThing, Color.Color8(0xDE, 0xAD, 0xFF, 0xFF));
        }
    }
}
