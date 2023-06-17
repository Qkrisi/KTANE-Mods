using System;
using System.Collections.Generic;
using HarmonyLib;

namespace TweaksAssembly.Patching
{
	static class Patching
	{
		private static readonly Dictionary<string, Harmony> instances = new Dictionary<string, Harmony>();

		public static bool PatchClasses(string id, out Harmony instance, params Type[] types)
		{
			instance = new Harmony($"samfundev.tweaks.{id}");
			var success = false;

			foreach (Type type in types)
			{
				var result = instance.CreateClassProcessor(type).Patch();
				success |= result?.Count > 0;
			}

			return success;
		}

		public static bool EnsurePatch(string id, params Type[] types)
		{
			if (instances.ContainsKey(id))
				return true;

			var success = PatchClasses(id, out Harmony instance, types);
			if (success)
				instances.Add(id, instance);
			return success;
		}

		public static Harmony ManualInstance(string id)
		{
			if (!instances.ContainsKey(id))
				instances.Add(id, new Harmony($"samfundev.tweaks.{id}"));

			return instances[id];
		}
	}
}