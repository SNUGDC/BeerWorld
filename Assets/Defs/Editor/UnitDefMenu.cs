using UnityEditor;
using System.Collections;

public class UnitDefMenu
{
	[MenuItem("Assets/Create/UnitDef")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<UnitDef>();
	}
}
