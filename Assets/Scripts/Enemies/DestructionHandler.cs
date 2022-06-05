using UnityEngine;

namespace Enemies
{
    public class DestructionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject destroyedObjectPrefab;
        
        public void Destroy()
        {
            if (destroyedObjectPrefab)
            {
                Instantiate(destroyedObjectPrefab, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}