using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GhostTypes {

	static Dictionary<string, Ghost> types;

	static Dictionary<string, Ghost> typeDict()
	{
		if (types == null) {
			types = new Dictionary<string, Ghost> ();
			types.Add("L3Ghost",new Ghost("L3Ghost", 10, "L2Ghost"));
			types.Add("L2Ghost",new Ghost("L2Ghost", 10, "L1Ghost"));
			types.Add("L1Ghost",new Ghost("L1Ghost", 10, "None"));
		}

		return types;
	}

	public static Ghost getType(string name) 
	{
		return GhostTypes.typeDict () [name];
	}
}