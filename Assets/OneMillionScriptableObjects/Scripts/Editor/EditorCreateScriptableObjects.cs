// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorCreateScriptableObjects.cs">
//   Copyright (c) 2023 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JD
{
	/// <summary>
	/// Editor menu items to batch create ScriptableObjects
	/// Expects folder <see cref="DataFolderPath"> to exist in project
	/// Will create folders for every 100 ScriptableObjects
	/// which are nested into folders of 10_000 ScriptableObjects
	/// </summary>
	public static class EditorCreateScriptableObjects
	{
		private const string DataFolderPath = "Assets/OneMillionScriptableObjects/Data";
		private static StringBuilder stringBuilder = new StringBuilder();

		[MenuItem("Tools/Create next 10_000 ScriptableObjects")]
		private static void CreateNext10_000()
		{
			int depth0Start = 0;
			string depth0 = $"{depth0Start:000000}-{(depth0Start + 10_000):000000}";
			while(AssetDatabase.IsValidFolder($"{DataFolderPath}/{depth0}"))
			{
				depth0Start += 10_000;
				depth0 = $"{depth0Start:000000}-{(depth0Start + 10_000):000000}";
			}

			CreateScriptableObjects(depth0Start, 10_000);
		}

		[MenuItem("Tools/Create 100_000 ScriptableObjects")]
		private static void Create100_000()
		{
			CreateScriptableObjects(0, 100_000);
		}

		[MenuItem("Tools/Create 1_000_000 ScriptableObjects")]
		private static void Create1_000_000()
		{
			CreateScriptableObjects(0, 1_000_000);
		}

		private static void CreateScriptableObjects(int start, int count)
		{
			Stopwatch sw = new Stopwatch();
			try
			{
				sw.Start();
				AssetDatabase.StartAssetEditing();
				int index = start;
				int end = start + count;
				while(index < end) {
					// Create ScriptableObjects in batches of 10_000 to minimize risk that something breaks the complete result
					int batchSize = Mathf.Min(end - index, 10_000);
					if(CreateScriptableObjectsInternal(index, batchSize))
					{
						break;
					}
					AssetDatabase.SaveAssets();
					UnityEngine.Debug.Log($"Created ScriptableObjects [{index};{index + batchSize}[");
					index += batchSize;
				}

			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
			}
			finally
			{
				UnityEngine.Debug.Log($"Created {count} ScriptableObjects in {sw.ElapsedMilliseconds/1000f:0.00}s -> {(float)sw.ElapsedMilliseconds/count:0.000}ms per SO");
				sw.Restart();
				AssetDatabase.StopAssetEditing();
				EditorUtility.ClearProgressBar();
				AssetDatabase.SaveAssets();
				UnityEngine.Debug.Log($"Finalized {count} ScriptableObjects in {sw.ElapsedMilliseconds/1000f:0.00}s");
			}

		}

		private static bool CreateScriptableObjectsInternal(int start, int count)
		{
			string progressInfo = $"Create ScriptableObjects [{start};{start + count}[ in {DataFolderPath}";
			int depth0Start, depth1Start;
			string depth0 = string.Empty, depth1 = string.Empty;

			for (int i = start; i < start + count; i++)
			{
				if (i % 10 == 0)
				{
					if (EditorUtility.DisplayCancelableProgressBar($"Create ScriptableObjects ({i}/{start + count})", progressInfo, (float)(i-start) / count))
					{
						return true;
					}
				}

				if (i % 10_000 == 0 || i == start)
				{
					depth0Start = Mathf.FloorToInt((float)i / 10_000f) * 10_000;
					depth0 = $"{depth0Start:000000}-{(depth0Start + 10_000):000000}";
					if (!AssetDatabase.IsValidFolder($"{DataFolderPath}/{depth0}"))
					{
						AssetDatabase.CreateFolder(DataFolderPath, depth0);
					}
				}

				if (i % 100 == 0 || i == start)
				{
					depth1Start = Mathf.FloorToInt((float)(i) / 100f) * 100;
					depth1 = $"{depth1Start:000000}-{(depth1Start + 100):000000}";
					if (!AssetDatabase.IsValidFolder($"{DataFolderPath}/{depth0}/{depth1}"))
					{
						AssetDatabase.CreateFolder($"{DataFolderPath}/{depth0}", depth1);
					}
				}

				SimpleScriptableObject so = ScriptableObject.CreateInstance<SimpleScriptableObject>();
				so.Id = i;
				so.OneDividedById = 1f / Mathf.Max(1, i);
				so.IdAsString = i.ToString("n0");

				// Folder Path
				stringBuilder.Append(DataFolderPath).Append('/').Append(depth0).Append('/').Append(depth1);
				// File Path
				stringBuilder.Append('/').Append(i.ToString("000000")).Append(".asset");

				AssetDatabase.CreateAsset(so, stringBuilder.ToString());
				stringBuilder.Clear();
			}
			return false;
		}
	}
}
