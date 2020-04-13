﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using THREE.Serialization;
using THREE.Utility;

namespace THREE.Core
{
	/// <summary>
	/// Base class for all geometries. \n
	/// Analogous to https://threejs.org/docs/index.html#api/core/Geometry \n
	/// Design based on need for Three.js Loaders.
	/// </summary>
	public class Geometry : Element, IGeometry, IEquatable<Geometry>
	{
		/// <summary>
		/// Geometry data.
		/// </summary>
		[JsonProperty("data")]
		GeometryData Data { get; set; }

		/// <summary>
		/// List of vertices for this geometry.
		/// </summary>
		[JsonIgnore]
		public List<float> Vertices
		{
			get { return Data.RawVertices; }
			set { Data.RawVertices = value; }
		}

		/// <summary>
		/// List of colors for this geometry.
		/// </summary>
		[JsonIgnore]
		public List<int> Colors
		{
			get { return Data.Colors; }
			set { Data.Colors = value; }
		}

		/// <summary>
		/// List of faces for this geometry.
		/// </summary>
		[JsonIgnore]
		public List<int> Faces
		{
			get { return Data.Faces; }
			set { Data.Faces = value; }
		}

		/// <summary>
		/// List of normals for this geometry.
		/// </summary>
		[JsonIgnore]
		public List<float> Normals
		{
			get { return Data.RawNormals; }
			set { Data.RawNormals = value; }
		}

