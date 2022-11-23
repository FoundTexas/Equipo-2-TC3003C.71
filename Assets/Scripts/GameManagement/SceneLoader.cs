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
        public PhotonView pv;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void Start()
        {
            pv = GetComponent<PhotonView>();
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

        public void RPCLoadScene(int index)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                LoadOnline(index);
            }
            // pv.RPC("PunRPCLoadScene", RpcTarget.All, 4);
        }

        [PunRPC]
        public void PunRPCLoadScene(int index)
        {
            LoadScene(index);
        }
        
        /// <summary>
        /// Method that start Loading scene routine by its index.
        /// </summary>
        /// <param name="index"> Scene index int. </param>
        public void LoadByIndex(int index)
        {
            Debug.Log("Trying to load scene by index.");
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

            // anim.SetTrigger("FadeIn");
            var scene = SceneManager.LoadSceneAsync(index);
            scene.allowSceneActivation = false;
            await Task.Delay(3000);
            do
            {
                // Debug.Log("waiting");
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
            int i = PlayerPrefs.GetInt("Loader.1", 1);

            if (i >= 2)
                    i = 2;

            LoadByIndex(i);
        }

        public void LoadOnline()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int sceneIndex = PlayerPrefs.GetInt("Loader.1", 1);

                if (sceneIndex >= 2)
                    sceneIndex = 2;

                var hash = PhotonNetwork.CurrentRoom.CustomProperties;
                if(hash.ContainsKey("Scene"))
                    hash["Scene"] = sceneIndex;
                else if(!hash.ContainsKey("Scene"))
                    hash.Add("Scene", sceneIndex);
                
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

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
                    print("Joining as client");
                    int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["Scene"];
                    loading = true;
                    GameManager.FirstPos(i);
                    anim.SetTrigger("FadeIn");
                    PhotonNetwork.LoadLevel(i);
                }
            }
        }
        public void LoadOnline(int sceneIndex, Vector3 pos)
        {
            GameManager.setCheckPoint(pos);
            LoadOnline(sceneIndex);
        }
        [PunRPC]
        public void LoadOnline(int sceneIndex)
        { 
            if (PhotonNetwork.IsMasterClient)
            {
                int i = 1;
                if (sceneIndex != -1)
                {
                    i = sceneIndex;
                }

                var hash = PhotonNetwork.CurrentRoom.CustomProperties;
                if(hash.ContainsKey("Scene"))
                    hash["Scene"] = i;
                else if(!hash.ContainsKey("Scene"))
                    hash.Add("Scene", i);
                
                print("Load Scene as server: " + i);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

                if (!loading)
                {
                    loading = true;

                    GameManager.FirstPos(i);
                    anim.SetTrigger("FadeIn");
                    PhotonNetwork.LoadLevel(i);
                }
            }
            else
            {
                if (!loading)
                {
                    sceneIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["Scene"];
                    print("Load Scene as client: " + sceneIndex);
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
            int i = PlayerPrefs.GetInt("Loader.1", 1);
            PlayerPrefs.SetInt("Loader.1", i);
        }

        public bool Save()
        {
            if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("Loader.1", 1))
            {
                PlayerPrefs.SetInt("Loader.1", SceneManager.GetActiveScene().buildIndex);
                Debug.Log("Saving: " + this.name);
                return true;
            }
            return true;
        }
    }
}
