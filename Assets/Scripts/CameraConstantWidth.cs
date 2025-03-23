using UnityEngine;

public class CameraConstantWidth : MonoBehaviour
{
    public Vector2 DefaultResolution = new Vector2(720, 1280);
    [Range(0f, 1f)] public float WidthOrHeight = 0;

    [Range(1f, 5f)] public float Plus = 1f;

    private Camera componentCamera;
    
    private float initialSize;
    private float targetAspect;

    private void Start()
    {
        componentCamera = GetComponent<Camera>();
        initialSize = componentCamera.orthographicSize;
        targetAspect = DefaultResolution.x / DefaultResolution.y;
        
    }

    private void Update()
    {
        if (componentCamera.orthographic)
        {
            float constantWidthSize = Plus + initialSize * (targetAspect / componentCamera.aspect);
            componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, initialSize, WidthOrHeight);
        }
    }
}