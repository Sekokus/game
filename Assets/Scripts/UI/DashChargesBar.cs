using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DashChargesBar : WombImageResourceBar
    {
        protected override CharacterResource GetResource(PlayerController player)
        {
            return player.DashResource;
        }

        protected override void OnWombFillChanged(float fillAmount, int wombIndex, IReadOnlyList<Image> wombImages)
        {
            if (!Mathf.Approximately(fillAmount, 1))
            {
                return;
            }

            // TODO: сделать эффект чтобы классно
        }
    }
}