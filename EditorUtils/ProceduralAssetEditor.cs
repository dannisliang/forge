using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Forge.EditorUtils {

	[CustomEditor(typeof(ProceduralAsset), true)]
	public class ProceduralAssetEditor : Editor {

		private static IconLoader IconLoader = null;

		private static bool ShowStatistics = true;
		private static bool ShowDataFile = true;

		private ProceduralAsset Asset = null;

		public override void OnInspectorGUI() {
			if (Asset == null)
				Asset = (ProceduralAsset) serializedObject.targetObject;

			DrawDefaultInspector();

			if (GUI.changed || GUILayout.Button("Build")) {
				Asset.Generate();
			}

			// Statistics
			ShowStatistics = EditorGUILayout.Foldout(ShowStatistics, "Statistics");
			if (ShowStatistics) {
				EditorGUILayout.LabelField("Last build time: ", Asset.IsBuilt ? System.String.Format("{0}ms", Asset.BuildMilliseconds) : "-");
				EditorGUILayout.LabelField(
					System.String.Format("Vertices: {0}", Asset.IsBuilt ? Asset.VertexCount.ToString() : "-"),
					System.String.Format("Triangles: {0}", Asset.IsBuilt ? Asset.TriangleCount.ToString() : "-")
				);
			}

			// Data File
			ShowDataFile = EditorGUILayout.Foldout(ShowDataFile, "Mesh Data File");
			if (ShowDataFile) {
				GUILayout.BeginHorizontal();

				string assetPath = AssetDatabase.GetAssetPath(Asset.Mesh);
				Asset.Mesh = (Mesh) EditorGUILayout.ObjectField(Asset.Mesh, typeof(Mesh), false);
				
				if (!string.IsNullOrEmpty(assetPath)) {
					if (GUILayout.Button("Disconnect")) {
						Asset.Mesh = new Mesh();
						Asset.Generate();
					}				
				} else {
					if (GUILayout.Button("Save as...")) {
						SaveAsset();
					}
				}

				GUILayout.EndHorizontal();
			}

		}

		private void SaveAsset() {
			if (Asset.Mesh != null) {

				string assetPath = AssetDatabase.GetAssetPath(Asset.Mesh);
				assetPath = EditorUtility.SaveFilePanelInProject("Save Mesh", "Untitled", "asset", "Please enter a name to save the generated mesh data");
				if (string.IsNullOrEmpty(assetPath)) return;

				if (!AssetDatabase.Contains(Asset.Mesh)) {
					AssetDatabase.CreateAsset(Asset.Mesh, assetPath);
				}
				Asset.Mesh.Optimize();
				AssetDatabase.SaveAssets();
			} else {
				Debug.Log("No mesh data");
			}
		}

		void OnSceneGUI() {

			if (Asset.MeshDisplay == null) Asset.MeshDisplay = (MeshDisplay) ScriptableObject.CreateInstance(typeof(MeshDisplay));
			if (IconLoader == null) IconLoader = new IconLoader();

			float height = 32f * 8;
			float width = 32f;
			Tools.hidden = true;

			var rect = new Rect(Screen.width - width - 10, Screen.height/2 - height/2, width, height);

			GUILayout.BeginArea(rect);

			// Vertex Display
			Asset.MeshDisplay.DisplayVertices = GUILayout.Toggle(Asset.MeshDisplay.DisplayVertices, IconLoader.Icons["vertex"], "button");
			Asset.MeshDisplay.DisplayVertexNormal = GUILayout.Toggle(Asset.MeshDisplay.DisplayVertexNormal, IconLoader.Icons["vertexNormal"], "button");
			Asset.MeshDisplay.DisplayVertexIndex = GUILayout.Toggle(Asset.MeshDisplay.DisplayVertexIndex, IconLoader.Icons["vertexIndex"], "button");
			Asset.MeshDisplay.DisplayVertexPosition = GUILayout.Toggle(Asset.MeshDisplay.DisplayVertexPosition, IconLoader.Icons["vertexPosition"], "button");

			// Face Display
			Asset.MeshDisplay.DisplayFaces = GUILayout.Toggle(Asset.MeshDisplay.DisplayFaces, IconLoader.Icons["face"], "button");
			Asset.MeshDisplay.DisplayFaceNormal = GUILayout.Toggle(Asset.MeshDisplay.DisplayFaceNormal, IconLoader.Icons["faceNormal"], "button");
			Asset.MeshDisplay.DisplayFaceIndex = GUILayout.Toggle(Asset.MeshDisplay.DisplayFaceIndex, IconLoader.Icons["faceIndex"], "button");

			// Origin
			Asset.MeshDisplay.DisplayOrigin = GUILayout.Toggle(Asset.MeshDisplay.DisplayOrigin, IconLoader.Icons["origin"], "button");
			GUILayout.EndArea();
		}

	}
	
}