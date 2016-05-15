using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GhostTypes {

	static Dictionary<string, Ghost> types;

	static Dictionary<string, Ghost> typeDict()
	{
		if (types == null) {
			types = new Dictionary<string, Ghost> ();

			float l3BounceHeight = GameObject.Find("L3BounceHeight").transform.position.y;
			types.Add("L3Ghost",new Ghost("L3Ghost", l3BounceHeight, "L2Ghost"));

			float l2BounceHeight = GameObject.Find("L2BounceHeight").transform.position.y;
			types.Add("L2Ghost",new Ghost("L2Ghost", l2BounceHeight, "L1Ghost"));

			float l1BounceHeight = GameObject.Find("L1BounceHeight").transform.position.y;
			types.Add("L1Ghost",new Ghost("L1Ghost", l1BounceHeight, "None"));
		}

		return types;
	}

	public static Ghost getType(string name) 
	{
		return GhostTypes.typeDict () [name];
	}
}