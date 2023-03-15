using LitJson;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DreamRiverTools_CommonSDKJsonDes : MonoBehaviour
{
    #region QQ

    /// <summary>
    /// 输入登陆QQ授权回调的：MiniJSON序列化后的result，返回序列化结果。
    /// </summary>
    public static QQAuthInfo QQ_AuthInfo(string str)
    {
        JObject g = JObject.Parse(str);

        var value = new QQAuthInfo();
        value.expiresIn = g["expiresIn"].ToString();
        value.secret = g["secret"].ToString();
        value.nickname = g["nickname"].ToString();
        value.gender = g["gender"].ToString();
        value.icon = g["icon"].ToString();
        value.pay_token = g["pay_token"].ToString();
        value.unionid = g["unionid"].ToString();
        value.iconQzone = g["iconQzone"].ToString();
        value.pfkey = g["pfkey"].ToString();
        value.pf = g["pf"].ToString();
        value.secretType = g["secretType"].ToString();
        value.userID = g["userID"].ToString();
        value.expiresTime = g["expiresTime"].ToString();
        value.token = g["token"].ToString();

        return value;
    }

    /// <summary>
    /// 输入查询QQ信息返回的：MiniJSON序列化后的result，返回序列化结果。
    /// </summary>
    public QQInfo QQ_Info(string str)
    {
        JObject g = JObject.Parse(str);

        var value = new QQInfo();
        value.nickname = g["nickname"].ToString();
        value.gender = g["gender"].ToString();
        value.province = g["province"].ToString();
        value.city = g["city"].ToString();
        value.year = g["year"].ToString();
        value.figureurl = g["figureurl"].ToString();
        value.figureurl_1 = g["figureurl_1"].ToString();
        value.figureurl_2 = g["figureurl_2"].ToString();
        value.figureurl_qq = g["figureurl_qq"].ToString();
        value.figureurl_qq_1 = g["figureurl_qq_1"].ToString();
        value.figureurl_qq_2 = g["figureurl_qq_2"].ToString();

        return value;
    }

    public class QQInfo
    {
        /// <summary>
        /// QQ昵称
        /// </summary>
        public string nickname;

        /// <summary>
        /// 性别
        /// </summary>
        public string gender;

        /// <summary>
        /// 所在省份
        /// </summary>
        public string province;

        /// <summary>
        /// 所在城市
        /// </summary>
        public string city;

        /// <summary>
        /// 出生年份
        /// </summary>
        public string year;

        /// <summary>
        /// 30x30的QQ头像
        /// </summary>
        public string figureurl;

        /// <summary>
        /// 50x50的QQ头像
        /// </summary>
        public string figureurl_1;

        /// <summary>
        /// 100x100的QQ头像
        /// </summary>
        public string figureurl_2;

        /// <summary>
        /// 140x140的QQ头像
        /// </summary>
        public string figureurl_qq;

        /// <summary>
        /// 40x40的QQ头像
        /// </summary>
        public string figureurl_qq_1;

        /// <summary>
        /// 100x100的QQ头像
        /// </summary>
        public string figureurl_qq_2;
    }

    public class QQAuthInfo
    {
        /// <summary>
        /// 凭证有效时间
        /// </summary>
        public string expiresIn;

        /// <summary>
        /// 第三方用户唯一凭证密钥，即appsecret
        /// </summary>
        public string secret;

        /// <summary>
        /// QQ昵称
        /// </summary>
        public string nickname;

        /// <summary>
        /// 性别
        /// </summary>
        public string gender;

        /// <summary>
        /// QQ头像
        /// </summary>
        public string icon;

        /// <summary>
        /// 从手Q登录态中获取的pay_token的值
        /// </summary>
        public string pay_token;

        public string unionid;

        /// <summary>
        /// QQ空间头像
        /// </summary>
        public string iconQzone;

        /// <summary>
        /// 登录时候获取，跟平台来源和openkey根据规则生成的一个密钥串。
        /// </summary>
        public string pfkey;

        /// <summary>
        /// 平台标识信息：平台-注册渠道-系统运行平台-安装渠道-业务自定义，如果业务没有自定义,格式可以为平台-渠道-操作系统。
        /// </summary>
        public string pf;

        public string secretType;

        /// <summary>
        /// 用户的openid。OpenID是此网站上或应用中唯一对应用户身份的标识
        /// 网站或应用可将此ID进行存储，便于用户下次登录时辨识其身份，或将其与用户在网站上或应用中的原有账号进行绑定。
        /// </summary>
        public string userID;

        /// <summary>
        /// 过期时间
        /// </summary>
        public string expiresTime;

        /// <summary>
        /// Token认证
        /// https://baijiahao.baidu.com/s?id=1593244938986076867&wfr=spider&for=pc
        /// </summary>
        public string token;
    }

    #endregion

    #region 百度语音返回结果解析

    //返回结果：https://blog.csdn.net/weixin_38239050/article/details/97235809

    /// <summary>
    /// 获得百度语音识别结果
    /// </summary>
    public string BaiDu_VoiceInfo(string originStr)
    {
        string str = originStr.Replace("[", "").Replace("]", "");

        BaiDuVoiceInfo baiDuVoiceInfo = JsonMapper.ToObject<BaiDuVoiceInfo>(str);

        return baiDuVoiceInfo.best_result;
    }

    private class BaiDuVoiceInfo
    {
        public string results_recognition;
        public string result_type;
        public string best_result;
        public BaiDuOriginResult BaiDuOriginResult;
        public int error;
    }

    private class BaiDuOriginResult
    {
        public long corpus_no;
        public int err_no;
        public Dictionary<string, string> result;
        public string sn;
    }

    #endregion
}