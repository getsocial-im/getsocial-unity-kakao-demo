#if UNITY_IOS
using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;
using System;

public static class GetSocialKakaoPostprocess
{
    const string KAKAO_APP_KEY = "4d436485d41336a82c47d0142682b4d7";
    const string KAKAO_URL_SCHEME = "kakao" + KAKAO_APP_KEY;

    static string PbxProjectPath = string.Empty;

    [PostProcessBuild(2000)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            PbxProjectPath = PBXProject.GetPBXProjectPath(path);

            PostProcessIosProject();
        }
    }

    static void PostProcessIosProject()
    {


        ModifyPlist(AddKakaoTalkUrlScheme);

        Debug.Log("KAKAO TALK setup for iOS project");
    }

    static void AddKakaoTalkUrlScheme(PlistDocument plist)
    {

    }

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

            string infoPlistPath = Path.Combine(PbxProjectPath, "Info.plist");
            plistInfoFile.ReadFromString(File.ReadAllText(infoPlistPath));

            modifier(plistInfoFile);

            File.WriteAllText(infoPlistPath, plistInfoFile.WriteToString());
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
#endif