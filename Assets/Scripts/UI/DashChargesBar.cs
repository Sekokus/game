using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DashChargesBar : WombImageResourceBar
    {
        [SerializeField] private Color rechargingColor = Color.gray;

        protected override Resource GetResource(PlayerCore player)
        {
            return player.Resources.DashCharges;
        }

        protected override void OnWombFillChanged(float fillAmount, int wombIndex, IReadOnlyList<Image> wombImages)
        {
            wombImages[wombIndex].color = Mathf.Approximately(fillAmount, 1) ? Color.white : rechargingColor;
        }
    }
}