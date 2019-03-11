using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

namespace MonsterFarm.Intro
{
    public class Intro : MonoBehaviour
    {
        public Animator book;
        private int _introIdx= 0;
        public string[] text;
        public Text textSubtitle;
        private bool canControl = false;
        public Image fade;
        private void Awake()
        {
            book = GetComponent<Animator>();
            text = new[] { "Among all the stars of the night, there is a wildland as special as imagination. One day, beyond time, humans find this place full of countless unique creatures and amazing treasures. They realize for the first time they are not alone in this world. These creatures they now discover remain mysterious to them. Yes, they are creatures and they are monsters, but only some are aggressive while others are friendly.  Most people still want to avoid them. But things are changing. The player can choose how to behave toward the monsters. He/she will need to subdue the dangerous ones by doing battle with them but he/she can become friends with the monsters if they are more naturally gentle. ", "In this special land, you can domesticate monsters by owning and managing a farm where they can live with you and you can also be a warrior when that’s the best way to encounter them. You can be the bridge between humans and monsters, to let these two races understand each other better.  ", "Grab your gear and get ready to have adventures in the wildland. You can have battles with deadly monsters and find valuable treasures. You can make friends with other monsters and have your own farm where they’ll live with you in harmony.  ", "You can go to town and trade with other people, to get more materials to upgrade your farm or improve your adventure gear." };
            textSubtitle.text = text[_introIdx];
            
            
        }

        private void Start()
        {
            StartCoroutine(FadeIn(0.5f, () =>
                {
                    canControl = true;
                    Debug.Log("finish fade in");		
                }
            ));
        }

        IEnumerator FadeIn(float duration, Action callback)
        {
            var timer = 0f;
            var color = fade.color;
            while (timer < duration)
            {
                color.a = 1f - timer / duration;
                fade.color = color;
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (callback != null)
            {
                callback();
            }
        }
	
        IEnumerator FadeOut(float duration, Action callback)
        {
            var timer = 0f;
            var color = fade.color;
            while (timer < duration)
            {
                color.a = timer / duration;
                fade.color = color;
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (callback != null)
            {
                callback();
            }
        }

        protected void Update()
        {
            if (canControl && Input.GetKeyDown(KeyCode.E))
            {
                _introIdx++;
                book.SetTrigger("NextPage");
                if (_introIdx < text.Length)
                {
                    textSubtitle.text = text[_introIdx];
                }
                else
                {
                    canControl = false;
                    StartCoroutine(FadeOut(0.5f, () =>
                        {
                            SceneManager.LoadScene("level1", LoadSceneMode.Single);		
                        }
                    ));
                    
                }                
            }
        }
    }
}