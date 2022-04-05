using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float speedAlarm = 10.0f;
    public float lookSensi = 0.3f;
    public float strongboxTime = 3.0f;
    public float carryBagFactor = 0.5f;
    public float buttonTime = 0.2f;
    public float stunThrowTimer = 0.5f;
    public float overTimeMaxFactor = 0.5f;
    public float overTimeMaxTime = 6.0f;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 lastValidMoveInput;
    [HideInInspector] public bool actionPressed = false;
    [HideInInspector] public bool spawned = false;
    private float actionAccumulator = 0.0f;

    private StrongboxComponent nearbyStrongbox = null;
    private BagComponent nearbyBag = null;
    private GameButtonComponent nearbyButton = null;
    public bool carryBag = false;
    private float carryAcc = 0.0f;
    private float stunTimer = 0.0f;
    public bool isInZone = false;

    private Rigidbody2D rb;
    public bool hasBag = false;

    [SerializeField] private Image actionUI;

    private Animator animator;

    private bool strongboxPressing = false;
    private bool buttonPressing = false;

    public float footTime = 0.3f;
    private float footAcc = 0.0f;

    public float hitTime = 0.2f;
    private float hitAcc = 0.0f;

    private int playerIndex = 1;


    private void Awake()
    {
        SceneManager.MoveGameObjectToScene(gameObject, StateManager.Instance.GetBootScene());
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        hasBag = false;
        animator.SetBool("HasBag", hasBag);
        actionUI.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        actionUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        PlayerManager.Instance.AddPlayer(gameObject);
        playerIndex = PlayerManager.Instance.GetPlayerCount();
    }


    private void Update()
    {
        if (isInZone && hasBag)
        {
            GlobalManager.Instance.ResetLevel();
            GlobalManager.Instance.currentLevel++;
            PlayerManager.Instance.ResetPlayersSpawned();
            StateManager.Instance.SwitchToScene(0);
            AudioManager.PlaySound("win");
        }


        actionUI.transform.position = transform.position;

        // Action
        if (actionPressed && CanPressAction())
        {
            if (nearbyStrongbox != null)
            {
                if (!strongboxPressing)
                {
                    strongboxPressing = true;
                }

                if (strongboxPressing)
                {
                    float percent = actionAccumulator / strongboxTime;
                    actionUI.gameObject.SetActive(true);
                    actionUI.fillAmount = percent;
                }
            }

            if (nearbyButton != null)
            {
                if (!buttonPressing)
                {
                    buttonPressing = true;
                }

                if (buttonPressing)
                {
                    float percent = actionAccumulator / buttonTime;
                    actionUI.gameObject.SetActive(true);
                    actionUI.fillAmount = percent;
                }
            }

            actionAccumulator += Time.deltaTime;
            animator.SetFloat("Action", actionAccumulator);

            hitAcc += Time.fixedDeltaTime;
            if (hitAcc > footTime)
            {
                hitAcc = 0.0f;
                AudioManager.PlaySound("hit" + Random.Range(1, 11));
            }
        }
        else
        {
            hitAcc = 0.0f;
            actionAccumulator = 0.0f;
            if (strongboxPressing)
            {
                strongboxPressing = false;
                actionUI.fillAmount = 0.0f;
                actionUI.gameObject.SetActive(false);
                animator.SetFloat("Action", actionAccumulator);
            }
            if (buttonPressing)
            {
                buttonPressing = false;
                actionUI.fillAmount = 0.0f;
                actionUI.gameObject.SetActive(false);
                animator.SetFloat("Action", actionAccumulator);
            }
        }


        if (nearbyStrongbox != null && actionAccumulator >= strongboxTime)
        {
            nearbyStrongbox.Explose();
            nearbyStrongbox = null;

            actionAccumulator = 0.0f;
            animator.SetFloat("Action", actionAccumulator);
            actionPressed = false;
            strongboxPressing = false;
            buttonPressing = false;
            actionUI.fillAmount = 0.0f;
            actionUI.gameObject.SetActive(false);
        }
        /*else if (nearbyBag != null && actionPressed)
        {
            CarryBag();
            nearbyBag.Carry();
            nearbyBag = null;

            actionAccumulator = 0.0f;
            actionPressed = false;
            strongboxPressing = false;
            buttonPressing = false;
            actionUI.fillAmount = 0.0f;
            actionUI.gameObject.SetActive(false);
        }*/
        else if (nearbyButton != null && actionAccumulator >= buttonTime)
        {
            nearbyButton.Press();
            nearbyButton = null;

            actionAccumulator = 0.0f;
            animator.SetFloat("Action", actionAccumulator);
            actionPressed = false;
            strongboxPressing = false;
            buttonPressing = false;
            actionUI.fillAmount = 0.0f;
            actionUI.gameObject.SetActive(false);
        }
        else if (nearbyButton == null && carryBag && actionPressed)
        {
            actionAccumulator = 0.0f;
            animator.SetFloat("Action", actionAccumulator);
            actionPressed = false;
            strongboxPressing = false;
            buttonPressing = false;
            actionUI.fillAmount = 0.0f;
            actionUI.gameObject.SetActive(false);

            ThrowBag();
        }
    }

    private bool CanPressAction()
    {
        return nearbyStrongbox != null || nearbyButton != null || carryBag;
    }

    public void SetStrongbox(StrongboxComponent strongboxComponent)
    {
        nearbyStrongbox = strongboxComponent;
    }

    public void SetBag(BagComponent bagComponent)
    {
        nearbyBag = bagComponent;
    }

    public void SetButton(GameButtonComponent buttonComponent)
    {
        nearbyButton = buttonComponent;
    }

    private void FixedUpdate()
    {
        if (carryBag) carryAcc += Time.fixedDeltaTime;

        stunTimer -= Time.fixedDeltaTime;

        if (stunTimer > 0.0f || actionAccumulator > 0.0f)
        {
            if (animator.GetFloat("Movement") != 0.0f)
                animator.SetFloat("Movement", 0.0f);
            return;
        }

        if (stunTimer < -0.5f && nearbyBag != null)
        {
            Destroy(nearbyBag.gameObject);
            CarryBag();
        }

        // Movement
        Vector2 movement;
        float movementMagn = 0.0f;
        if (GlobalManager.Instance != null && GlobalManager.Instance.IsAlarmActivated())
        {
            if (moveInput.magnitude < 0.3f)
            {
                movement = Vector2.zero;
            }
            else
            {
                movement = moveInput.normalized;
            }
        }
        else
        {
            movement = (moveInput.magnitude > 1.0f) ? moveInput.normalized : moveInput;
            if (movement.magnitude < 0.3f)
            {
                movement = Vector2.zero;
            }
        }
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime * GetSpeed());

        movementMagn = movement.magnitude;
        animator.SetFloat("Movement", movementMagn);

        if (movementMagn > 0.3f)
        {
            footAcc += Time.fixedDeltaTime;
            if (footAcc > footTime)
            {
                footAcc = 0.0f;
                AudioManager.PlaySound("footstepPlayer" + playerIndex);
            }

            lastValidMoveInput = movement.normalized;

            float rotZ = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90.0f);
        }
        else
        {
            footAcc = 0.0f;
        }
    }

    public float GetSpeed()
    {
        float speeeeeeeeeed = (GlobalManager.Instance.IsAlarmActivated()) ? speedAlarm : speed;
        if (carryBag)
        {
            speeeeeeeeeed *= carryBagFactor;

            //float tPercent = Mathf.Clamp01(carryAcc / overTimeMaxTime);
            //float overTimeFactor = 1.0f - overTimeMaxFactor * tPercent;
            //speeeeeeeeeed *= overTimeFactor;
        }
        return speeeeeeeeeed;
    }

    private void CarryBag()
    {
        carryBag = true;
        hasBag = true;
        animator.SetBool("HasBag", hasBag);
    }

    private void ThrowBag()
    {
        carryBag = false;
        hasBag = false;
        animator.SetFloat("Movement", 0.0f);
        animator.SetFloat("Action", 0.0f);
        animator.SetBool("HasBag", hasBag);
        animator.SetTrigger("Throw");
        carryAcc = 0.0f;
        stunTimer = stunThrowTimer;
    }
}