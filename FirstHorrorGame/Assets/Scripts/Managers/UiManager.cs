using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace CSF
{
    public class UiManager : MonoBehaviour
    {
        public Slider sliderHP, sliderStamina;
        public MovimentaPersonagem scriptMovimenta;
        public Text municao;
        public Image imagemModoTiro;
        public Sprite[] spriteModoTiro;
        public Image mira;
        // Start is called before the first frame update
        void Start()
        {
            scriptMovimenta = GameObject.FindWithTag("Player").GetComponent<MovimentaPersonagem>();
            municao.enabled = true;
            imagemModoTiro.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            sliderHP.value = scriptMovimenta.hp;
            sliderStamina.value = scriptMovimenta.stamina;
        }
    }
}
