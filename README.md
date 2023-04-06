# 一、修改默认字体

1、打开该路径下的脚本：

E:\WorkSoft\Unity\2021.3.6f1\Editor\Data\Resources\PackageManager\BuiltInPackages\com.unity.ugui\Runtime\UI\Core\Text.cs 

2、将 AssignDefaultFontIfNecessary 方法改为如下所示：

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

3、若你已经创建了工程，还需删掉工程Library\PackageCache\com.unity.ugui@1.0.0，重新打开工程。
