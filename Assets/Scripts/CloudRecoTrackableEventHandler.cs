/*===============================================================================
Copyright (c) 2015 PTC Inc. All Rights Reserved.
 
Copyright (c) 2010-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class CloudRecoTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PRIVATE_MEMBERS
    private TrackableBehaviour mTrackableBehaviour;
    float timer = 0;
    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    void Update() {
        timer += Time.deltaTime;
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
        else if (previousStatus == TrackableBehaviour.Status.UNKNOWN &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            // Ignore this specific combo
            return;
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
        switch (mTrackableBehaviour.Trackable.Name) {

            case "h5-109-1.jpg":
            case "h5-109-2.jpg":
            case "h5-109-3.jpg":
                OpenLink("http://h5property.com//tour/listings/DE2359940/h5tour.html");
                break;
            case "h5-109-4.jpg":
            case "h5-109-5.jpg":
            case "h5-109-6.jpg":
                OpenLink("http://h5property.com/tour/listings/DE1873993/tour.html");
                break;
            case "h5-109-7.jpg":
            case "h5-109-8.jpg":
            case "h5-109-9.jpg":
                OpenLink("https://www.privatmegleren.no/panorama/38160301/nordstrand-ljan-koselig-romantisk-og-familievennlig-enebolig-frodig-blomsterhage-2-3-s-rom-noe-moderniseringsbehov%23gallerimobil#solgt");
                break;

            case "teknotomta_front.jpg":
            
                OpenLink("http://byparkensyd.no");
                break;
            case "Screen-Shot-2016-09-17.jpg":
            
                OpenLink("http://one.h5property.com/areas/2");
                break;
            default:
                OpenLink("http://h5property.com/tour/stormcam/Margrethe%20Thomsetsvei%209/h5tour.html");
                break;
        }
        
    }

    private void OnTrackingLost()
    {
        
    }
    void OpenLink(string url)
    {
        if (timer > 0.5f)
        {
            timer = 0;
            Application.OpenURL(url);

        }
    }
    #endregion //PRIVATE_METHODS
}
