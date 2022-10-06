using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using GameManagement;

public class NPC : MonoBehaviour
{
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

    public static bool PossibleDialogue = false;
    public UnityEvent StartConv, EndConv;

    Quaternion originalRot;
    Quaternion rot;

    Move Player;

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
        Locked = false;
        originalRot = transform.rotation;
        rot = originalRot;
        manager = FindObjectOfType<DialogueManager>();
    }
    private void Update()
    {
        GetTarget();
        PossibleDialogue = distance <= MinDist;

        if (!PossibleDialogue)
        {
            rot = originalRot;
        }
        if(transform.rotation != rot)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.3f);
        }
        if (!Locked && interactions.Count > index && distance <= MaxDist)
            manager.ShowInput(PossibleDialogue);
    }

    void InteractionHandler(InputAction.CallbackContext context)
    {
        if (DialogueManager.Ended)
        {
            if(Player.wasGrounded)
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

                            Vector3 dir = Player.transform.position - transform.position;
                            dir.y = 0; // keep the direction strictly horizontal
                            rot = Quaternion.LookRotation(dir);
                     
                            foreach (var npc in Involved)
                            {
                                dir = Player.transform.position - npc.transform.position;
                                dir.y = 0; // keep the direction strictly horizontal
                                npc.rot = Quaternion.LookRotation(dir);
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
        turnRig.weight = Mathf.Clamp(MaxDist - distance, 0.0f, 1.0f);
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
                Player = player.gameObject.GetComponent<Move> ();
                target.position = player.transform.position;
            }
        }
    }

    void ResetPlayer()
    {
        if (endEvents[index])
        {
            endEvents[index].SetTrigger();
        }
        Player.canMove = true;
        Locked = false;
    }
}
