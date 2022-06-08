using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DashChargesBar : CombImageResourceBar
    {
        [SerializeField] private Color rechargingColor = Color.gray;

        protected override Resource GetResource(PlayerCore player)
        {
            return player.Resources.DashCharges;
        }

        protected override void OnCombFillChanged(float fillAmount, int combIndex, IReadOnlyList<Image> combImages)
        {
            combImages[combIndex].color = Mathf.Approximately(fillAmount, 1) ? Color.white : rechargingColor;
        }
    }
}