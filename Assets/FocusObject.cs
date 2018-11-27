using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// These objects also need to be on the Focus layer for it to work.
/// </summary>
public class FocusObject : MonoBehaviour {

	/// <summary>
	/// How close you need to be for the zoom to work. 100 is max in demo
	/// </summary>
	public float RangeRequirement = 100;
	/// <summary>
	/// Go the additional mile and attempt to look at the middle of the object
	/// as well as zoom. e.g. a computer screen
	/// </summary>
	public bool ZoomInRightGood = false;
}
