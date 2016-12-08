/**
 *     Copyright 2015-2016 GetSocial B.V.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;
using System;
using GetSocialSdk.Core;
using UnityEngine;
using System.Runtime.InteropServices;

public class KakaoInvitePlugin : IInvitePlugin
{
    public const string ProviderName = "kakao";

    public bool IsAvailableForDevice()
    {
#if UNITY_ANDROID
        return IsKakaoInstalledAndroid();
#elif UNITY_IOS
        return _isKakaoInstalled();
#else
        return false;
#endif
    }

    #if UNITY_ANDROID
    bool IsKakaoInstalledAndroid()
    {
        try
        {
            using (AndroidJavaObject packageManager = AndroidUtils.Activity.Call<AndroidJavaObject>("getPackageManager"))
            {
                using (AndroidJavaObject result = packageManager.Call<AndroidJavaObject>("getPackageInfo", "com.kakao.talk", 0))
                {
                }
            }
            return true;
        }
        catch (Exception e)
        {
            if (Debug.isDebugBuild)
            {
                Debug.LogWarning("There was an error checking if KAKAOTALK is installed :" + e.Message);
                Debug.LogException(e);
            }
        }
        return false;
    }
    #endif


    public void InviteFriends(string subject, string text, string referralDataUrl, byte[] image, Action<string, List<string>> onSuccess, Action onCancel, Action<Exception> onFailure)
    {
#if UNITY_ANDROID
        SendInviteAndroid(text, referralDataUrl, onSuccess, onFailure);
#endif
        
#if UNITY_IOS
        SendInviteIOS(text, referralDataUrl, onSuccess);
#endif
    }

    #if UNITY_ANDROID
    void SendInviteAndroid(string text, string referralDataUrl, Action<string, List<string>> onSuccess, Action<Exception> onFailure)
    {
        try
        {
            using (var c = new AndroidJavaClass("com.kakao.KakaoLink"))
            {
                var context = AndroidUtils.Activity;
                var kakaoLink = c.CallStatic<AndroidJavaObject>("getKakaoLink", context);

                String linkContents = kakaoLink.
                        Call<AndroidJavaObject>("createKakaoTalkLinkMessageBuilder").
                        Call<AndroidJavaObject>("addText", text).
                        Call<AndroidJavaObject>("addWebLink", referralDataUrl, referralDataUrl).
                        Call<string>("build");

                kakaoLink.Call("sendMessage", linkContents, context);

                onSuccess(null, null);
                kakaoLink.Dispose();
            }
        }
        catch (Exception e)
        {
            if (Debug.isDebugBuild)
            {
                Debug.LogException(e);
                onFailure(e);
            }
        }
    }
    #endif

    #if UNITY_IOS
    void SendInviteIOS(string text, string referralDataUrl, Action<string, List<string>> onSuccess)
    {
        _sendKakaoInvite(text, referralDataUrl);
        onSuccess(null, null);
    }

    [DllImport("__Internal")]
    static extern bool _sendKakaoInvite(string text, string referralDataUrl);

    [DllImport("__Internal")]
    static extern bool _isKakaoInstalled();
    #endif
}
