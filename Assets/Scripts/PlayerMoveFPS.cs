using System;
using UnityEngine;

// Garantálja, hogy az objektumon legyen CharacterController, mielőtt a script lefut
[RequireComponent(typeof(CharacterController))]
public class PlayerMoveFPS : MonoBehaviour
{
    [Header("Mozgás beállítások")]
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float dashSpeed = 50f;
    private int doubleJump = 0;
    
    
    [Header("Nézelődés beállítások")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f; // Korlátozza, mennyire nézhetünk fel vagy le (fok)

    public float cameraY; // A kamera alapértelmezett magasságának tárolására
    
    Vector3 moveDirection = Vector3.zero; // Ebben tároljuk a mozgás irányát és erejét
    float rotationX = 0f; // A függőleges (fel-le) forgás aktuális értéke
    
    public bool canMove = true; // Ezzel globálisan letiltható a mozgás (pl. menüben)

    CharacterController characterController; // Referencia a fizikai vezérlőhöz
    
    void Start()
    {
        // Komponens lekérése és a kurzor elrejtése/középre zárása
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Elmentjük a kamera induló magasságát
    }

    void Update()
    {
        //Debug.Log( doubleJump );
        
        #region Handles Movement
        // Irányok lekérése a karakter helyi koordinátarendszere alapján (merre néz éppen)
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        // Futás ellenőrzése: ha le van nyomva a Shift, a runSpeed-et használjuk
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Sebesség kiszámítása tengelyenként (Vertical = előre/hátra, Horizontal = oldalra)
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        // float curSpeedX;
        //
        // if (canMove)
        // {
        //     if (isRunning)
        //         curSpeedX = runSpeed * Input.GetAxis("Vertical");
        //
        //     else
        //         curSpeedX = walkSpeed * Input.GetAxis("Vertical");
        // }
        // else
        //     curSpeedX = 0;
        
        
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        // float curSpeedY;
        //
        // if (canMove)
        // {
        //     if (isRunning)
        //         curSpeedY = runSpeed * Input.GetAxis("Vertical");
        //     else
        //         curSpeedY = walkSpeed * Input.GetAxis("Vertical");
        // }
        // else
        //     curSpeedY = 0;
        
        
        // Elmentjük az aktuális függőleges sebességet (gravitáció/ugrás), mielőtt felülírnánk az irányt
        float movementDirectionY = moveDirection.y;
        
        // Összeadjuk az előre és oldalirányú mozgást
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        
        #endregion
        
        
        
        
        #region Handles Jumping

        // Ugrás: ha nyomják a gombot, van engedély és a földön állunk
        if (Input.GetButtonDown("Jump") && canMove && doubleJump < 2)
        {
            moveDirection.y = jumpPower; // Felfelé irányuló erőt adunk neki
            doubleJump += 1;
        }
        else
        {
            // Ha nem ugrunk, visszakapja a korábbi függőleges sebességét
            moveDirection.y = movementDirectionY;

            if (characterController.isGrounded)
                doubleJump = 0;
        }

        // Gravitáció alkalmazása: ha a levegőben vagyunk, folyamatosan húzzuk lefelé a karaktert
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        //Debug.Log(doubleJump);

        #endregion
        
        #region Handles Rotation 
        // A tényleges mozgatás végrehajtása a CharacterControllerrel
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            // Függőleges nézelődés: az egér Y mozgását hozzáadjuk a forgáshoz (invertálva a mínusz jellel)
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            // Clamping: megakadályozzuk, hogy a kamera átforduljon a fejed felett
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            
            // Csak a kamerát forgatjuk fel-le
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            
            // A teljes karaktert (testet) forgatjuk balra-jobbra az egér X alapján
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
        
        
        #region Handles Dash

        bool isDashing = Input.GetKey(KeyCode.LeftAlt);
        
        
        
        //float dashDirection =  
                            // TODO A Dasht megoldani :)))))) | Maybe tedd az egész mozgást egy else ágba 
        if (isDashing)
        {
            
            Debug.Log($"A dash lefutott | {isDashing}");
            walkSpeed = dashSpeed;
            isDashing = false;
            Debug.Log($"Dash jelenlegi értéke : {isDashing}");
            
        }
        walkSpeed = 6f;
        #endregion
    }
}