//////////////////////////////////////////////////////////////////////////
//
// Find Asset Reference
// 
// Created by CY.
//
// Copyright 2011 FourNext Group
// All rights reserved
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class FN_FindAssetReferences : MonoBehaviour
{
	public static bool needRefresh = true;
	public static bool IgnoreCacheUpdate
	{
		set { EditorPrefs.SetBool("FNAssetFindReferences", value);	}		
		get { return EditorPrefs.GetBool("FNAssetFindReferences", false); } 
	}

	public class AssetData
	{
		public string AssetPath = "";
		public string[] AssetDependencies = new string[0];
		
		public bool Find(string assetPath)
		{
			if( assetPath.ToLower() == AssetPath.ToLower() )
				return false;
			
			foreach(string s in AssetDependencies)
			{
				if( assetPath.ToLower() == s.ToLower() )
					return true;
			}
			
			return false;
		}
	}
	private static List<AssetData> mAssetCache = new List<AssetData>();
	
	[MenuItem("CONTEXT/Object/Find Asset References")]
	static void FindReferences(MenuCommand command)
	{
		List<Object> findedPrefabList = new List<Object>();
	
		Object obj = command.context;

		// for prefab
		if( obj.GetType() == typeof(Transform) )
			obj = ((Transform)obj).gameObject;

		FindInProject(AssetDatabase.GetAssetPath(obj), ref findedPrefabList);

        FN_FindAssetReferencesWindow refWindow = (FN_FindAssetReferencesWindow)EditorWindow.GetWindow(typeof(FN_FindAssetReferencesWindow));
		refWindow.SetObject(obj, findedPrefabList);
	}

    [MenuItem("CONTEXT/Object/Find Asset References", true)]
	static bool FindReferencesValidate(MenuCommand command)
	{
		// only Asset files
		Object o = command.context;
		return (AssetDatabase.Contains(o));
	}
	
	static void WriteLog(string log)
	{
		Debug.Log(log);
	}
	static void WriteLog(string fmt, params object[] args)
	{
		WriteLog(string.Format(fmt, args));
	}
	
	//-----------------------------------------------------------------------------
	static void FindInProject(string findObjectAssetPath, ref List<Object> findedList)
	{
        if (needRefresh && IgnoreCacheUpdate == false)
        {
            if (EditorUtility.DisplayDialog("CacheUpdate", "Asset changed! update asset cache ?", "OK", "Cancel"))
                MakeAssetCache();
        }

        if (mAssetCache.Count <= 0 && IgnoreCacheUpdate == false)
        {
            if (EditorUtility.DisplayDialog("CacheUpdate", "Asset Cache is empty. Do you update cache?", "OK", "Cancel"))
                MakeAssetCache();
        }
		
		foreach(AssetData ad in mAssetCache)
		{
			if( ad.Find(findObjectAssetPath) )
			{
				Object prefab = (Object)AssetDatabase.LoadAssetAtPath(ad.AssetPath, typeof(Object));
				if( prefab != null )
				{
					if( findedList.Contains(prefab) == false )
					{
						findedList.Add(prefab);	
					}
				}
				else
				{
					Debug.LogWarning("Scene find. " + ad.AssetPath);
				}
			}				
		}
	}

    //-----------------------------------------------------------------------------
    public static string AssetCacheInfo()
    {
        return string.Format("Prefab count in AssetCache : {0}", mAssetCache.Count);
    }

	//-----------------------------------------------------------------------------
	public static void MakeAssetCache()
	{
		List<string> pathlist = new List<string>();
		RetrievePrefab_inProject(ref pathlist);
		RetrieveScene_inProject (ref pathlist);
		
		mAssetCache.Clear();

		int count = 0;
		float progress = 0.0f;
		foreach(string p in pathlist)
		{
			count++;
			progress = count / pathlist.Count;
			if( EditorUtility.DisplayCancelableProgressBar("Make cache", p, progress) )
				break;
			
			AssetData ad = new AssetData();
			ad.AssetPath = p;
			string[] srcPaths = new string[1];
			srcPaths[0] = p;
			ad.AssetDependencies = AssetDatabase.GetDependencies(srcPaths);
			
			mAssetCache.Add(ad);
		}
		
		if( mAssetCache.Count != pathlist.Count )
			needRefresh = true;
		else
			needRefresh = false;
		
		EditorUtility.ClearProgressBar();
	}
	
	//-----------------------------------------------------------------------------
	static void RetrievePrefab_inProject(ref List<string> fileList)
	{
		string[] files;

		Stack stack = new Stack();
		stack.Push(Application.dataPath);

		while (stack.Count > 0)
		{
			string dir = (string)stack.Pop();

			try
			{
				files = Directory.GetFiles(dir, "*.prefab");
				for (int i = 0; i < files.Length; ++i)
				{
					files[i] = files[i].Substring(Application.dataPath.Length - 6);	// remove name "Assets"
					fileList.Add(files[i]);					
				}

				foreach (string dn in Directory.GetDirectories(dir))
				{
					stack.Push(dn);
				}
			}
			catch
			{
				Debug.LogError("Could not access folder: \"" + dir + "\"");
			}
		}
	}	

	static void RetrieveScene_inProject (ref List<string> fileList)
	{
		string[] files;
		
		Stack stack = new Stack();
		stack.Push(Application.dataPath);
		
		while (stack.Count > 0)
		{
			string dir = (string)stack.Pop();
			
			try
			{
				files = Directory.GetFiles(dir, "*.unity");
				for (int i = 0; i < files.Length; ++i)
				{
					files[i] = files[i].Substring(Application.dataPath.Length - 6);	// remove name "Assets"
					fileList.Add(files[i]);					
				}
				
				foreach (string dn in Directory.GetDirectories(dir))
				{
					stack.Push(dn);
				}
			}
			catch
			{
				Debug.LogError("Could not access folder: \"" + dir + "\"");
			}
		}
	}
}

