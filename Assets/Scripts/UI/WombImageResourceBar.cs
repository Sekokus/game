using System.Collections.Generic;
using Sekokus.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class WombImageResourceBar : ResourceBar
    {
        [SerializeField] private Image[] wombs;

        protected override void OnValueChanged(Resource resource)
        {
            var value = resource.Value;

            var fullyFilledWombs = Mathf.FloorToInt(value);
            var remainedWombFill = value - fullyFilledWombs;

            var wombCount = wombs.Length;

            int i = 0;
            for (; i < Mathf.Min(wombCount, fullyFilledWombs); i++)
            {
                SetWombFillAmount(1, i);
            }

            if (wombCount > i)
            {
                SetWombFillAmount(remainedWombFill, i++);
            }

            for (; i < wombCount; i++)
            {
                SetWombFillAmount(0, i);
            }
        }

        private void SetWombFillAmount(float fillAmount, int wombIndex)
        {
            var womb = wombs[wombIndex];
            if (Mathf.Approximately(fillAmount, womb.fillAmount))
            {
                return;
            }

            womb.fillAmount = fillAmount;
            OnWombFillChanged(fillAmount, wombIndex, wombs);
        }

        protected virtual void OnWombFillChanged(float fillAmount, int wombIndex, IReadOnlyList<Image> wombImages)
        {
        }
    }
}