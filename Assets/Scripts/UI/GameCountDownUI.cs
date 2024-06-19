using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameCountDownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countDownTimerText;
        private const float CountDownToStart = 3f;
       
      
        private void Start()
        {
            KitchenGameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        }

        private void GameManager_OnStateChanged(object sender, EventArgs e)
        {
            if (KitchenGameManager.Instance.IsCountDownStartActive())
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Hide()
        {
            countDownTimerText.gameObject.SetActive(false); 
            StopCoroutine(StartCountDown());
        }

        private void Show()
        {
            countDownTimerText.gameObject.SetActive(true);
            StartCoroutine(StartCountDown());
        }

        IEnumerator StartCountDown()
        {
            
            float maximTime = CountDownToStart;

            do
            {
                yield return new WaitForSeconds(1f);
                maximTime -= 1f;
                countDownTimerText.text = maximTime.ToString();
                
            } while (maximTime > 0f);

        }
    }
}
