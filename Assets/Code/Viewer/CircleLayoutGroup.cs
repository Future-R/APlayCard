using UnityEngine;
using UnityEngine.UI;

/// <summary>扇形布局组件</summary>
/// <author>Danny Yan</author>
[AddComponentMenu("Layout/Circle Layout Group", 150)]
public class CircleLayoutGroup : LayoutGroup
{
    public enum LayoutMode
    {
        /// <summar>平均分布</summary>
        Hypodispersion = 0,
        /// <summar>扇形分布</summary>
        Sector = 1,
    }

    [Header("分布模式: Hypodispersion(圆形平均分布) Sector(扇形分布)")]
    public LayoutMode mode = LayoutMode.Hypodispersion;

    [Header("半径")]
    public float radius = 0;

    [Header("起始角度")]
    public float initAngle = 0;

    [Header("是否保持弧度值不变")]
    public bool keepRadLen = false;
    [Header("弧度保持不变的值(keepRadLen为true时有效)")]
    public float keepRadLenVal = 0f;
    [Header("扇形分布范围")]
    public float sectorAngle = 0;
    [Header("扇形分布时且keepRadLen为false时,是否中间对齐到扇形中心,否则两端对齐")]
    public bool sectorAlignCenter = false;
    [Header("扇形分布且sectorAlignCenter为false时,是否为逆时针")]
    public bool sectorClockwise = true;

    // public float fDistance;
    // [Range(0f, 360f)]
    // public float MinAngle, MaxAngle, StartAngle;
    protected override void OnEnable()
    {
        base.OnEnable();
        CalculateRadial();
    }
    public override void SetLayoutHorizontal()
    {
        // Util.Print("SetLayoutHorizontal");
        CalculateRadial();
    }
    public override void SetLayoutVertical()
    {
        // Util.Print("SetLayoutVertical");
        CalculateRadial();
    }
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        // Util.Print("CalculateLayoutInputHorizontal");
        CalculateRadial();
    }
    public override void CalculateLayoutInputVertical()
    {
        // Util.Print("CalculateLayoutInputVertical");
        CalculateRadial();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        CalculateRadial();
    }
#endif

    protected void CalculateRadial()
    {
        this.m_Tracker.Clear();
        if (transform.childCount == 0)
            return;

        if (this.mode == LayoutMode.Hypodispersion)
        {
            this.Hypodispersion();
        }
        else if (this.mode == LayoutMode.Sector)
        {
            this.Sector();
        }
    }

    /// 平均分布
    private void Hypodispersion()
    {
        // rectChildren来自父类,在CalculateLayoutInputHorizontal()中进行的非active和IgonreLayout的子节点剔除
        if (this.rectChildren.Count <= 0) return;

        float perRad = 2 * Mathf.PI / rectChildren.Count;
        // float perAngle = 360 / rectChildren.Count;
        float initRad = this.initAngle * Mathf.Deg2Rad;

        this.SetLayout(initRad, perRad);
    }

    /// 扇形分布
    private void Sector()
    {
        if (rectChildren.Count <= 0) return;

        float perRad = 0;
        float initRad = this.initAngle * Mathf.Deg2Rad;
        // 扇形弧度
        var sectorRad = this.sectorAngle * Mathf.Deg2Rad;

        if (this.keepRadLen)
        {
            // 弧长公式为: L = (angle * PI / 180) * radius = rad * radius, 要保持弧长不变,则需要改变rad即可, rad = L / radius
            perRad = keepRadLenVal / this.radius;
            if (sectorAlignCenter)
            {
                // 将initRad重置到sectorRad中线上
                initRad += (sectorClockwise ? sectorRad * .5f : -sectorRad * .5f);
                if (rectChildren.Count > 1)
                {
                    // 根据数量重置initRad
                    float _radOff = perRad * ((rectChildren.Count - 1) * 0.5f);
                    initRad -= (sectorClockwise ? _radOff : -_radOff);
                }
            }
            else
            {
                perRad = keepRadLenVal / this.radius;
            }
        }
        else
        {
            // 居中对齐到扇形中心
            if (sectorAlignCenter)
            {
                perRad = sectorRad / (rectChildren.Count + 1);
                initRad += sectorClockwise ? perRad : -perRad;
            }
            else
            {
                perRad = rectChildren.Count == 1 ? 0 : sectorRad / (rectChildren.Count - 1);
            }
        }

        if (!sectorClockwise)
        {
            perRad *= -1;
        }

        this.SetLayout(initRad, perRad);
    }

    private void SetLayout(float initRad, float perRad)
    {
        // 计算最佳大小
        float totalMin = 0;
        float totalPreferred = 0;
        float totalFlexible = 0;

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            var child = rectChildren[i];

            //禁用子节点recttransform相关属性
            m_Tracker.Add(this, child,
            DrivenTransformProperties.Anchors |
            DrivenTransformProperties.AnchoredPosition |
            DrivenTransformProperties.Pivot);

            var size = child.rect.size;
            child.pivot = new Vector2(0.5f, 0.5f);
            child.anchorMin = new Vector2(0.5f, 0.5f);
            child.anchorMax = new Vector2(0.5f, 0.5f);
            child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

            // child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -pSize.x*.5f, size.x);
            // child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -pSize.y*.5f, size.y);
            // child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left | RectTransform.Edge.Right, size.x*.5f, size.x);
            // child.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top | RectTransform.Edge.Bottom, size.y*.5f, size.y);

            // float min, preferred, flexible;
            // GetChildSizes(child, 0, out min, out preferred, out flexible);
            // size.x = LayoutUtility.GetMinSize(child, 0);
            // size.y = LayoutUtility.GetMinSize(child, 1);

            size *= 0.5f * child.localScale;
            Vector3 vPos = child.localPosition;
            var rad = initRad + perRad * i;
            vPos.x = this.radius * Mathf.Cos(rad);
            vPos.y = this.radius * Mathf.Sin(rad);
            child.localPosition = vPos;

            var left = vPos.x - size.x;
            if (left < minX) minX = left;
            var right = vPos.x + size.x;
            if (right > maxX) maxX = right;

            var bottom = vPos.y - size.y;
            if (bottom < minY) minY = bottom;
            var top = vPos.y + size.y;
            if (top > maxY) maxY = top;
        }

        // 此处宽高计算并不是很精确
        var w = Mathf.Abs(maxX - minX);
        var h = Mathf.Abs(maxY - minY);
        totalMin = this.radius;
        totalPreferred = w;
        if (this.mode == LayoutMode.Sector)
        {
            // totalPreferred += radius;
        }
        SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, 0);
        totalPreferred = h;
        if (this.mode == LayoutMode.Sector)
        {
            // totalPreferred += radius;
        }
        SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, 1);
    }
}
