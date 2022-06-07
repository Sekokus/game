using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(MultiImageButtonGraphics))]
    public class MultiImageButton : Button
    {
        [SerializeField, HideInInspector] private MultiImageButtonGraphics graphics;

        protected override void OnValidate()
        {
            base.OnValidate();

            graphics = GetComponent<MultiImageButtonGraphics>();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            var targetColor = state switch
            {
                SelectionState.Disabled => colors.disabledColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Normal => colors.normalColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Selected => colors.selectedColor,
                _ => Color.white
            };

            foreach (var graphic in graphics.Graphics)
            {
                graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }
    }
}