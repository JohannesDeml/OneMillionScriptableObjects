using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace JD
{
	[InitializeOnLoad]
	public static class EditorTimeMeasurements
	{
		static EditorTimeMeasurements()
		{
			// Note that EditorApplication.timeSinceStartup does not count from the moment you open the project
			// Therefore this measurement excludes a lot of time for importing assets
			// Therefore the measurements done in theis repo rely on MeasureTime.sh which measures the real durations
			if (!SessionState.GetBool("FirstInitDone", false))
			{
				Debug.Log($"Editor Startup time ([InitializeOnLoad]): {EditorApplication.timeSinceStartup:0.00}");
				SessionState.SetBool("FirstInitDone", true);
			}
		}
		
		[UsedImplicitly]
		public static void LogEditorStartupTime()
		{
			Debug.Log($"Editor Startup time (LogEditorStartupTime): {EditorApplication.timeSinceStartup:0.00}");
		}
	}
}