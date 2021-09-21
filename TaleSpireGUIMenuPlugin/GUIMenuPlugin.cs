using BepInEx;
using BepInEx.Configuration;
using System;
using UnityEngine;

namespace LordAshes
{
	[BepInPlugin(Guid, Name, Version)]
	public partial class GUIMenuPlugin : BaseUnityPlugin
	{
		// Plugin info
		public const string Name = "GUI Menu Plug-In";
		public const string Guid = "org.lordashes.plugins.guimenu";
		public const string Version = "1.0.0.0";

		/// <summary>
		/// Function for initializing plugin
		/// This function is called once by TaleSpire
		/// </summary>
		void Awake()
		{
			UnityEngine.Debug.Log("GUI Menu Plugin: Active.");
		}
	}
}
