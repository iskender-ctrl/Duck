using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// destroys the particle system game object whenever it's finished
    /// </summary>
    public class DestroyParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem vfx;

        void Update()
        {
            if (vfx.isStopped)
            {
                Destroy(gameObject);
            
            }
        }
    }
}
