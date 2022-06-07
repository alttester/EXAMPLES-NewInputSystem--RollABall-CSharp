using UnityEngine;

// Include the namespace required to use Unity UI and Input System
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Create public variables for player speed, and for the Text UI game objects
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject; 
    public Vector3 previousAcceleration = Vector3.zero;

    private Rigidbody ballRigidBody;
    private int count;
    private float movementX;
    private float movementY;
    private float movementZ;
        
    // Start is called before the first frame update
    void Start()
    {
        // Assign the Rigidbody component to our privated variable
        ballRigidBody = GetComponent<Rigidbody>();

        // Set the count to zero
        count = 0;

        SetCountText();
        
        // Set the text property of the Win Text UI to an empty string, making the 'You Win' (game over message) blank
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
        UnityEngine.Debug.Log("I m in move");
    }

    void OnLook(InputValue lookValue)
    {
        Vector2 lookVector = lookValue.Get<Vector2>();

        movementX = lookVector.x;
        movementY = lookVector.y;
    }

    // void OnTilt(InputValue accelerationValue)
    // {
    //     Vector3 movementVector = accelerationValue.Get<Vector3>();

    //     movementX = movementVector.x;
    //     movementY = movementVector.y;
    //     movementZ = movementVector.z;

    //     Debug.Log(movementVector);
    // }

    void OnEnable()
    {
        InputSystem.EnableDevice(Accelerometer.current);        
    }

    void OnDisable()
    {
        InputSystem.DisableDevice(Accelerometer.current);        
    }
    
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            // Set the text value of your 'winText'
            winTextObject.SetActive(true);
        }
    }

    void Update()
    {
        var acceleration = Accelerometer.current.acceleration.ReadValue();
        if (acceleration != previousAcceleration)
        {
            Debug.Log(acceleration);
            previousAcceleration = acceleration;
            ballRigidBody.AddForce(acceleration);
        }
    }

    void FixedUpdate()
    {
        // Create a Vector3 variable, and assign X and Z to feature the horizontal and vertical float variables above
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        
        ballRigidBody.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ..and if the GameObject you intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            // Add one to the score variable 'count'
            count++;

            // Run the 'SetCountText()' function (see below)
            SetCountText();
        }
    }
}
