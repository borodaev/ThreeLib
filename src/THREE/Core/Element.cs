using Newtonsoft.Json;
using System;
using THREE.Serialization;

namespace THREE.Core
{
	public interface IElement
	{
		Guid Uuid { get; set; }

		string Name { get; set; }

		//string Type { get; set; }

		//IElement Copy(IElement other);

		//IElement Clone();

		//string ToJSON();
	}

	/// <summary>
	/// Base class for objects which have a Uuid, Name, and Type.
	/// </summary>
	public class Element : IElement
	{
		/// <summary>
		/// Unique Guid.
		/// </summary>		
		public Guid Uuid { get; set; }

		/// <summary>
		/// Name.
		/// </summary>		
		public string Name { get; set; }

		/// <summary>
		/// Type of object.
		/// </summary>		
		public string Type { get; set; }

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Element()
		{
			Uuid = Guid.NewGuid();
			Type = GetType().Name;
		}		

		/// <summary>
		/// Convert the object to JSON format. 
		/// </summary>
		/// <returns>A string representation of this object, serialized to JSON.</returns>
		/// <returns>JSON String.</returns>
		public virtual string ToJSON(bool format)
		{
			var serializerSettings = new JsonSerializerSettings
			{
				Formatting = format ? Formatting.Indented : Formatting.None,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				ContractResolver = new CamelCaseCustomResolver()
			};

			return JsonConvert.SerializeObject(this, serializerSettings);
		}
	}
}
