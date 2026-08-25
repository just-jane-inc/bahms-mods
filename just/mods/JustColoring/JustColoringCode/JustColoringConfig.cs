using BaseLib.Config;
using Godot;

namespace JustColoring.JustColoringCode
{
    // sets up the local configuration to provide the default value
    // override for a players color
    internal class JustColorsConfig : SimpleModConfig
    {
        private static string _drawingColor = "#deadffff";

        [ConfigSection("Drawing Color Settings")]
        [ConfigColorPicker]
        public static string DrawingColorThing
        {
            get => _drawingColor;
            set
            {
                if (value == _drawingColor)
                    return;

                Color color = Color.FromString(value, default);
                _drawingColor = value;
                MapDrawingColorPatch.UserColorMap[1] = color;
            }
        }

        public static void UpdateColor(Color color)
        {
            DrawingColorThing = color.ToHtml();
            NetworkIO.SendConfigurationUpdate();
        }

        public static Color ConfiguredColor
        {
            get => Color.FromString(DrawingColorThing, Color.Color8(0xDE, 0xAD, 0xFF, 0xFF));
        }
    }
}
