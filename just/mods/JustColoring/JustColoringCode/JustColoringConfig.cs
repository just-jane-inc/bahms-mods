using BaseLib.Config;
using Godot;

namespace JustColoring.JustColoringCode
{
    // sets up the local configuration to provide the default value
    // override for a players color
    internal class JustColorsConfig : SimpleModConfig
    {
        private static Color _drawingColor = Color.Color8(0xDE, 0xAD, 0xFF);

        [ConfigSection("Drawing Color Settings")]
        [ConfigColorPicker]
        public static Color DrawingColorThing
        {
            get => _drawingColor;
            set
            {
                if (value == _drawingColor)
                    return;

                _drawingColor = value;
                MapDrawingColorPatch.UserColorMap[1] = value;
            }
        }
    }
}
