using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DreamRiverTools_GlintingUI : MonoBehaviour
{
    [Header("是否自动开启闪耀")] public bool autoStart;
    [Header("闪耀的颜色")] public Color glintingColor = new(1, 0, 0, 1);
    [Header("闪动频率")] public float frequency = 0.7f;

    private bool _isTab;
    private Image[] _imageChilds;
    private Color[] _imasColor;
    private Text[] _textChilds;
    private Color[] _textsColor;

    /// <summary>
    /// 闪烁时长
    /// </summary>
    private float? _glintingTime;

    private void Start()
    {
        if (autoStart)
            StartGlinting();

        _imageChilds = GetComponentsInChildren<Image>();
        _textChilds = GetComponentsInChildren<Text>();

        RecordRawData();
    }

    public void StartGlinting(float? time = null)
    {
        _glintingTime = time;
        StartCoroutine(Glinting());
    }

    private void RecordRawData()
    {
        if (_imageChilds != null)
        {
            _imasColor = new Color[_imageChilds.Length];
            for (int i = 0; i < _imageChilds.Length; i++)
            {
                _imasColor[i] = new Color(_imageChilds[i].color.r, _imageChilds[i].color.g, _imageChilds[i].color.b,
                    _imageChilds[i].color.a);
            }
        }

        if (_textChilds != null)
        {
            _textsColor = new Color[_textChilds.Length];
            for (int i = 0; i < _textChilds.Length; i++)
            {
                _textsColor[i] = new Color(_textChilds[i].color.r, _textChilds[i].color.g, _textChilds[i].color.b,
                    _textChilds[i].color.a);
            }
        }
    }

    private IEnumerator Glinting()
    {
        while (true)
        {
            if (_isTab == false)
            {
                if (_imageChilds != null)
                {
                    foreach (Image ima in _imageChilds)
                    {
                        ima.color = new Color(glintingColor.r, glintingColor.g, glintingColor.b,
                            glintingColor.a -= Time.deltaTime * frequency);
                    }
                }

                if (_textChilds != null)
                {
                    foreach (Text tex in _textChilds)
                    {
                        tex.color = new Color(glintingColor.r, glintingColor.g, glintingColor.b,
                            glintingColor.a -= Time.deltaTime * frequency);
                    }
                }

                if (glintingColor.a <= 0.3f)
                {
                    _isTab = true;
                }
            }

            if (_isTab)
            {
                if (_imageChilds != null)
                {
                    foreach (Image ima in _imageChilds)
                    {
                        ima.color = new Color(glintingColor.r, glintingColor.g, glintingColor.b,
                            glintingColor.a += Time.deltaTime * frequency);
                    }
                }

                if (_textChilds != null)
                {
                    foreach (Text tex in _textChilds)
                    {
                        tex.color = new Color(glintingColor.r, glintingColor.g, glintingColor.b,
                            glintingColor.a += Time.deltaTime * frequency);
                    }
                }

                if (glintingColor.a >= 1)
                {
                    _isTab = false;
                }
            }

            yield return null;

            if (_glintingTime != null)
            {
                _glintingTime -= Time.deltaTime;
                if (_glintingTime <= 0)
                {
                    StopGlinting();
                    yield break;
                }
            }
        }
    }

    public void StopGlinting()
    {
        //若数组没初始化成功，也不会有数组的长度
        if (_imageChilds != null)
        {
            for (int i = 0; i < _imageChilds.Length; i++)
            {
                _imageChilds[i].color = _imasColor[i];
            }
        }

        if (_textChilds != null)
        {
            for (int i = 0; i < _textChilds.Length; i++)
            {
                _textChilds[i].color = _textsColor[i];
            }
        }
    }
}