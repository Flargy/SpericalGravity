using UnityEngine;

public class PlanetShift : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerMovement>().ChangePlanet(gameObject);
    }
}
