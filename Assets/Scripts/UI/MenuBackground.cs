using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class MenuBackground : MonoBehaviour
    {
        [SerializeField] private float cursorFollowWeight;
        [SerializeField] private Vector2 maxOffset;
        [SerializeField] private RectTransform rectTransform;

        private Vector2 _initialPosition;

        private void Awake()
        {
            _initialPosition = rectTransform.localPosition;
        }

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }

            var position = mouse.position.ReadValue();
            var centeredCursorPosition = new Vector2(position.x - Screen.width / 2f, position.y - Screen.height / 2f);
            var offset = (centeredCursorPosition - _initialPosition) * cursorFollowWeight;
            offset.x = Mathf.Clamp(offset.x, -maxOffset.x, maxOffset.x);
            offset.y = Mathf.Clamp(offset.y, -maxOffset.y, maxOffset.y);
            rectTransform.localPosition = _initialPosition + offset;
        }
    }
}