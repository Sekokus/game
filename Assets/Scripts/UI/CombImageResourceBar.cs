using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class CombImageResourceBar : ResourceBar
    {
        [SerializeField] private Image[] combs;

        protected override void OnValueChanged(Resource resource)
        {
            var value = resource.Value;

            var fullyFilledCombs = Mathf.FloorToInt(value);
            var remainedCombFill = value - fullyFilledCombs;

            var combCount = combs.Length;

            var i = 0;
            for (; i < Mathf.Min(combCount, fullyFilledCombs); i++)
            {
                SetCombFillAmount(1, i);
            }

            if (combCount > i)
            {
                SetCombFillAmount(remainedCombFill, i++);
            }

            for (; i < combCount; i++)
            {
                SetCombFillAmount(0, i);
            }
        }

        private void SetCombFillAmount(float fillAmount, int combIndex)
        {
            var comb = combs[combIndex];
            if (Mathf.Approximately(fillAmount, comb.fillAmount))
            {
                return;
            }

            comb.fillAmount = fillAmount;
            OnCombFillChanged(fillAmount, combIndex, combs);
        }

        protected virtual void OnCombFillChanged(float fillAmount, int combIndex, IReadOnlyList<Image> combImages)
        {
        }
    }
}