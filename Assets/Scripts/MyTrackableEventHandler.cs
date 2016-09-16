/*============================================================================== 
 * Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class MyTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PRIVATE_MEMBERS
    private TrackableBehaviour mTrackableBehaviour;
    private bool mHasBeenFound = false;
    private bool mLostTracking;
    private float mSecondsSinceLost;
    #endregion // PRIVATE_MEMBERS

    public GameObject threeDLogo;
    public GameObject house;
    float timer = 0;
 //   LineMoving line;
    GameObject effects;
    Animator anim;
    public Text info;
    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        //       line = GameObject.Find("Canvas").transform.FindChild("Image").GetComponent<LineMoving>();
//        info = GameObject.Find("Canvas").transform.FindChild("Text").GetComponent<Text>();
        anim = GameObject.Find("Canvas").transform.FindChild("Image").GetComponent<Animator>();
        effects = GameObject.Find("ARCamera").transform.FindChild("Camera").transform.FindChild("Flashes").gameObject;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        OnTrackingLost();
    }

    void Update()
    {
        timer += Time.deltaTime;
        //string text;
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("moving")) text = "moving";
        //else if (anim.GetCurrentAnimatorStateInfo(0).IsName("stay")) text = "stay";
        //else text = "no";
        //       info.text = text;
        // Pause the video if tracking is lost for more than two seconds
        if (mHasBeenFound && mLostTracking)
        {
            if (mSecondsSinceLost > 2.0f)
            {
                VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour>();
                if (video != null &&
                    video.CurrentState == VideoPlayerHelper.MediaState.PLAYING)
                {
                    video.VideoPlayer.Pause();
                }

                mLostTracking = false;
            }

            mSecondsSinceLost += Time.deltaTime;
        }
    }

    #endregion //MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }
    #endregion //PUBLIC_METHODS


    #region PRIVATE_METHODS
    private void OnTrackingFound()
    {
        StopFindAnimation();
        switch (gameObject.name) {
            case "3DTarget":
                threeDLogo.SetActive(true);
                break;
            case "HouseTarget":
                house.SetActive(true);
                house.GetComponent<Collider>().enabled = true;
                info.text = "Click ok";
                break;
            case "LinkTarget":
                OpenLink("http://h5property.com/tour/listings/DE2261654/h5tour.html");
                break;
            case "Link1":
                OpenLink("http://www.privatmegleren.no/aveny/2160238/vika-nydelig-lys-og-klassisk-2-3-toppleilighet-med-solrik-balkong-7-5-kvm-rundovn-og-parkering-etter-annsiennitet#");
                break;
            case "Link2":
                OpenLink("http://m.finn.no/realestate/homes/ad.html?finnkode=79118558");
                break;
            case "VideoTarget":
            case "VideoTarget2":
            case "VideoTarget3":
            case "VideoTarget4":
                ShowVideoOnPlane();
                break;
            case "360":
                OpenLink("http://h5property.com/tour/listings/CSG3494163/tour.html");
                break;
            case "Link3":
                OpenLink("http://h5property.com/Demo/BAM%20Version%20V/h5tour.html");
                break;
            case "SimpleVideo":
                if (timer > 0.5f)
                {
                    timer = 0;
                    Handheld.PlayFullScreenMovie("486533413simple.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
                }
                break;
        }      
    }

    void OpenLink(string url) {
        if (timer > 0.5f)
        {
            timer = 0;
            Application.OpenURL(url);
           
        }     
    }

    void ShowVideoOnPlane() {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();
        Collider[] colliderComponents = GetComponentsInChildren<Collider>();

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

        // Optionally play the video automatically when the target is found

        VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour>();
        if (video != null && video.AutoPlay)
        {
            if (video.VideoPlayer.IsPlayableOnTexture())
            {
                VideoPlayerHelper.MediaState state = video.VideoPlayer.GetStatus();
                if (state == VideoPlayerHelper.MediaState.PAUSED ||
                    state == VideoPlayerHelper.MediaState.READY ||
                    state == VideoPlayerHelper.MediaState.STOPPED)
                {
                    // Pause other videos before playing this one
                    PauseOtherVideos(video);

                    // Play this video on texture where it left off
                    video.VideoPlayer.Play(false, 0);
                }
                else if (state == VideoPlayerHelper.MediaState.REACHED_END)
                {
                    // Pause other videos before playing this one
                    PauseOtherVideos(video);

                    // Play this video from the beginning
                    video.VideoPlayer.Play(false, 0);
                }
            }
        }

        mHasBeenFound = true;
        mLostTracking = false;
    }

    void StartFindAnimation() {
        GameObject.Find("Canvas").transform.FindChild("Image").GetComponent<Animator>().SetTrigger("move");
//        anim.gameObject.SetActive(true);
//        anim.SetTrigger("move");
 //       line.StartMoving();
        effects.SetActive(true);
    }

    void StopFindAnimation() {
        anim.SetTrigger("stay");
 //       line.StopMoving();
        effects.SetActive(false);
    }

    private void OnTrackingLost()
    {
        StartFindAnimation();
        if (threeDLogo!=null) threeDLogo.SetActive(false);
        if (house != null) house.SetActive(false);
        //        webWiev.SetActive(false);
        //VideoPlaybackBehaviour video = GetComponentInChildren<VideoPlaybackBehaviour>();
        //if (video.CurrentState == VideoPlayerHelper.MediaState.PLAYING)
        //{
        //    video.VideoPlayer.Stop();
        //}
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();
        Collider[] colliderComponents = GetComponentsInChildren<Collider>();

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

        mLostTracking = true;
        mSecondsSinceLost = 0;
    }

    // Pause all videos except this one
    private void PauseOtherVideos(VideoPlaybackBehaviour currentVideo)
    {
        VideoPlaybackBehaviour[] videos = (VideoPlaybackBehaviour[])
                FindObjectsOfType(typeof(VideoPlaybackBehaviour));

        foreach (VideoPlaybackBehaviour video in videos)
        {
            if (video != currentVideo)
            {
                if (video.CurrentState == VideoPlayerHelper.MediaState.PLAYING)
                {
                    video.VideoPlayer.Pause();
                }
            }
        }
    }
    #endregion //PRIVATE_METHODS
}
