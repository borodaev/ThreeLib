using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using THREE.Core;
using THREE.Materials;
using THREE.Textures;
using System.Linq;

namespace THREE.Utility
{
	public abstract class SerializationAdapter
	{
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("metadata", Order = 0)]
		public Metadata Metadata { get; set; }

		internal SerializationAdapter()
		{
			Metadata = new Metadata
			{
				Version = 4.5,
				Generator = "ThreeLib"
			};
		}
	}	

	public abstract class ObjectSerializationAdapter : SerializationAdapter
	{
		[JsonProperty("geometries", Order = 1)]
		internal ElementCollection Geometries { get; set; }		

		[JsonProperty(Order = 2)]
		internal ElementCollection Images { get; set; }

		[JsonProperty(Order = 3)]
		internal ElementCollection Textures { get; set; }

		[JsonProperty(Order = 4)]
		internal ElementCollection Materials { get; set; }

		[JsonProperty(Order = 5)]
		internal ElementCollection Fonts { get; set; }

		internal ObjectSerializationAdapter()
		{
			Metadata.Type = "Object";
			Geometries = new ElementCollection();
			Materials = new ElementCollection();
			Fonts = new ElementCollection();
			Images = new ElementCollection();
			Textures = new ElementCollection();
		}

		public bool ShouldSerializeImages()
		{
			return Images.Count > 0;
		}

		public bool ShouldSerializeTextures()
		{
			return Textures.Count > 0;
		}

	}
}
