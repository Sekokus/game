using TMPro;
using UnityEngine;

namespace UI
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;

        private void Awake()
        {
            textMesh.text = "v" + Application.version;
        }
    }
}