#if UNITY_IOS
using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;
using System;
using System.Linq;

public static class GetSocialKakaoPostprocess
{
    const string KAKAO_APP_KEY = "4d436485d41336a82c47d0142682b4d7";
    const string KAKAO_URL_SCHEME = "kakao" + KAKAO_APP_KEY;

    static string ProjectPath = string.Empty;
    static string PbxProjectPath = string.Empty;

    [PostProcessBuild(2000)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            ProjectPath = path;
            PbxProjectPath = PBXProject.GetPBXProjectPath(path);

            PostProcessIosProject();
        }
    }

    static void PostProcessIosProject()
    {
        ModifyPlist(AddKakaoTalkUrlScheme);
        ModifyPlist(AddKakaoAppKey);
        ModifyPlist(AddApplicationQuerySceheme);

        Debug.Log("KAKAO TALK setup for iOS project");
    }

    static void AddKakaoTalkUrlScheme(PlistDocument plist)
    {
        const string CFBundleURLTypes = "CFBundleURLTypes";
        const string CFBundleURLSchemes = "CFBundleURLSchemes";

        if (!plist.root.values.ContainsKey(CFBundleURLTypes))
        {
            plist.root.CreateArray(CFBundleURLTypes);
        }

        var cFBundleURLTypesElem = plist.root.values[CFBundleURLTypes] as PlistElementArray;

        var getSocialUrlSchemesArray = new PlistElementArray();
        getSocialUrlSchemesArray.AddString(KAKAO_URL_SCHEME);

        PlistElementDict getSocialSchemeElem = cFBundleURLTypesElem.AddDict();
        getSocialSchemeElem.values[CFBundleURLSchemes] = getSocialUrlSchemesArray;
    }

    static void AddKakaoAppKey(PlistDocument plist)
    {
        plist.root.SetString("KAKAO_APP_KEY", KAKAO_APP_KEY);
    }

    static void AddApplicationQuerySceheme(PlistDocument plist)
    {
        const string LSApplicationQueriesSchemes = "LSApplicationQueriesSchemes";

        string[] kakaoSchemes =
            {
                KAKAO_URL_SCHEME,
                "kakaokompassauth",
                "storykompassauth",
                "kakaolink",
                "kakaotalk-4.5.0",
                "kakaostory-2.9.0",
                "storylink"
            };

        PlistElementArray appsArray;
        appsArray = plist.root.values.ContainsKey(LSApplicationQueriesSchemes) ? 
                (PlistElementArray)plist.root.values[LSApplicationQueriesSchemes] : 
                plist.root.CreateArray(LSApplicationQueriesSchemes);
        kakaoSchemes.ToList().ForEach(appsArray.AddString);
    }

    #region helpers

    static void ModifyProject(Action<PBXProject> modifier)
    {
        PBXProject project = new PBXProject();
        project.ReadFromString(File.ReadAllText(PbxProjectPath));

        modifier(project);

        File.WriteAllText(PbxProjectPath, project.WriteToString());
    }

    static void ModifyPlist(Action<PlistDocument> modifier)
    {
        try
        {
            var plistInfoFile = new PlistDocument();

            string infoPlistPath = Path.Combine(ProjectPath, "Info.plist");
            plistInfoFile.ReadFromString(File.ReadAllText(infoPlistPath));

            modifier(plistInfoFile);

            File.WriteAllText(infoPlistPath, plistInfoFile.WriteToString());
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    #endregion
}
#endif