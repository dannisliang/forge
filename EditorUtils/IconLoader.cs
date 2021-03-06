using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class IconLoader {

	public Dictionary<string, Texture2D> Icons;

	public IconLoader() {
		Icons = new Dictionary<string, Texture2D>();

		string[] icons = new string[] {
			"vertex", "vertexIndex", "vertexNormal", "vertexPosition",
			"face", "faceIndex", "faceNormal", "facePosition",
			"origin"
		};

		for (int i = 0; i < icons.Length; i++) {
			string filePath = System.String.Format("Forge/Icons/{0}.png", icons[i]);
			string vertexIconPath = Path.Combine(Application.dataPath, filePath);
			Texture2D texture = new Texture2D( 1, 1 );
			texture.LoadImage(File.ReadAllBytes(vertexIconPath));
			texture.Apply();
			texture.hideFlags = HideFlags.HideAndDontSave;
			Icons.Add(icons[i], texture);
		}

	}

}