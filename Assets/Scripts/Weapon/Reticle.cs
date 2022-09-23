using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeaponSystem
{
    public class Reticle : MonoBehaviour
    {
        [SerializeField] Image[] images;
        [SerializeField] Color Normal, Interactable, Enemy;
        [SerializeField] LayerMask layers;
        [SerializeField] Transform camara;
        WeaponManager WM;
        public int hitting;

        private void Start()
        {
            WM = FindObjectOfType<WeaponManager>();
        }
        private void Update()
        {
            if (GetRay().transform)
            {
                hitting = GetRay().transform.gameObject.layer;
            }
            else
            {
                hitting = -1;
            }

            switch (hitting)
            {
                default:
                    ChangeColor(Normal);
                    break;
                case 6:
                    ChangeColor(Interactable);
                    break;
            }
        }

        void ChangeColor(Color change)
        {
            foreach (var img in images)
            {
                img.color = change;
            }
        }
        RaycastHit GetRay()
        {
            RaycastHit tmp = new RaycastHit();
            if (Physics.Raycast(camara.position, camara.forward,
                out RaycastHit hitinfo, WM.currentSelect().distance, layers))
            {
                tmp = hitinfo;
            }
            return tmp;
        }
    }
}
