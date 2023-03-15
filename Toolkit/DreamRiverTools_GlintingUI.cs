using UnityEngine;
using UnityEngine.UI;

public class DreamRiverTools_GlintingUI : MonoBehaviour
{
    [Header("是否自动开启闪耀")] public bool AutoGlinting = true;
    [Header("闪耀的颜色")] public Color GlintingColor = new(1, 0, 0, 1);
    [Header("延时闪耀的时间")] public float delayGlintingTime;
    [Header("闪耀时长。当值为负数时，循环闪耀")] public float GlintingTime = -1;
    [Header("闪动频率")] public float frequency = 0.7f;

    private bool _isFirst;
    private bool _isTab;
    private Image[] _imas;
    private Color[] _imasColor;
    private Text[] _texts;
    private Color[] _textsColor;

    private void Start()
    {
        if (AutoGlinting)
            Invoke(nameof(StartGlinting), delayGlintingTime);
        if (GlintingTime > 0)
            Invoke(nameof(StopGlinting), delayGlintingTime + GlintingTime);

        _imas = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<Text>();

        //记录初始颜色
        if (_imas != null)
        {
            _imasColor = new Color[_imas.Length];
            for (int i = 0; i < _imas.Length; i++)
            {
                _imasColor[i] = new Color(_imas[i].color.r, _imas[i].color.g, _imas[i].color.b, _imas[i].color.a);
            }
        }

        if (_texts != null)
        {
            _textsColor = new Color[_texts.Length];
            for (int i = 0; i < _texts.Length; i++)
            {
                _textsColor[i] = new Color(_texts[i].color.r, _texts[i].color.g, _texts[i].color.b, _texts[i].color.a);
            }
        }
    }

    private void Update()
    {
        if (_isFirst)
        {
            if (_isTab == false)
            {
                if (_imas != null)
                {
                    foreach (Image ima in _imas)
                    {
                        ima.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b,
                            GlintingColor.a -= Time.deltaTime * frequency);
                    }
                }

                if (_texts != null)
                {
                    foreach (Text tex in _texts)
                    {
                        tex.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b,
                            GlintingColor.a -= Time.deltaTime * frequency);
                    }
                }

                if (GlintingColor.a <= 0.3f)
                {
                    _isTab = true;
                }
            }

            if (_isTab)
            {
                if (_imas != null)
                {
                    foreach (Image ima in _imas)
                    {
                        ima.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b,
                            GlintingColor.a += Time.deltaTime * frequency);
                    }
                }

                if (_texts != null)
                {
                    foreach (Text tex in _texts)
                    {
                        tex.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b,
                            GlintingColor.a += Time.deltaTime * frequency);
                    }
                }

                if (GlintingColor.a >= 1)
                {
                    _isTab = false;
                }
            }
        }
    }

    public void StartGlinting()
    {
        _isFirst = true;
    }

    public void StopGlinting()
    {
        #region 还原初始颜色

        //若数组没初始化成功，也不会有数组的长度
        if (_imas != null)
        {
            for (int i = 0; i < _imas.Length; i++)
            {
                _imas[i].color = _imasColor[i];
            }
        }

        if (_texts != null)
        {
            for (int i = 0; i < _texts.Length; i++)
            {
                _texts[i].color = _textsColor[i];
            }
        }

        #endregion

        _isFirst = false;
    }
}