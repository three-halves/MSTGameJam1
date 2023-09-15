using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PostGameWindow : MonoBehaviour
{
    [SerializeField] GameObject postGameWindow;
    [SerializeField] GameObject retryButton;
    [SerializeField] Animator postGameAnimator;

    [SerializeField] Sprite[] playerLogoSprites;
    [SerializeField] Image playerLogoImage;

    // [SerializeField] private AudioClip menuMusic;
    // Start is called before the first frame update
    void Start()
    {
        postGameWindow.SetActive(false);
        // postGameRoutine(1);   
    }

    public void postGameRoutine(int winningTeam)
    {
        postGameWindow.SetActive(true);
        Time.timeScale = 0.0f;
        playerLogoImage.sprite = playerLogoSprites[winningTeam];
        postGameAnimator.SetTrigger("Appear");
        EventSystem.current.SetSelectedGameObject(retryButton);
    }
}
