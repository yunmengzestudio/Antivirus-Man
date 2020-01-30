﻿using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class FluentTextPanel : MonoBehaviour
{
    //更改的字体目标
    public Text TargetText;
    //隐藏的text组件，只用于确定panel需要变换的大小，后期改为自动生成
    private Text HiddenText;

    [Header("PanelSetting")]
    public ContentSizeFitter.FitMode HorizontalFit;
    public ContentSizeFitter.FitMode VerticalFit;
    public float MaxWidth = 400;
    public float ExtraWidth = 30;
    public float ExtraHeight = 30;
    public float TimePerWord = 0.08f;

    private Vector2 TargetSize
    {
        get { return GetComponent<RectTransform>().sizeDelta; }
        set { GetComponent<RectTransform>().sizeDelta = value; }
    }

    private void Awake()
    {
        //生成隐藏的Text
        HiddenText = Instantiate(TargetText.gameObject, this.transform).GetComponent<Text>();
        HiddenText.gameObject.name = "HiddenText";
        HiddenText.color = new Color(0, 0, 0, 0);
        var sizeFitter = HiddenText.gameObject.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = HorizontalFit;
        sizeFitter.verticalFit = VerticalFit;

        //保证以开始为缩小状态不可见
        GetComponent<RectTransform>().DOScale(0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
    }



    /// <summary>
    /// 更新panel中的显示的文字
    /// </summary>
    /// <param name="word"></param>
    public void ChangeWord(string word)
    {
        StartCoroutine(StartChangeWord(word));
    }

    public void Show()
    {
        Sequence se = DOTween.Sequence();
        se.Append(GetComponent<RectTransform>().DOScale(1.0f, 0.25f));
        GetComponent<CanvasGroup>().DOFade(1, 0.1f);
    }

    public void Hide()
    {
        Sequence se = DOTween.Sequence();
        se.Append(GetComponent<RectTransform>().DOScale(1.1f, 0.06f)).Append(GetComponent<RectTransform>().DOScale(0f, 0.25f));
        GetComponent<CanvasGroup>().DOFade(0, 0.25f);
    }

    /// <summary>
    /// 传入所需要显示的话并更新panel大小
    /// </summary>
    /// <param name="changeWord"></param>
    /// <returns></returns>
    IEnumerator StartChangeWord(string Word)
    {
        string changeWord = Word.Replace("\\n", "\n");
        TargetText.text = "";
        HiddenText.text = changeWord;
        RefreshSize();
        yield return new WaitForSeconds(0.3f);
        float textTime = changeWord.Length * TimePerWord;
        TargetText.DOText(changeWord, textTime).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 刷新文字panel以及文字本身大小
    /// </summary>
    private void RefreshSize()
    {
        var textPanelSize = GetPreferredSize(HiddenText.gameObject);
        //隐藏panel的原大小
        var rawPanelSize = textPanelSize;

        //增加边框余地
        textPanelSize += new Vector2(ExtraWidth, ExtraHeight);
        //增加随机大小
        textPanelSize += new Vector2(Random.Range(0, 50), Random.Range(0, 50));
        //刷新显示文字大小
        TargetText.GetComponent<RectTransform>().sizeDelta = rawPanelSize;
        //让文字外框做一个变形动画
        DOTween.To(() => TargetSize, x => TargetSize = x, textPanelSize, 0.3f);

    }

    /// <summary>
    /// 立即获取ContentSizeFitter的区域
    /// </summary>
    private Vector2 GetPreferredSize(GameObject obj)
    {
        //刷新网格
        LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());

        RefreshPanelSize(obj);

        return new Vector2(HandleSelfFittingAlongAxis(0, obj), HandleSelfFittingAlongAxis(1, obj));
    }

    /// <summary>
    /// 获取隐藏panel的宽或高
    /// </summary>
    private float HandleSelfFittingAlongAxis(int axis, GameObject obj)
    {
        //获取当前隐藏panel选择的适配模式
        ContentSizeFitter.FitMode fitting = (axis == 0 ? obj.GetComponent<ContentSizeFitter>().horizontalFit : obj.GetComponent<ContentSizeFitter>().verticalFit);
        if (fitting == ContentSizeFitter.FitMode.MinSize)
        {
            return LayoutUtility.GetMinSize(obj.GetComponent<RectTransform>(), axis);
        }
        else if (fitting == ContentSizeFitter.FitMode.PreferredSize)
        {
            return LayoutUtility.GetPreferredSize(obj.GetComponent<RectTransform>(), axis);
        }
        else
        {
            //如果是unconstrained的话直接获取当前的大小
            return axis == 0 ? obj.GetComponent<RectTransform>().sizeDelta.x : obj.GetComponent<RectTransform>().sizeDelta.y;
        }
    }

    /// <summary>
    /// 刷新隐藏文字和显示文字的宽，如果偏大就设置为最大值，如果偏小就缩小为合适的宽度
    /// </summary>
    private void RefreshPanelSize(GameObject obj)
    {
        if (LayoutUtility.GetPreferredSize(obj.GetComponent<RectTransform>(), 0) < MaxWidth)
        {
            var origin = obj.GetComponent<RectTransform>().sizeDelta;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(LayoutUtility.GetPreferredSize(obj.GetComponent<RectTransform>(), 0), origin.y);
            TargetText.GetComponent<RectTransform>().sizeDelta = new Vector2(LayoutUtility.GetPreferredSize(obj.GetComponent<RectTransform>(), 0), origin.y);
        }
        else
        {
            var origin = obj.GetComponent<RectTransform>().sizeDelta;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(MaxWidth, origin.y);
            TargetText.GetComponent<RectTransform>().sizeDelta = new Vector2(MaxWidth, origin.y);
        }
    }
}
