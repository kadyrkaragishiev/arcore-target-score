using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
public class TriggerWork : MonoBehaviour
{
    public Text progressText;
    public ARTrackedImageManager ARTrackedImageManager;
    public ARRaycastManager ARRaycastManager;
    public ARPlaneManager ARPlaneManager;
    public XRReferenceImageLibrary xRReferenceImages;
    public int PlaneCheckSteps = 5;
    public float CombineContoursDelay = 3f;
    public float ManulModeTimeOut;
    public float PlaneCheckProgress;
    private Vector2 touchPosition = new Vector2(Screen.width / 2f, Screen.height/2f);

    public void StartScanSurface()
    {
        ARRaycastManager.enabled = true;
        ARPlaneManager.enabled = true;
        StartCoroutine(ScanSurface());  
    }
    private IEnumerator ScanSurface()
    {
        yield return new WaitForSeconds(1f);

        var currentSteps = 0;
        var hits = new List<ARRaycastHit>();


        while (currentSteps < PlaneCheckSteps)
        {
            var b = ARRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);
            if (b) currentSteps += 1;
            PlaneCheckProgress = (float)currentSteps / PlaneCheckSteps;
            progressText.text = "Готово на" + PlaneCheckProgress.ToString();
            yield return new WaitForSeconds(1f);
        }
        yield return CombineContours();
    }
    private IEnumerator CombineContours()
    {
        while (ARTrackedImageManager.referenceLibrary == null)
        {
            yield return null;
        }

        ARTrackedImageManager.referenceLibrary = xRReferenceImages;
        ARTrackedImageManager.enabled = true;

        yield return new WaitForSeconds(CombineContoursDelay);

        var time = 0f;


        while (ARTrackedImageManager.trackables.count == 0)
        {
            time += Time.deltaTime;

            if (time > ManulModeTimeOut)
            {
                yield break;
            }

            yield return null;
        }

        yield return ShowContent();
    }
    private IEnumerator ShowContent()
    {
        var hits = new List<ARRaycastHit>();

        while (!ARRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            yield return null;


        var hitPose = hits[0].pose;
    }
}
