using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables {
	/// <summary>
	/// Class assigned to the main collectable of the game
	/// </summary>
	public class PlanetFragment : Collectable
	{
		[Header("Planet Fragment Values")]

		[Tooltip("Max amount of movement to the vertex")]
		public float offset;
		[Tooltip("Time between vertices movement")]
		public float timer = Mathf.Infinity;

		List<Vector3> Vectices;
		Vector3[] newlist, tmpList;
		Mesh myMesh;
		MeshFilter mf;

		/// <summary>
		/// Struct of shape with geometry and topology.
		/// </summary>
		struct Shape
		{
			// {0,0,0}, {6,0,0}, {3, 3, 0}, {9,3,0}
			public List<Vector3> geometry;
			// {0, 1, 2, 1, 4, 3}
			public List<int> topology;
		};

		// ----------------------------------------------------------------------------------------------- Unity Methods

		void Start()
		{
			myMesh = new Mesh();
			Shape i = new Shape();
			i.geometry = new List<Vector3>(){
	 		new Vector3(0, 0, 1),
			new Vector3(1, 0, 0),
			new Vector3(0, 0, -1),
			new Vector3(-1, 0, 0),
			new Vector3(0, 1, 0),
			new Vector3(0, -1, 0)
		};

			i.topology = new List<int>(){   0, 1, 4,
										 1, 2, 4,
								 		2, 3, 4,
										 3, 0, 4,

										 0, 5, 1,
								 		1, 5, 2,
								 		2, 5, 3,
								 		3, 5, 0
								};
			Tessellate(i);
			Tessellate(i);
			Vectices = i.geometry;
			myMesh.vertices = i.geometry.ToArray();
			myMesh.triangles = i.topology.ToArray();
			myMesh.RecalculateNormals();
			render = GetComponent<MeshRenderer>();
			render.material = mat;// Material(Shader.Find("Diffuse"));
			mf = gameObject.AddComponent<MeshFilter>();
			mf.mesh = myMesh;

			newlist = Vectices.ToArray();
			tmpList = Vectices.ToArray();
			//Vectices.AddRange(myMesh.vertices);
		}
		private void FixedUpdate()
		{
			Change();
			transform.Rotate(Vector3.up * (30 * Time.deltaTime));
		}

		// ----------------------------------------------------------------------------------------------- Private Methods

		// Function that adds sub 4 triangles to a triangular face on a mesh
		void Tessellate(Shape input)
		{
			for (int t = 0; t < input.topology.Count; t += 12)
			{
				Vector3 A = input.geometry[input.topology[t + 0]];
				Vector3 B = input.geometry[input.topology[t + 1]];
				Vector3 C = input.geometry[input.topology[t + 2]];
				Vector3 o = ((A + B) / 2.0f).normalized;
				Vector3 p = ((B + C) / 2.0f).normalized;
				Vector3 q = ((C + A) / 2.0f).normalized;
				int ia = input.topology[t + 0];
				int ib = input.topology[t + 1];
				int ic = input.topology[t + 2];
				int io = FindVertex(input.geometry, o);
				int ip = FindVertex(input.geometry, p);
				int iq = FindVertex(input.geometry, q);

				if (io == -1)
				{
					input.geometry.Add(o);
					io = input.geometry.Count - 1;
				}
				if (ip == -1)
				{
					input.geometry.Add(p);
					ip = input.geometry.Count - 1;
				}
				if (iq == -1)
				{
					input.geometry.Add(q);
					iq = input.geometry.Count - 1;
				}

				List<int> newT = new List<int>();
				for (int i = 0; i < t; i++)
					newT.Add(input.topology[i]);
				newT.Add(ia); newT.Add(io); newT.Add(iq);
				newT.Add(io); newT.Add(ib); newT.Add(ip);
				newT.Add(iq); newT.Add(ip); newT.Add(ic);
				newT.Add(io); newT.Add(ip); newT.Add(iq);
				for (int i = t + 3; i < input.topology.Count; i++)
					newT.Add(input.topology[i]);

				input.topology.Clear(); // Reemplazo la topología con la versión teselada.
				for (int i = 0; i < newT.Count; i++)
					input.topology.Add(newT[i]);
			}
		}
		// Gets the index of a certain Vertex in a list.
		int FindVertex(List<Vector3> list, Vector3 look)
		{
			return list.IndexOf(look);
		}

		// This method hangels the movement of the vertex on a FixedUpdate.
		void Change()
		{
			if (timer > 1)
			{
				for (int i = 0; i < newlist.Length; i++)
				{
					newlist[i] = new Vector3(
						Random.Range(-offset, offset),
						Random.Range(-offset, offset),
						Random.Range(-offset, offset)
						);
				}
				timer = 0;
			}
			else if (timer <= 5)
			{
				for (int i = 0; i < tmpList.Length; i++)
				{
					tmpList[i] += newlist[i] * Time.deltaTime;

					tmpList[i] = new Vector3(
						Mathf.Clamp(tmpList[i].x, Vectices[i].x - offset, Vectices[i].x + offset),
						Mathf.Clamp(tmpList[i].y, Vectices[i].y - offset, Vectices[i].y + offset),
						Mathf.Clamp(tmpList[i].z, Vectices[i].z - offset, Vectices[i].z + offset)
						);
				}
				timer += Time.deltaTime;
				myMesh.vertices = tmpList;
			}
		}
	}
}
