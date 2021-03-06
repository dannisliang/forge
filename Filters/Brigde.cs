using UnityEngine;

namespace Forge.Filters {
	
	public class Bridge {

		private Geometry _a;
		private Geometry _b;

		public Bridge() {}

		public Bridge(Geometry a, Geometry b) {
			InputA(a);
			InputB(b);
		}

		public void InputA(Geometry geometry) {
			_a = geometry.Copy();
		}

		public void InputB(Geometry geometry) {
			_b = geometry.Copy();
		}

		public Geometry Output() {
		
			int vertexCount = Mathf.Min(_a.Vertices.Length, _b.Vertices.Length);

			Geometry geometry = new Geometry();
			geometry.Vertices = new Vector3[vertexCount * 2];
			geometry.UV = new Vector2[vertexCount * 2];
			geometry.Triangles = new int[vertexCount * 6];

			// Vertices and triangles
			for (int i = 0; i < vertexCount; i++) {
				geometry.Vertices[i] = _a.Vertices[i];
				geometry.Vertices[vertexCount + i] = _b.Vertices[i];

				// First Triangle
				geometry.Triangles[i*6  ] = i;
				geometry.Triangles[i*6+1] = i + vertexCount;
				geometry.Triangles[i*6+2] = i + 1;

				// Second Triangle
				geometry.Triangles[i*6+3] = i;
				geometry.Triangles[i*6+4] = i + vertexCount - 1;
				geometry.Triangles[i*6+5] = i + vertexCount;
			}

			geometry.CalculateNormals();

			return geometry;
		}

	} // class

} // namespace