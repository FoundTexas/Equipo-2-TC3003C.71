using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Slider progress;
    bool loading = false;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void LoadByName(string name)
    {
        if(SceneManager.GetSceneByName(name) == null) { return; }
        if (!loading)
        {
            loading = true;
            int index = SceneManager.GetSceneByName(name).buildIndex;
            Load(index);
        }
    }
    public void LoadByIndex(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index) == null) { return; }
        if (!loading)
        {
            loading = true;
            Load(index);
        }
    }

    async void Load(int index)
    {
        anim.SetTrigger("FadeIn");
        var scene = SceneManager.LoadSceneAsync(index);
        scene.allowSceneActivation = false;
        await Task.Delay(3000);
        do
        {
            progress.value = scene.progress;
        } while (scene.progress < 0.9f);
        scene.allowSceneActivation = true;
    }
}
