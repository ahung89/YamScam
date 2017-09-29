using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomablePanel : MonoBehaviour {

    public RectTransform zoomSource;
    public float growthPerFrame = 1.25f;

    private RectTransform rt;
    private Vector2 smallScale;
    private Rect canvasRect;
    private CanvasGroup canvasGroup;

    private void Awake ()
    {
        rt = GetComponent<RectTransform>();
        canvasRect = zoomSource.GetComponentInParent<Canvas>().pixelRect;
        canvasGroup = GetComponent<CanvasGroup>();

        smallScale = new Vector2(zoomSource.rect.width / rt.rect.width, zoomSource.rect.height / rt.rect.height);
        rt.localScale = smallScale;
        rt.position = zoomSource.position;
        rt.pivot = FindPivot();

        // Changing the pivot automatically moves the box, so re-adjust the position.
        rt.position = (Vector2)zoomSource.position + (new Vector2(rt.pivot.x * rt.rect.width * rt.lossyScale.x, rt.pivot.y * rt.rect.height * rt.lossyScale.y)
            - new Vector2(rt.rect.width * rt.lossyScale.x / 2, rt.rect.height * rt.lossyScale.y / 2));

        SetPanelInteractable(false);
    }

    Vector2 FindPivot()
    {
        float rectWidth = rt.rect.width * rt.localScale.x;
        float rectHeight = rt.rect.height * rt.localScale.y;
        float rectExtentX = rectWidth * .5f;
        float rectExtentY = rectHeight * .5f;

        RectTransform parentRt = rt.transform.parent.GetComponent<RectTransform>();
        Vector2 localPoint = (Vector2)parentRt.InverseTransformPoint(rt.transform.position)
            + new Vector2(parentRt.rect.width / 2f, parentRt.rect.height / 2f);

        Vector2 innerSize = new Vector2(rectWidth, rectHeight);
        Vector2 outerSize = parentRt.rect.size;
        Vector2 innerMax = new Vector2(localPoint.x + rectExtentX,
            localPoint.y + rectExtentY);
        Vector2 outerMax = parentRt.rect.size;

        float x = (innerSize.x * outerMax.x - outerSize.x * innerMax.x) / (innerSize.x - outerSize.x);
        float y = (innerSize.y * outerMax.y - outerSize.y * innerMax.y) / (innerSize.y - outerSize.y);

        return new Vector2((x - (localPoint.x - rectExtentX)) / rectWidth, (y - (localPoint.y - rectExtentY)) / rectHeight);
    }

    public void ZoomIn()
    {
        StartCoroutine(Zoom());
        SetPanelInteractable(true);
    }

    public void ZoomOut()
    {
        StartCoroutine(Zoom(false));
        SetPanelInteractable(false);
    }

    IEnumerator Zoom(bool zoomIn = true)
    {
        Vector2 lowerBound = zoomIn ? Vector2.zero : smallScale;

        while ((Vector2)rt.localScale != (zoomIn ? Vector2.one : smallScale))
        {
            float growth = Time.deltaTime * growthPerFrame * (zoomIn ? 1 : -1);
            rt.localScale = new Vector3(Mathf.Clamp(rt.localScale.x + growth, lowerBound.x, 1), Mathf.Clamp(rt.localScale.y + growth, lowerBound.y, 1));
            yield return null;
        }
    }

    void SetPanelInteractable (bool enabled)
    {
        canvasGroup.interactable = enabled;
        canvasGroup.blocksRaycasts = enabled;
    }
}
