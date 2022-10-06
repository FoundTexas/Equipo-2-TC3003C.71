using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogUI, dialogBox, InputBox, responsePrefab;
    [SerializeField] Transform spawnSpot;
    [SerializeField] TextMeshProUGUI dialogText;

    AudioSource audios;
    DialogueInteraction currentInteraction;
    int index;
    public static bool typeing;
    public static bool Ended;

    UnityEvent End;
    EventSystemUpdater events;

    void Start()
    {
        Ended = true;
        audios = GetComponent<AudioSource>();
        dialogUI.SetActive(false);
        dialogBox.SetActive(false);
    }

    public void ShowInput(bool b)
    {
        InputBox.SetActive(b);
    }

    public void StartInteraction(DialogueInteraction interaction, UnityEvent End)
    {
        InputBox.SetActive(false);
        dialogBox.SetActive(true);
        this.End = End;
        events = FindObjectOfType<EventSystemUpdater>();
        events.paused = true;
        events.UpdateSelected(dialogBox);
        StopAllCoroutines();
        Ended = false;
        typeing = false;
        currentInteraction = interaction;
        index = 0;
        foreach (Transform obj in spawnSpot)
        {
            Destroy(obj.gameObject);
        }
        Continue();
    }

    public void Continue()
    {
        dialogText.text = "";
        if (!typeing)
        {
            if (currentInteraction.lineContinue(index))
            {
                typeing = true;

                dialogUI.SetActive(true);
                dialogBox.SetActive(true);

                Line line = currentInteraction.getLine(index);
                audios.PlayOneShot(line.getSound());
                dialogText.text = line.getName() + ": \n";// + line.GetLine();
                string[] tmpText = line.getLine().Split(" ");

                StartCoroutine(Type(tmpText));
                index++;
            }
            else
            {
                var NextInteractions = currentInteraction.getInteractions();
                
                if (NextInteractions.Count > 2)
                {
                    StartInteraction(NextInteractions[Random.Range(0,NextInteractions.Count-1)], End);
                }
                else if (NextInteractions.Count > 1)
                {
                    dialogBox.SetActive(false);
                    dialogText.text = "Select:";

                    bool selecting = false;
                    foreach (DialogueInteraction inter in NextInteractions)
                    {
                        GameObject g = Instantiate(responsePrefab, spawnSpot);
                        g.GetComponent<Button>().onClick.AddListener(
                            delegate
                            {
                                StartInteraction(inter,End);
                            });
                        g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = inter.getButtonText();
                        if (!selecting)
                        {
                            selecting = true;
                            events.UpdateSelected(g);
                        }
                    }
                }
                else if (NextInteractions.Count == 1)
                {
                    StartInteraction(NextInteractions[0],End);
                }
                else
                {
                    Ended = true;
                    events.paused = false;
                    dialogUI.SetActive(false);
                    dialogBox.SetActive(false);
                    End.Invoke();
                }
            }
        }
        else if (typeing)
        {
            StopAllCoroutines();
            typeing = false;
            Line line = currentInteraction.getLine(index-1);
            dialogText.text = line.getName() + ": \n" + line.getLine();
        }
    }

    IEnumerator Type(string[] log)
    {
        foreach (string str in log)
        {
            yield return new WaitForSeconds(0.17f);
            dialogText.text += str + " ";
        }
        typeing = false;
    }
}
