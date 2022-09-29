using UnityEngine;
using UnityEngine.UI;

//不会产生dc的透明射线目标
public class NoDrawGraphic : Graphic
{
    protected override void Awake()
    {
        canvasRenderer.cullTransparentMesh = true;
        color = UnityEngine.Color.clear;
    }
    //不加入渲染列队
    public override void Rebuild(CanvasUpdate update)
    {
        return;
    }


#if UNITY_EDITOR
    [SerializeField,Tooltip("是否显示Rect")]
    public bool isShowRect = false;
    [SerializeField, Tooltip("选中时是否显示Rect")]
    public bool isShowRectSelected = true;
    [SerializeField, Tooltip("显示的Rect颜色")]
    public Color rectColor = new Color(0.8f, 0.1f, 0.1f, 0.3f);
    private Texture2D _tex2D;

    protected override void OnValidate()
    {
        if (_tex2D == null)
        {
            _tex2D = new Texture2D(1, 1);
            _tex2D.wrapMode = TextureWrapMode.Repeat;
        }
        _tex2D.SetPixel(0, 0, rectColor);
        _tex2D.Apply();
    }
    public void OnDrawGizmos()
    {
        if (!isShowRect)
            return;
        if (_tex2D == null)
        {
            _tex2D = new Texture2D(1, 1);
            _tex2D.wrapMode = TextureWrapMode.Repeat;
            _tex2D.SetPixel(0, 0, rectColor);
            _tex2D.Apply();
        }
        Vector2 worldPos = RectTransformUtility.WorldToScreenPoint(this.canvas.worldCamera, transform.TransformPoint(new Vector3(rectTransform.rect.x, rectTransform.rect.y, 0)));
        Rect rect = new Rect(worldPos.x, worldPos.y, rectTransform.rect.width, rectTransform.rect.height);
        Gizmos.DrawGUITexture(rect, _tex2D);
    }
    public void OnDrawGizmosSelected()
    {
        if (isShowRect||!isShowRectSelected)
            return;
        if (UnityEditor.Selection.activeGameObject != this.gameObject)
            return;
        if (_tex2D == null)
        {
            _tex2D = new Texture2D(1, 1);
            _tex2D.wrapMode = TextureWrapMode.Repeat;
            _tex2D.SetPixel(0, 0, rectColor);
            _tex2D.Apply();
        }
        Vector2 worldPos = RectTransformUtility.WorldToScreenPoint(this.canvas.worldCamera, transform.TransformPoint(new Vector3(rectTransform.rect.x, rectTransform.rect.y, 0)));
        Rect rect = new Rect(worldPos.x, worldPos.y, rectTransform.rect.width, rectTransform.rect.height);
        Gizmos.DrawGUITexture(rect, _tex2D);
    }
#endif
}