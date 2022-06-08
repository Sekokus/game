using UnityEngine;

namespace Enemies
{
    public class DestructionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject destroyedObjectPrefab;
        [SerializeField] private float destroyTempObjectAfter = 3;

        public void Destroy()
        {
            ImitateDestruction();
            Destroy(gameObject);
        }

        public void ImitateDestruction()
        {
            if (!destroyedObjectPrefab)
            {
                return;
            }

            var obj = Instantiate(destroyedObjectPrefab, transform.position, transform.rotation);
            obj.SetActive(true);
            Destroy(obj, destroyTempObjectAfter);
        }
    }
}