		/// <summary>
		/// The list of UVs associated with this geometry.
		/// </summary>
		[JsonIgnore]
		public List<List<float>> Uvs
		{
			get { return Data.Uvs; }
			set { Data.Uvs = value; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Geometry()
		{
			Type = GetType().Name;
			Data = new GeometryData();
		}

		/// <summary>
		/// Constructor with default values = null.
		/// </summary>
		/// <param name="vertices"></param>
		/// <param name="faces"></param>
		/// <param name="normals"></param>
		/// <param name="colors"></param>
		/// <param name="uvs"></param>
		public Geometry(List<float> vertices = null, List<int> faces = null, List<float> normals = null, List<int> colors = null, List<List<float>> uvs = null) : this()
		{
			if (vertices == null)
			{
				return;
			}

			Vertices = vertices;

			if (normals != null && normals.Count > 0)
			{
				Normals = normals;
			}

			if (colors != null && colors.Count > 0)
			{
				Colors = colors;
			}

			if (uvs != null && uvs.Count > 0)
			{
				Uvs = uvs;
			}

			if (faces != null)
			{
				Faces = faces;
			}
		}

		/// <summary>
		/// Utility method for processing faces.
		/// TODO: Extend for all types of faces and switches.
		/// </summary>
		/// <param name="faces"></param>
		/// <param name="vertexColors"></param>
		/// <param name="uvs"></param>
		/// <returns>A list of int.</returns>
		public static List<int> ProcessFaceArray(IEnumerable<int[]> faces, bool vertexColors, bool uvs)
		{
			var face = new GeometryFace
			{
				Topology = false,
				VertexColors = vertexColors,
				FaceColor = false,
				FaceMaterial = false,
				FaceNormals = false,
				FaceUVs = false,
				FaceVertexUVs = uvs,
				VertexNormals = true
			};

			List<int> facesIndex = new List<int>();

			if (faces != null)
			{
				foreach (var meshFace in faces)
				{
					if (meshFace.Length == 3) // has count 3
					{
						face.Topology = false;

						facesIndex.Add(face.GetFaceType());

						facesIndex.Add(meshFace[0]); //A
						facesIndex.Add(meshFace[1]); //B
						facesIndex.Add(meshFace[2]); //C

						if (face.VertexNormals)
						{
							facesIndex.Add(meshFace[0]); //A
							facesIndex.Add(meshFace[1]); //B
							facesIndex.Add(meshFace[2]); //C
						}

						if (face.VertexColors)
						{
							facesIndex.Add(meshFace[0]); //A
							facesIndex.Add(meshFace[1]); //B
							facesIndex.Add(meshFace[2]); //C
						}

						if (face.FaceVertexUVs)
						{
							facesIndex.Add(meshFace[0]); //A
							facesIndex.Add(meshFace[1]); //B
							facesIndex.Add(meshFace[2]); //C
						}
					}
					else
					{
						face.Topology = true;

						facesIndex.Add(face.GetFaceType());

						facesIndex.Add(meshFace[0]); //A
						facesIndex.Add(meshFace[1]); //B
						facesIndex.Add(meshFace[2]); //C
						facesIndex.Add(meshFace[3]); //D

						if (face.VertexNormals)
						{

							facesIndex.Add(meshFace[0]); //A
							facesIndex.Add(meshFace[1]); //B
							facesIndex.Add(meshFace[2]); //C
							facesIndex.Add(meshFace[3]); //D
						}

						if (face.VertexColors)
						{
							facesIndex.Add(meshFace[0]); //A
							facesIndex.Add(meshFace[1]); //B
							facesIndex.Add(meshFace[2]); //C
							facesIndex.Add(meshFace[3]); //D
						}

						if (face.FaceVertexUVs)
						{
							facesIndex.Add(meshFace[0]); //A
							facesIndex.Add(meshFace[1]); //B
							facesIndex.Add(meshFace[2]); //C
							facesIndex.Add(meshFace[3]); //D
						}
					}
				}
			}

			return facesIndex;

		}

		/// <summary>
		/// Utility method for flattening a List of float[].
		/// </summary>
		/// <param name="vertices">The list to flatten.</param>
		/// <returns>A list of float.</returns>
		public static List<float> ProcessVertexArray(IEnumerable<float[]> vertices)
		{
			var Vertices = new List<float>();

			foreach (var vert in vertices)
			{
				Vertices.Add(vert[0]);
				Vertices.Add(vert[1]);
				Vertices.Add(vert[2]);
			}

			return Vertices;
		}

		/// <summary>
		/// Flatten a List of float[].
		/// </summary>
		/// <param name="normals">The list to flatten.</param>
		/// <returns>A list of float.</returns>
		public static List<float> ProcessNormalArray(IEnumerable<float[]> normals)
		{
			var Normals = new List<float>();

			foreach (var norm in normals)
			{
				Normals.Add(norm[0]);
				Normals.Add(norm[1]);
				Normals.Add(norm[2]);
			}

			return Normals;
		}

		/// <summary>
		/// Check if one Geometry equals another.
		/// TODO: Check if base.Equals(other)? Object3D would need to be IEquatable.
		/// </summary>
		/// <param name="other">The other object to check.</param>
		/// <returns></returns>
		public bool Equals(Geometry other)
		{
			if (other == null)
			{
				return false;
			}
			else
			{
				return Data.Colors.SequenceEqual(other.Data.Colors) &&
					   Data.Faces.SequenceEqual(other.Data.Faces) &&
					   Data.RawNormals.SequenceEqual(other.Data.RawNormals) &&
					   Data.Uvs.Any(uv => other.Data.Uvs.Any(otherUv => uv.SequenceEqual(otherUv))) &&
					   Data.RawVertices.SequenceEqual(other.Data.RawVertices);
			}
		}

		/// <summary>
		/// Check if this geometry equals another comparing them as objects.
		/// </summary>
		/// <param name="other">The other object to compare.</param>
		/// <returns></returns>
		public override bool Equals(object other)
		{
			//return Equals(other as Geometry);
			if (other.GetType() == typeof(Geometry))
			{
				return Equals((Geometry)other) && base.Equals(other);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Override of the == operator.
		/// </summary>
		/// <param name="a">The first geometry.</param>
		/// <param name="b">The second geometry.</param>
		/// <returns>True if geometries are equal, false if not.</returns>
		public static bool operator == (Geometry a, Geometry b)
		{
			bool aIsNull = ReferenceEquals(a, null);
			bool bIsNull = ReferenceEquals(b, null);
			if (aIsNull & bIsNull)
			{
				return true;
			}
			if (aIsNull)
			{
				return false;
			}
			if (bIsNull)
			{
				return false;
			}
			return a.Equals(b);
		}

		/// <summary>
		/// Override the != operator.
		/// </summary>
		/// <param name="a">The first geometry.</param>
		/// <param name="b">The second geometry.</param>
		/// <returns>False if geometries are equal, true if not.</returns>
		public static bool operator != (Geometry a, Geometry b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Override of the GetHashCode function.
		/// </summary>
		/// <returns>A hashcode of the combined vertices, colors, faces, Uvs, and Normals.</returns>
		public override int GetHashCode()
		{			
			return Utilities.CombineHashCodes(Vertices, Colors, Faces, Uvs, Normals);
		}

		/// <summary>
		/// Convert this geometry to json format.
		/// </summary>
		/// <param name="format">True will result in formatted json, false will result in an unformatted json string.</param>
		/// <returns>The geometry as json.</returns>
		public override string ToJSON(bool format)
		{
			//var serializationAdapter = new GeometrySerializationAdapter
			//{
			//	Data = Data
			//};

			var serializerSettings = new JsonSerializerSettings
			{
				Formatting = format == true ? Formatting.Indented : Formatting.None,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				ContractResolver = new CamelCaseCustomResolver()
			};

			return JsonConvert.SerializeObject(this, serializerSettings);
			//return JsonConvert.SerializeObject(serializationAdapter, serializerSettings);
		}
	}	

	/// <summary>
	/// 
	/// </summary>
	internal class GeometryData
	{
		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		internal List<float> RawVertices { get; set; }

		[JsonProperty("vertices")]
		internal IEnumerable<object> Vertices //OutputVertices
		{
			get
			{
				return Utilities.OptimizeFloats(RawVertices);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("colors")]
		internal List<int> Colors { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("faces")]
		internal List<int> Faces { get; set; }

		/// <summary>
		/// The list of UVs associated with this geometry.
		/// </summary>
		[JsonProperty("uvs")]
		public List<List<float>> Uvs { get; set; }

		/// <summary>
		/// The list of normals associated with this geometry.
		/// </summary>
		[JsonIgnore]
		internal List<float> RawNormals { get; set; }

		[JsonProperty("normals")]
		internal IEnumerable<object> Normals //OutputNormals
		{
			get
			{
				return Utilities.OptimizeFloats(RawNormals);
			}
		}

		internal GeometryData()
		{
			RawVertices = new List<float>();
			Colors = new List<int>();
			Faces = new List<int>();
			RawNormals = new List<float>();
			Uvs = new List<List<float>>();
		}

	}

	/// <summary>
	/// Class for storing geometry face data.
	/// </summary>
	internal class GeometryFace
	{
		/// <summary>
		/// False for triangle, true for quad.
		/// </summary>
		internal bool Topology { get; set; } //false for triangle, true for quad

		/// <summary>
		/// 
		/// </summary>
		internal bool FaceMaterial { get; set; }

		/// <summary>
		/// 
		/// </summary>
		internal bool FaceUVs { get; set; }

		/// <summary>
		/// 
		/// </summary>
		internal bool FaceVertexUVs { get; set; }

		/// <summary>
		/// 
		/// </summary>
		internal bool FaceNormals { get; set; }

		/// <summary>
		/// 
		/// </summary>
		internal bool VertexNormals { get; set; }

		/// <summary>
		/// 
		/// </summary>
		internal bool FaceColor { get; set; }

		/// <summary>
		/// 
		/// </summary>
		internal bool VertexColors { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal byte GetFaceType()
		{
			bool[] faceBits = new bool[] { Topology, FaceMaterial, FaceUVs, FaceVertexUVs,
										   FaceNormals, VertexNormals, FaceColor, VertexColors };
			System.Collections.BitArray bits = new System.Collections.BitArray(faceBits);

			byte b = 0;
			if (bits.Get(0)) b++;
			if (bits.Get(1)) b += 2;
			if (bits.Get(2)) b += 4;
			if (bits.Get(3)) b += 8;
			if (bits.Get(4)) b += 16;
			if (bits.Get(5)) b += 32;
			if (bits.Get(6)) b += 64;
			if (bits.Get(7)) b += 128;
			return b;
		}
	}

	//internal class GeometrySerializationAdapter : SerializationAdapter
	//{
	//	/// <summary>
	//	/// Geometry data.
	//	/// </summary>
	//	[JsonProperty("data", Order = 1)]
	//	internal GeometryData Data { get; set; }

	//	internal GeometrySerializationAdapter()
	//	{
	//		Metadata = new Metadata
	//		{
	//			Type = "Geometry",
	//			Version = 4.5, //3
	//			Generator = "ThreeLib-Geometry.toJSON"
	//		};

	//		Data = new GeometryData();
	//	}
	//}
}
