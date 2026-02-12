using UnityEngine;

public class SlideSurface : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != null && other.gameObject.TryGetComponent(out ISlidables slidables))
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
