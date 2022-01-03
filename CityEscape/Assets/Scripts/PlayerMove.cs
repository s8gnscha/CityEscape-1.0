using UnityEngine;


/**
 * Movement vom Player. Beschränkt sich zunächst nur auf laufen
 */
public class PlayerMove : MonoBehaviour
{
	public CharacterController characterController;

	public Transform target;

	
	private bool isGrounded;
	private bool isWalling;
	[SerializeField] private LayerMask groundLayers;
	[SerializeField] private LayerMask wallLayers;
	
	
	//Position des Spielers
	
	[SerializeField] private float gravity = -9.81f;
	private float jumpHeight = 1f;

	private int line;
	private bool push;
	private Vector3 velocity;

	public Transform groundcheck;

	public Transform wallcheckleft;
	public Transform wallcheckright;
	
	private int rotatecounter;

	

	private float currentTime;


	private float f;
    void Start()
    {
        rotatecounter=0;
	    isWalling = false;
	    push = true;
	    line = 0;
	    currentTime = 1f;
	    characterController= GetComponent<CharacterController>();
			
       }
   
       
       /**
        * Bei fixedupdate wird durch die Velocity function vom rigidbody das automatische laufen vom Spieler generiert.
        * Die Auskommentieren sind weitere Ansätze die bisher noch nicht funktionieren ^^"
        */
       void Update()
       {
	       float x = Input.GetAxis("Horizontal");
	       isGrounded = Physics.CheckSphere(groundcheck.position, 0.4f, groundLayers);
	       Gravity(isGrounded);
	       MoveForward();
	       MoveLeftRight(x);
	       WalkWall(x);
	       CheckWallWalking();
	       CheckJump(isGrounded);
	       GoDown();
	       
       }


       void MoveForward()
       {
	       int horizontalInput = -10;
	       //transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);
	       characterController.Move(new Vector3(0, 0, horizontalInput) * Time.deltaTime);
       }
       void Gravity(bool isGrounded)
       {
	       if ((isGrounded || isWalling) && velocity.y < 0)
	       {
		       velocity.y = 0;
	       }
	       else
	       {
		       velocity.y += gravity * Time.deltaTime;
	       }
       }
       
       void MoveLeftRight(float x)
       {
	       if (push)
	       {
		       if (!isWalling)
		       {
			       if (x < 0)
			       {
				       if (line == 0)
				       {
					       characterController.Move(new Vector3(3.5f, 0, 0));
					       line = -1;
					       push = false;

				       }

				       else if (line == 1)
				       {

					       characterController.Move(new Vector3(3.5f, 0, 0));
					       line = 0;
					       push = false;
				       }
			       }

			       else if (x > 0)
			       {
				       if (line == 0)
				       {

					       characterController.Move(new Vector3(-3.5f, 0, 0));
					       line = 1;
					       push = false;
				       }

				       else if (line == -1)
				       {

					       characterController.Move(new Vector3(-3.5f, 0, 0));
					       line = 0;
					       push = false;
				       }
			       }
		       }
		       else
		       {
			       if (x < 0)
			       {
				       if (line == 0)
				       {
					       characterController.Move(new Vector3(3.5f, 0, 0));
					       line = -1;
					       currentTime = 1f;
					       isWalling = true;
					       push = false;

				       }

				       else if (line == 1)
				       {

					       characterController.Move(new Vector3(7f, 0, 0));
					       line = -1;
					       currentTime = 1f;
					       isWalling = true;
					       push = false;
				       }
			       }

			       else if (x > 0)
			       {
				       if (line == 0)
				       {

					       characterController.Move(new Vector3(-3.5f, 0, 0));
					       line = 1;
					       currentTime = 1f;
					       isWalling = true;
					       push = false;
				       }

				       else if (line == -1)
				       {

					       characterController.Move(new Vector3(-7f, 0, 0));
					       line = 1;
					       currentTime = 1f;
					       isWalling = true;
					       push = false;
				       }
			       }
		       }
	       }

	       if (x == 0)
	       {
		       push = true;
	       }
       }

       void CheckJump(bool isGrounded)
       {
	       if (!isWalling)
	       {
		       if (isGrounded && Input.GetButtonDown("Jump"))
		       {
		       
			       velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
		       }
		       characterController.Move(velocity*Time.deltaTime);
	       }
	       
       }

       void GoDown()
       {
	       if (Input.GetButtonDown("Fire1"))
	       {
		       if (rotatecounter == 0)
		       {
			       this.transform.Rotate(90, 0, 0);
			       characterController.height = 1f;
			       characterController.Move(new Vector3(0, -1, 0));
			       rotatecounter = 1;
		       }
		       else
		       {
			       this.transform.Rotate(-90, 0, 0);
			       characterController.height = 2f;
			       rotatecounter = 0;
		       }
		       
		       
	       }
       }

       void WalkWall(float x)
       {
	       if (push)
	       {
		       if (x < 0)
		       {
			       if (line == -1)
			       {
				       bool isWallLeft = Physics.CheckSphere(wallcheckleft.position, 0.4f, wallLayers);
				       if (isWallLeft)
				       {
					       f = 0.5f;
					       if (target.position.y < 3)
					       {
						       velocity.y = Mathf.Sqrt(3f * -2 * gravity);
						       characterController.Move(velocity*Time.deltaTime);
						       characterController.Move(new Vector3(f, 0, 0));
						       isWalling = true;
					       }
					       
				       }
				       else
				       {
					       isWalling = false;
				       }
			       }
		       }

		       else if (x > 0)
		       {
			       if (line == 1)
			       {
				       bool isWallRight = Physics.CheckSphere(wallcheckright.position, 0.4f, wallLayers);
				       if (isWallRight)
				       {

					       f = -0.5f;
					       if (target.position.y < 3)
					       {
						       velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
						       characterController.Move(velocity*Time.deltaTime);
						       characterController.Move(new Vector3(f, 0, 0));
						       isWalling = true;
					       }
				       }
				       else
				       {
					       isWalling = false;
				       }
			       }
			      
		       }
	       }
       }

       void CheckWallWalking()
       {
	       bool isWallLeft = Physics.CheckSphere(wallcheckleft.position, 0.4f, wallLayers);
	       bool isWallRight = Physics.CheckSphere(wallcheckright.position, 0.4f, wallLayers);
	       if (isWalling)
	       {
		       currentTime -= 1 * Time.deltaTime;
		       if (currentTime <= 0f)
		       {
			       isWalling = false;
			       currentTime = 1f;
		       }
	       }

	       if (!(isWallLeft || isWallRight))
		       isWalling = false;
       }
}
