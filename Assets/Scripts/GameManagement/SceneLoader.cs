using Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManagement
{
    /// <summary>
    /// Class in Charge of chamging between scenes with an async method.
    /// </summary>
    public class SceneLoader : MonoBehaviour, ISave
    {
        [Tooltip("Progress visualization slider")]
        [SerializeField] Slider progress;
        bool loading = false;
        Animator anim;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void Start()
        {
            anim = GetComponent<Animator>();
            FromJson();
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Method that start Loading scene routine by its name.
        /// </summary>
        /// <param name="name"> Scene name string. </param>
        public void LoadByName(string name)
        {
            if (SceneManager.GetSceneByName(name) == null) { return; }
            if (!loading)
            {
                loading = true;
                int index = SceneManager.GetSceneByName(name).buildIndex;
                GameManager.FirstPos(index);
                Load(index);
            }
        }
        /// <summary>
        /// Method that start Loading scene routine by its index.
        /// </summary>
        /// <param name="index"> Scene index int. </param>
        public void LoadByIndex(int index)
        {
            if (SceneManager.GetSceneByBuildIndex(index) == null) { return; }
            if (!loading)
            {
                loading = true;
                GameManager.FirstPos(index);
                Load(index);
            }
        }
        /// <summary>
        /// Method that start Loading scene routine by its name.
        /// </summary>
        /// <param name="name"> Scene name string. </param>
        public void LoadByName(string name, Vector3 pos)
        {
            if (SceneManager.GetSceneByName(name) == null) { return; }
            if (!loading)
            {
                GameManager.setCheckPoint(pos);
                loading = true;
                int index = SceneManager.GetSceneByName(name).buildIndex;
                Load(index);
            }
        }
        /// <summary>
        /// Method that start Loading scene routine by its index.
        /// </summary>
        /// <param name="index"> Scene index int. </param>
        public void LoadByIndex(int index, Vector3 pos)
        {
            if (SceneManager.GetSceneByBuildIndex(index) == null) { return; }
            if (!loading)
            {
                GameManager.setCheckPoint(pos);
                loading = true;
                Load(index);
            }
        }
        /// <summary>
        /// Loading Async method.
        /// </summary>
        /// <param name="index"> Scene index int. </param>
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
        /// <summary>
        /// Void that quits the application.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

        public void LoadLastSaved()
        {
            int i = 1;
            if (PlayerPrefs.HasKey("Loader.1"))
            {
                i = PlayerPrefs.GetInt("Loader.1");
            }

            LoadByIndex(i);
        }

        public void FromJson()
        {
            int i = 1;
            if (PlayerPrefs.HasKey("Loader.1"))
            {
                i = PlayerPrefs.GetInt("Loader.1");
            }
            PlayerPrefs.SetInt("Loader.1", i);
        }

        public bool Save()
        {
            PlayerPrefs.GetInt("Loader.1", SceneManager.GetActiveScene().buildIndex);
            return true;
        }
    }
}
