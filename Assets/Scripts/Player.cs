using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rB;
    public GameObject CharonGO;
    public GameObject ClothGO;
    public GameObject PaddleGO;
    public Animator _animCharon;
    public Animator _animCloth;
    public Animator _animPaddle;
    /*public Vector3 COM;

    Transform m_COM;
    
    float horizontalInput;
    float steerFactor;*/
    public GameObject endPointShop;
    public GameObject charonModerl;
    public GameObject clothModerl;
    public GameObject paddleModerl;

    public UI cameraScript;
    float verticalInput;
    float movementFactor;
    public float movementThresold = 0.3f;


    float movement;
    public bool restartLevel;
    public bool playerIsDead;
    public bool flowStarted = false;
    [Header("Settings")]
    public float speed = 20f;
    public float rotatePower = 10f;
    public float flowPower = 40f;

    [Space(20)]
    public float health = 0f;
    public float healthMax = 100f;

    [Space(20)]
    public float spiritHave = 0f;
    public float spiritNeed = 3f;

    
    void Awake()
    {
        health = healthMax;
       
    }

    // Start is called before the first frame update
    void Start()
    {
        _rB = GetComponent<Rigidbody>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<UI>();
        _animCharon = CharonGO.GetComponent<Animator>();
        //_animCloth = ClothGO.GetComponent<Animator>();
        _animPaddle = PaddleGO.GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        { 

            PlayerDestroy(); 
        }

        if (cameraScript.gameState == 0)
        { transform.Translate(Vector3.forward * Time.deltaTime * 8f); }

        if (cameraScript.gameState == 1)
        {   Movement(); }

        if (cameraScript.gameState == 2)
        { ShopMove(); }
        //Balance();
        //Steer();

        if (Input.GetKey("r"))
        {
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.y, 0.0f);
        }


    }

    /*void Balance()
    {
        if (!m_COM)
        {
            m_COM = new GameObject("COM").transform;
            m_COM.SetParent(transform);
        }

        m_COM.position = COM;
        _rB.centerOfMass = m_COM.position;
    }*/

    void Movement()
    {
        if (!flowStarted)
        { verticalInput = Input.GetAxis("Vertical");
            CharonGO.transform.rotation = transform.rotation;
            ClothGO.transform.rotation = transform.rotation;
            PaddleGO.transform.rotation = transform.rotation;

            _animCharon.SetBool("flow", false);
            //_animCloth.SetBool("flow", false);
            _animPaddle.SetBool("flow", false);
        }

        movementFactor = Mathf.Lerp(movementFactor, verticalInput, Time.deltaTime * movementThresold);
        transform.Translate(0.0f, 0.0f, movementFactor * speed * Time.deltaTime);

        //_rB.AddForce(transform.forward * movementFactor * speed);

        
        if (Input.GetKey("w"))
        {
            _animCharon.SetFloat("inverse", 1f);
            //_animCloth.SetFloat("inverse", 1f);
            _animPaddle.SetFloat("inverse", 1f);

            _animCharon.SetBool("idle", false);
           // _animCloth.SetBool("idle", false);
            _animPaddle.SetBool("idle", false);
        }

        if (Input.GetKey("s"))
        {
            _animCharon.SetFloat("inverse", -1f);
            //_animCloth.SetFloat("inverse", -1f);
            _animPaddle.SetFloat("inverse", -1f);

            _animCharon.SetBool("idle", false);
            //_animCloth.SetBool("idle", false);
            _animPaddle.SetBool("idle", false);
        }

        /*if (Input.GetKey("d"))
        {
            _animCharon.SetBool("mirrorSwim", false);
            _animCloth.SetBool("mirrorSwim", false);
            _animPaddle.SetBool("mirrorSwim", false);
        }

        if (Input.GetKey("a"))
        {
            _animCharon.SetBool("mirrorSwim", true);
            _animCloth.SetBool("mirrorSwim", true);
            _animPaddle.SetBool("mirrorSwim", true);
        }*/

        if (!Input.GetKey("w") && !Input.GetKey("s"))
        {
            _animCharon.SetBool("idle", true);
            //_animCloth.SetBool("idle", true);
            _animPaddle.SetBool("idle", true);
        }

        if (Input.GetKey("d") && !flowStarted)
        {
            _rB.transform.Rotate(0.0f, rotatePower, 0.0f, Space.Self);            
        }

        if (Input.GetKey("a") && !flowStarted)
        {
            _rB.transform.Rotate(0.0f, -rotatePower, 0.0f, Space.Self);
        }

        
    }

    void ShopMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(endPointShop.transform.position.x, transform.position.y, endPointShop.transform.position.z), Time.deltaTime * 8f);
    }

    /*void Steer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        steerFactor = Mathf.Lerp(steerFactor, horizontalInput , Time.deltaTime / movementThresold);
        transform.Rotate(0.0f, steerFactor * rotatePower * Time.deltaTime, 0.0f);

    }*/

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if (go.tag == "gameRestart")
        {
            restartLevel = true;
            //Debug.Log("TRIGGER!");
            endPointShop = go;
            cameraScript.coinsHave += spiritHave/3;
            cameraScript.coinsHaveText.text = cameraScript.coinsHave.ToString();
            transform.LookAt(endPointShop.transform);
            _animCharon.SetBool("idle", true);
            //_animCloth.SetBool("idle", true);
            _animPaddle.SetBool("idle", true);
        }
    }

    void PlayerDestroy()
    {
        playerIsDead = true;
    }

}
