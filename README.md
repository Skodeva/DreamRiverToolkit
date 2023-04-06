# 一、修改默认字体

## 1、修改Text默认字体

✅、打开该路径下的脚本：

E:\WorkSoft\Unity\2021.3.6f1\Editor\Data\Resources\PackageManager\BuiltInPackages\com.unity.ugui\Runtime\UI\Core\Text.cs 

✅、将 AssignDefaultFontIfNecessary 方法改为如下所示：

```c#
internal void AssignDefaultFontIfNecessary()
{
    if (font == null)
    {
        font = Resources.Load<Font>("Font/MainFont");
        
        if(font==null)
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    }
}
```

✅、若你已经创建了工程，还需删掉工程Library\PackageCache\com.unity.ugui@1.0.0，重新打开工程。



## 2、修改Doozy的默认TMP字体

Hierarchy右键：DreamRiver / 切换Doozy的TMP字体

若未完成切换，则在unity运行状态中再点一次试试。



# 二、优化该插件大小的方法

若未使用如下内容，删掉Resources即可。

- 默认字体不是 Resources 下的 MainFont
- Doozy或者其他TMP未使用 Resources 下的 MainFont SDF
