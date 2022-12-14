using Interfaces;
using Photon.Pun;
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

            Save();
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

        public void EndScene()
        {
            int i = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("Loader.1", i);
            if (GameManager.isOnline)
            {
                LoadOnline(2);
            }
            else
            {
                LoadByIndex(2);
            }
        }

        public void LoadScene(int index)
        {
            if (GameManager.isOnline)
            {
                LoadOnline(index);
            }
            else
            {
                LoadByIndex(index);
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
                Debug.Log("waiting");
                await Task.Delay(100);
            } while (GameManager.saved == false);
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
            int i = PlayerPrefs.GetInt("Loader.1", 2);

            LoadByIndex(i);
        }

        public void LoadOnline()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int sceneIndex = PlayerPrefs.GetInt("Loader.1", 2);

                if (sceneIndex >= 2)
                    sceneIndex = 2;

                if (!loading)
                {
                    loading = true;

                    GameManager.FirstPos(sceneIndex);
                    anim.SetTrigger("FadeIn");
                    PhotonNetwork.LoadLevel(sceneIndex);
                }
            }
            else
            {
                if (!loading)
                {
                    int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["Scene"];
                    loading = true;
                    GameManager.FirstPos(i);
                    anim.SetTrigger("FadeIn");
                    PhotonNetwork.LoadLevel(i);
                }
            }
        }
        public void LoadOnline(int sceneIndex)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int i = 1;
                if (sceneIndex != -1)
                {
                    i = sceneIndex;
                }

                if (!loading)
                {
                    loading = true;

                    GameManager.FirstPos(sceneIndex);
                    anim.SetTrigger("FadeIn");
                    PhotonNetwork.LoadLevel(sceneIndex);
                }
            }
            else
            {
                if (!loading)
                {
                    int i = 1;
                    if (sceneIndex != -1)
                    {
                        i = sceneIndex;
                    }
                    loading = true;
                    GameManager.FirstPos(sceneIndex);
                    anim.SetTrigger("FadeIn");
                    PhotonNetwork.LoadLevel(sceneIndex);
                }
            }
        }

        public void RestartGame()
        {
            if (!loading)
            {
                PlayerPrefs.DeleteAll();
                loading = true;
                Load(1);
            }
        }
        public void FromJson()
        {
            int i = PlayerPrefs.GetInt("Loader.1", 2);
            PlayerPrefs.SetInt("Loader.1", i);
        }

        public bool Save()
        {
            if (SceneManager.GetActiveScene().buildIndex > 1)
            {
                PlayerPrefs.SetInt("Loader.1", SceneManager.GetActiveScene().buildIndex);
                Debug.Log("Saving: " + this.name);
                return true;
            }
            return true;
        }
    }
}
