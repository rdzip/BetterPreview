using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using RDLevelEditor;
using UnityEngine;

namespace BetterPreview
{
	[BepInPlugin("com.rhythmdr.betterpreview", "Better Preview", "0.1")]
	public class BetterPreview : BaseUnityPlugin
	{
		private static ConfigEntry<int> maxSize;

		private void Awake()
		{
			maxSize = Config.Bind("General", "MaxSize", 16, new ConfigDescription("Max size.", new AcceptableValueRange<int>(1, 64)));
			Harmony.CreateAndPatchAll(typeof(MainPatch));
		}

		private void OnDestroy()
		{
			Harmony.UnpatchAll();
		}

		public static class MainPatch
		{
			[HarmonyPatch(typeof(SpriteHeader), "UpdateUI")]
			[HarmonyPostfix]
			public static void PostUpdateUI(SpriteHeader __instance)
			{
				Vector2 size = __instance.spriteImage.rectTransform.sizeDelta;

				while (Mathf.Max(size.x / 4, size.y) - maxSize.Value > maxSize.Value / 2)
				{
					size /= 2;
				}
				__instance.spriteImage.rectTransform.sizeDelta = size;
				float x = (size.x % 2 == 0) ? 0f : -0.5f;
				float y = (size.y % 2 == 0) ? 0f : -0.5f;
				__instance.spriteImage.rectTransform.anchoredPosition = new Vector2(x, y);
			}
		}
	}
}
