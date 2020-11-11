using UnityEditor.Scripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private GameObject activePlanet = null;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private LayerMask groundLayer;

    private float gravity = 9.810f;
    private bool grounded = false;
    private Rigidbody rb;
    private Vector3 movementVector = Vector3.zero;
    private Vector3 groundNormal = Vector3.zero;
    private float distanceToGround = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        //movement
        movementVector.x = Input.GetAxisRaw("Horizontal") * (Time.deltaTime * moveSpeed);
        movementVector.z = Input.GetAxisRaw("Vertical") * (Time.deltaTime * moveSpeed);
        
        transform.Translate(movementVector);
        
        //local rotation, redo this later because it's shit

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 150 * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -150 * Time.deltaTime, 0);
        }
        
        
        
        
        //jump

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * (40000 * jumpHeight * Time.deltaTime));
        }
        
        //Groundcontrol
        GroundCheck();

        Vector3 gravityDirection = (transform.position - activePlanet.transform.position).normalized;
        if (grounded == false)
        {
            rb.AddForce(gravityDirection * -gravity);
        }
        RotateToPlanet();
    }

    private void RotateToPlanet()
    {
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = toRotation;
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10f, groundLayer))
        {
            distanceToGround = hit.distance;
            groundNormal = hit.normal;

            if (distanceToGround <= 0.2f)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
    }

    public void ChangePlanet(GameObject newPlanet)
    {
        activePlanet = newPlanet;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, activePlanet.transform.position - transform.position, out hit, 10f, groundLayer))
        {
            groundNormal = hit.normal;
        }
        RotateToPlanet();
    }
}
