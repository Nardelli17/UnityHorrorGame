using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CSF;

namespace CSF
{
    public class UiManager : MonoBehaviour
    {
        public Slider sliderHP, sliderStamina;
        public MovimentaPersonagem scriptMovimenta;
        // Start is called before the first frame update
        void Start()
        {
            scriptMovimenta = GameObject.FindWithTag("Player").GetComponent<MovimentaPersonagem>();
        }

        // Update is called once per frame
        void Update()
        {
            sliderHP.value = scriptMovimenta.hp;
            sliderStamina.value = scriptMovimenta.stamina;
        }
    }
}
