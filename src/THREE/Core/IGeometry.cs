using System;

namespace THREE.Core
{
	/// <summary>
	/// 
	/// </summary>
	public interface IGeometry: IElement
	{
		
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IGeometryContainer
	{
		/// <summary>
		/// 
		/// </summary>
		IGeometry Geometry { get; set; }
	}
}
