using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GhostTypes {

	static Dictionary<string, Ghost> types;

	static Dictionary<string, Ghost> typeDict()
	{
		if (types == null) {
			types = new Dictionary<string, Ghost> ();

			float l3BounceHeight = GameObject.Find ("L3BounceHeight").transform.position.y;
			types.Add ("L3Ghost", new Ghost ("L3Ghost", l3BounceHeight, "L2Ghost"));

			float l2BounceHeight = GameObject.Find ("L2BounceHeight").transform.position.y;
			types.Add ("L2Ghost", new Ghost ("L2Ghost", l2BounceHeight, "L1Ghost"));

			float l1BounceHeight = GameObject.Find ("L1BounceHeight").transform.position.y;
			types.Add ("L1Ghost", new Ghost ("L1Ghost", l1BounceHeight, "None"));

			
			GameObject go = GameObject.Find ("L1BounceHeightHigh");
			if (go != null) {
				float l1BounceHeightHigh = GameObject.Find ("L1BounceHeightHigh").transform.position.y;
				types.Add ("L1GhostHigh", new Ghost ("L1GhostHigh", l1BounceHeightHigh, "None"));
			}

			go = GameObject.Find ("L4BounceHeight");
			if (go != null) {
				float l4BounceHeight = GameObject.Find ("L4BounceHeight").transform.position.y;
				types.Add ("L4Ghost", new Ghost ("L4Ghost", l4BounceHeight, "L3Ghost"));
			}
			
			go = GameObject.Find ("L5BounceHeight");
			if (go != null) {
				float l5BounceHeight = GameObject.Find ("L5BounceHeight").transform.position.y;
				types.Add ("L5Ghost", new Ghost ("L5Ghost", l5BounceHeight, "L4Ghost"));
			}
		}

		return types;
	}

	public static Ghost getType(string name) 
	{
		return GhostTypes.typeDict () [name];
	}
}