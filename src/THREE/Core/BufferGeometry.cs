using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using THREE.Serialization;
using THREE.Utility;

namespace THREE.Core
{
	public class BufferGeometry : Element, IGeometry, IEquatable<BufferGeometry>
	{
		[JsonProperty("data")]
		BufferGeometryData Data { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public BufferGeometryBoundingSphere BoundingSphere
		{
			get { return Data.BoundingSphere; }
			set { Data.BoundingSphere = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public IDictionary<string, BufferAttribute> Attributes
		{
			get { return Data.Attributes; }
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public BufferAttribute Index
		{
			get { return Data.Index; }
			set { Data.Index = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public BufferGeometry()
		{
			Data = new BufferGeometryData();
		}

		/// <summary>
		/// Convert this BufferGeometry to json format.
		/// </summary>
		/// <param name="format">True will result in formatted json, false will result in an unformatted json string.</param>
		/// <returns>The geometry as json.</returns>
		public override string ToJSON(bool format)
		{
			//var serializationAdapter = new BufferGeometrySerializationAdapter
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

		public bool Equals(BufferGeometry other)
		{
			if (other == null)
			{
				return false;
			}
			else
			{
				return Data.Attributes.SequenceEqual(other.Data.Attributes) &&
					   Data.BoundingSphere.Equals(other.BoundingSphere);
			}
		}

		public override bool Equals(object other)
		{
			//return Equals(other as BufferGeometry);
			if (other.GetType() == typeof(BufferGeometry))
			{
				return Equals((BufferGeometry)other) && base.Equals(other);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Override of the == operator.
		/// </summary>
		/// <param name="a">The first buffer geometry.</param>
		/// <param name="b">The second buffer geometry.</param>
		/// <returns>True if buffer geometries are equal, false if not.</returns>
		public static bool operator == (BufferGeometry a, BufferGeometry b)
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
		/// <param name="a">The first buffer geometry.</param>
		/// <param name="b">The second buffer geometry.</param>
		/// <returns>False if buffer geometries are equal, true if not.</returns>
		public static bool operator != (BufferGeometry a, BufferGeometry b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Override of the GetHashCode function.
		/// </summary>
		/// <returns>A hashcode of the combined data.</returns>
		public override int GetHashCode()
		{			
			return Utilities.CombineHashCodes(Data.Attributes, Data.BoundingSphere);
		}
	}	

	internal class BufferGeometryData
	{
		[JsonProperty("attributes")]
		internal IDictionary<string, BufferAttribute> Attributes { get; private set; }

		[JsonProperty("index")]
		internal BufferAttribute Index { get; set; }

		[JsonProperty("boundingSphere")]
		internal BufferGeometryBoundingSphere BoundingSphere { get; set; }

		internal BufferGeometryData()
		{
			Attributes = new Dictionary<string, BufferAttribute>();
		}
	}

	/// <summary>
	/// Data for the bounding sphere.
	/// </summary>
	public class BufferGeometryBoundingSphere
	{
		/// <summary>
		/// Center position of the bounding sphere.
		/// </summary>
		[JsonProperty("center")]
		public float[] Center { get; set; }

		/// <summary>
		/// Radius of the bounding sphere.
		/// </summary>
		[JsonProperty("radius")]
		public float Radius { get; set; }

	}
	
	//internal class BufferGeometrySerializationAdapter : SerializationAdapter
	//{
	//	/// <summary>
	//	/// Geometry data.
	//	/// </summary>
	//	[JsonProperty("data", Order = 1)]
	//	internal BufferGeometryData Data { get; set; }

	//	internal BufferGeometrySerializationAdapter()
	//	{
	//		Metadata = new Metadata
	//		{
	//			Type = "BufferGeometry",
	//			//Version = 4.5, //3
	//			Generator = "ThreeLib-BufferGeometry.toJSON"
	//		};

	//		Data = new BufferGeometryData();
	//	}
	//}
}
