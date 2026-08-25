using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace JustColoring.JustColoringCode
{
    // woof woof 69 nice 67 ehehe  - ty999999
    // patches sts2 NMapDrawings node CreateLineForPlayer method, adds our
    // stuff as a decorator
    [HarmonyPatch(typeof(NMapScreen), nameof(NMapScreen._Ready))]
    internal static class MapColorPickerThing
    {
        private static Button _button = new();

        private const string PalettePath =
            "res://JustColoring/assets/palette-mask.png";

        [HarmonyPostfix]
        private static void AfterMapScreenReady(
            NMapScreen __instance
        )
        {
            var tools = __instance.GetNode<Control>(
                (NodePath)"%DrawingTools"
            );

            foreach (Node node in tools.GetChildren())
            {
                if (node is not HBoxContainer toolbar)
                {
                    continue;
                }

                var existingButton =
                    toolbar.GetChild<NMapDrawButton>(0);

                _button = CreateDrawingColorButton(
                    existingButton
                );

                toolbar.AddChild(_button);

                toolbar.CustomMinimumSize +=
                    new Vector2(90f, 0f);

                break;
            }
        }

        private static Button CreateDrawingColorButton(
            NMapDrawButton existingButton
        )
        {
            var existingIcon =
                existingButton.GetNode<TextureRect>("Icon");

            if (existingIcon.Duplicate() is not TextureRect paletteIcon)
            {
                throw new InvalidOperationException(
                    "Could not duplicate the map drawing button icon"
                );
            }

            paletteIcon.Name = "Icon";
            paletteIcon.Texture =
                PreloadManager.Cache.GetAsset<Texture2D>(
                    PalettePath
                );

            paletteIcon.SelfModulate =
                JustColorsConfig.ConfiguredColor;

            paletteIcon.MouseFilter =
                Control.MouseFilterEnum.Ignore;

            var button = new Button
            {
                Name = "JustColoringButton",
                Text = "",
                Flat = true,
                TooltipText = "Change drawing color",
                Theme = existingButton.Theme,
                ThemeTypeVariation =
                    existingButton.ThemeTypeVariation,
                CustomMinimumSize =
                    existingButton.CustomMinimumSize,
                SizeFlagsHorizontal =
                    existingButton.SizeFlagsHorizontal,
                SizeFlagsVertical =
                    existingButton.SizeFlagsVertical,
                FocusMode =
                    existingButton.FocusMode,
                MouseDefaultCursorShape =
                    existingButton.MouseDefaultCursorShape
            };

            button.AddChild(paletteIcon);

            button.Pressed += () =>
                ShowColorPicker(
                    button,
                    paletteIcon
                );

            return button;
        }

        private static void ShowColorPicker(
            Button button,
            TextureRect paletteIcon
        )
        {
            var popup = new PopupPanel
            {
                Borderless = false
            };

            var picker = new ColorPicker
            {
                Color = JustColorsConfig.ConfiguredColor,
                EditAlpha = false
            };

            picker.ColorChanged += color =>
            {
                paletteIcon.SelfModulate = color;
            };

            popup.AddChild(picker);
            button.GetTree().Root.AddChild(popup);

            popup.PopupHide += () =>
            {
                Color color = picker.Color;

                paletteIcon.SelfModulate = color;
                JustColorsConfig.UpdateColor(color);
                NetworkIO.SendConfigurationUpdate();

                popup.QueueFree();
            };

            popup.PopupOnParent(
                new Rect2I(
                    (int)(button.GlobalPosition.X + button.Size.X),
                    (int)button.GlobalPosition.Y,
                    360,
                    420
                )
            );
        }
    }
}
