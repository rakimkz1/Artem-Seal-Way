using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WaterZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && other.gameObject.GetComponent<ISwimable>() != null)
            other.gameObject.GetComponent<ISwimable>().EnterWater(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != null && other.gameObject.GetComponent<ISwimable>() != null)
            other.gameObject.GetComponent<ISwimable>().ExitWater(this);
    }
}