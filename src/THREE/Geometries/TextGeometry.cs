using Newtonsoft.Json;
using System;
using THREE.Core;

namespace THREE.Geometries
{
	/// <summary>
	/// 
	/// </summary>
	public class TextGeometryParameters : IEquatable<TextGeometryParameters>
	{
		/// <summary>
		/// An instance of THREE.Font.
		/// </summary>
		public Font Font { get; set; }

		/// <summary>
		/// Size of the text. Default is 100.
		/// </summary>
		public float Size { get; set; }

		/// <summary>
		/// Thickness to extrude text. Default is 50.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Number of points on the curves. Default is 12.
		/// </summary>
		public int CurveSegments { get; set; }

		/// <summary>
		/// Turn on bevel. Default is False.
		/// </summary>
		public bool BevelEnabled { get; set; }

		/// <summary>
		/// How deep into text bevel goes. Default is 10.
		/// </summary>
		public float BevelThickness { get; set; }

		/// <summary>
		/// How far from text outline is bevel. Default is 8.
		/// </summary>
		public float BevelSize { get; set; }

		/// <summary>
		/// Number of bevel segments. Default is 3.
		/// </summary>
		public int BevelSegments { get; set; }

		public bool Equals(TextGeometryParameters other)
		{
			if (other == null)
			{
				return false;
			}
			else
			{
				return BevelEnabled.Equals(other.BevelEnabled) &&
					   BevelSegments.Equals(other.BevelSegments) &&
					   BevelSize.Equals(other.BevelSize) &&
					   BevelThickness.Equals(other.BevelThickness) &&
					   CurveSegments.Equals(other.CurveSegments) &&
					   Font.Equals(other.Font) &&
					   Height.Equals(other.Height) &&
					   Size.Equals(other.Size);
			}
		}
	}

	public interface ITextGeometry
	{
		TextGeometryParameters Parameters { get; set; }
	}

	/// <summary>
	/// A class for generating text geometries.
	/// Analagous to: https://threejs.org/docs/index.html#api/en/geometries/TextGeometry
	/// JS Source: https://github.com/mrdoob/three.js/blob/master/src/geometries/TextGeometry.js
	/// </summary>	
	public class TextGeometry : Geometry, ITextGeometry, IEquatable<TextGeometry>
	{
		/// <summary>
		/// 
		/// </summary>
		public TextGeometryParameters Parameters { get; set; }

		bool IEquatable<TextGeometry>.Equals(TextGeometry other)
		{
			if (other == null)
			{
				return false;
			}
			else
			{
				return Parameters.Equals(other.Parameters);
			}
		}
	}

	public class TextBufferGeometry : Geometry, ITextGeometry, IEquatable<TextBufferGeometry>
	{
		/// <summary>
		/// 
		/// </summary>
		public TextGeometryParameters Parameters { get; set; }

		public TextBufferGeometry(TextGeometryParameters parameters)
		{
			Parameters = parameters;
		}

		bool IEquatable<TextBufferGeometry>.Equals(TextBufferGeometry other)
		{
			if (other == null)
			{
				return false;
			}
			else
			{
				return Parameters.Equals(other.Parameters);
			}
		}
	}
}