using UnityEngine;

namespace Enemies
{
    public class DestructionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject destroyedObjectPrefab;
        [SerializeField] private float destroyTempObjectAfter = 3;

        [SerializeField] private GameObject destroyObjectKotoriyNeDestroyCherezTempVremya;

        public void Destroy()
        {
            ImitateDestruction();
            Destroy(gameObject);
        }

        public void ImitateDestruction()
        {
            if (destroyedObjectPrefab)
            {
                var obj = Instantiate(destroyedObjectPrefab, transform.position, transform.rotation);
                obj.SetActive(true);
                Destroy(obj, destroyTempObjectAfter);
            }

            if (destroyObjectKotoriyNeDestroyCherezTempVremya)
            {
                var obj = Instantiate(destroyObjectKotoriyNeDestroyCherezTempVremya, transform.position,
                    transform.rotation);
                obj.SetActive(true);
            }
        }
    }
}