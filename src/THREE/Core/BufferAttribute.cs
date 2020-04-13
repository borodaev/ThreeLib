using Newtonsoft.Json;
using System;

namespace THREE.Core
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBufferAttribute: IElement
	{

	}

	/// <summary>
	/// 
	/// </summary>
	public class BufferAttribute: IBufferAttribute
	{		
		public Guid Uuid { get; set; }
		
		public string Name { get; set; }
		
		public int ItemSize { get; set; }
		
		public int Count { get; private set; }
		
		public string Type { get; set; }
		
		public bool Normalized { get; set; }
		
		public bool Dynamic { get; set; }
		
		public object[] Array { get; set; }

		public BufferAttribute()
		{
			Uuid = Guid.NewGuid();
			Type = GetType().Name;
		}

		public BufferAttribute(string type, object[] array, int itemSize, bool normalized) : this()
		{
			Type = type;
			ItemSize = itemSize;
			Array = array;
			Count = Array != null ? Array.Length / ItemSize : 0;
			Normalized = normalized;
			Dynamic = false;
		}

		public enum BufferAttributeType
		{
			Int8Array = 0,
			Uint8Array = 1,
			Uint8ClampedArray = 2,
			Int16Array = 3,
			Uint16Array = 4,
			Int32Array = 5,
			Uint32Array = 6,
			Float32Array = 7,
			Float64Array = 8
		}
	}
}
