// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleScriptableObject.cs">
//   Copyright (c) 2023 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace JD
{
	/// <summary>
	/// Simple ScriptableObject that holds very few data to test Unity performance with thousands of tiny SOs
	/// </summary>
	public class SimpleScriptableObject : ScriptableObject
	{
		public int Id;
		public float OneDividedById;
		public string IdAsString;
	}
}
