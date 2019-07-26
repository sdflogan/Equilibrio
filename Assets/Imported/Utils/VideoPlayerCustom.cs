using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerCustom : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource AudioSource;
    public Image loadImage;

    private void Start()
    {
        StartCoroutine(PlayVideo());
    }

    private IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.1f);
        }
        loadImage.gameObject.SetActive(false);
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}