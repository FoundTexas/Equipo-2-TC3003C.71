using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using GameManagement;

public class NPC : MonoBehaviour
{
    public GameObject InputBox;
    public NPC[] Involved;
    PlayerInputs PlayerInput;
    InputAction JumpInput;

    public InGameEvent[] endEvents;

    [SerializeField] Transform target;
    [SerializeField] Rig turnRig;
    [SerializeField] float MaxDist, MinDist, distance = Mathf.Infinity;
    public List<DialogueInteraction> interactions;
    public int index;
    public bool Locked;
    DialogueManager manager;

    public UnityEvent StartConv, EndConv;

    Quaternion originalRot;
    Quaternion rot;

    Move Player;
    Animator anim;

    private void Awake()
    {
        PlayerInput = new PlayerInputs();
    }
    private void OnEnable()
    {
        JumpInput = PlayerInput.Game.Jump;
        JumpInput.Enable();

        JumpInput.performed += InteractionHandler;
    }
    private void OnDisable()
    {
        JumpInput.Disable();
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        ShowInput(false);
        Locked = false;
        originalRot = transform.rotation;
        rot = originalRot;
        manager = FindObjectOfType<DialogueManager>();
    }
    private void Update()
    {
        if (Waves.isEnded)
        {
            anim.SetBool("Afraid",false);
            GetTarget();
            if (distance <= MaxDist)
            {
                if (!Locked && interactions.Count > index && distance <= MinDist)
                {
                    InputBox.transform.LookAt(Camera.main.transform);
                    ShowInput(true);
                    Player.PossibleDialogue = true;
                }
                else if (distance > MinDist)
                {
                    ShowInput(false);
                    Player.PossibleDialogue = false;
                }
            }
            else
            {
                rot = originalRot;
            }

            if (transform.rotation != rot)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.3f);
            }
        }
        else if (!Waves.isEnded)
        {
            anim.SetBool("Afraid",true);
        }
    }
    void ShowInput(bool b)
    {
        InputBox.SetActive(b);
    }

    void InteractionHandler(InputAction.CallbackContext context)
    {
        if (DialogueManager.Ended)
        {
            if (Player.wasGrounded)
            {
                Player.canMove = false;
                if (context.performed && !Locked)
                {
                    if (interactions.Count >= index)
                    {
                        if (distance <= MaxDist)
                        {
                            Locked = true;
                            Player.StopMove();
                            Player.canMove = false;
                            ShowInput(false);

                            if (turnRig)
                            {
                                Vector3 dir = Player.transform.position - transform.position;
                                dir.y = 0; // keep the direction strictly horizontal
                                rot = Quaternion.LookRotation(dir);

                                foreach (var npc in Involved)
                                {
                                    dir = Player.transform.position - npc.transform.position;
                                    dir.y = 0; // keep the direction strictly horizontal
                                    npc.rot = Quaternion.LookRotation(dir);
                                }
                            }

                            Player.transform.LookAt(new Vector3(
                                transform.position.x,
                                Player.transform.position.y,
                                transform.position.z
                                ));

                            StartConv.Invoke();
                            EndConv.AddListener(ResetPlayer);
                            manager.StartInteraction(interactions[index], EndConv);
                        }
                        else
                        {
                            Player.canMove = true;
                        }
                    }
                    else
                    {
                        Player.canMove = true;
                    }
                }
                else
                {
                    Player.canMove = true;
                }
            }
        }
    }


    void GetTarget()
    {
        if (turnRig)
        {
            turnRig.weight = Mathf.Clamp(MaxDist - distance, 0.0f, 1.0f);
        }
        if (Locked)
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        distance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float newDistance = Vector3.Distance(transform.position, player.transform.position);
            if (newDistance <= distance)
            {
                distance = Mathf.Abs(newDistance);
                Player = player.gameObject.GetComponent<Move>();
                target.position = player.transform.position;
            }
        }
    }

    void ResetPlayer()
    {
        Player.canMove = true;
        Locked = false;
        if (endEvents[index])
        {
            endEvents[index].PunRPCSetTrigger();
        }
    }
}
