using UnityEngine;

public class SlideSurface : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != null && other.gameObject.TryGetComponent(out ISlidables slidables))
        {
            slidables.OnSlideSurfaceEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject != null && other.gameObject.TryGetComponent(out ISlidables slidables))
        {
            slidables.OnSlideSurfaceExit();
        }
    }
}
