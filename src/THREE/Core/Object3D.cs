using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using THREE.Materials;
using THREE.Math;
using THREE.Objects;
using THREE.Utility;
using System.Linq;
using THREE.Geometries;
using THREE.Serialization;

namespace THREE.Core
{
	/// <summary>
	/// Base class for all objects. Analogous to https://threejs.org/docs/index.html#api/core/Object3D
	/// </summary>
	public class Object3D : Element
	{
		#region Properties

		/// <summary>
		/// Object visibility.
		/// </summary>		
		public bool Visible { get; set; }

		/// <summary>
		/// Flag for determining if object casts shadow.
		/// </summary>
		public bool CastShadow { get; set; }

		/// <summary>
		/// Flag for determining if object receives shadow.
		/// </summary>		
		public bool ReceiveShadow { get; set; }

		/// <summary>
		/// List with object's children.
		/// </summary>		
		public List<Object3D> Children { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public Object3D Parent { get; set; }

		/// <summary>
		/// Object user data.
		/// </summary>		
		public Dictionary<string, Dictionary<string, object>> UserData { get; set; }

		/// <summary>
		/// Object matrix.
		/// </summary>
		[JsonIgnore]
		public Matrix4 Matrix { get; set; }

		[JsonProperty("matrix")]
		public IEnumerable<object> MatrixArray { get { return Matrix.ToObjectList(); } }

		/// <summary>
		/// The object's local position.
		/// </summary>
		[JsonIgnore]
		public Vector3 Position { get { return Position; } set { Matrix.SetPosition(value); } }

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public Euler Rotation
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public Quaternion Quaternion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public Vector3 Scale { get; set; }
		
		public static Vector3 DefaultUp { get; set; }
		
		[JsonIgnore]
		internal Object3DSerializationAdapter SerializationAdapter { get; set; }

		#endregion

		#region Constructors

		static Object3D()
		{
			DefaultUp = new Vector3(0, 1, 0);
		}
		/// <summary>
		/// Default constructor. Results in an empty Object3D with new Uuid.
		/// </summary>
		public Object3D()
		{
			Children = new List<Object3D>();
			Matrix = Matrix4.Identity();
			Position = new Vector3();
			Rotation = new Euler();
			Quaternion = new Quaternion();
			Scale = new Vector3 { X = 1, Y = 1, Z = 1 };
			Parent = null;
		}

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		public void UpdateMatrix()
		{
			Matrix.Compose(Matrix.GetPosition(), Quaternion, Scale);
		}		

		/// <summary>
		/// Adds an object as a child of this object.
		/// </summary>
		/// <param name="obj"></param>
		public void Add(Object3D obj)
		{
			if (obj is Object3D obj3D)
			{
				obj3D.Parent = this;
			}

			//if (obj != null && obj.GetType().IsSubclassOf(typeof(Object3D)))
			//{
			//	(obj as Object3D).Parent = this;
			//}

			Children.Add(obj);
		}

		/// <summary>
		/// Adds a list of objects as children of this object.
		/// </summary>
		/// <param name="objects"></param>
		public void AddRange(IEnumerable<Object3D> objects)
		{
			Children.AddRange(objects);
		}

		/// <summary>
		/// Convert the object to JSON format. 
		/// </summary>
		/// <returns>A string representation of this object, serialized to JSON.</returns>
		/// <summary>
		/// Converts this object to a THREEJS-compatible JSON format.
		/// </summary>
		/// <returns>JSON String.</returns>
		public override string ToJSON(bool format)
		{
			SerializationAdapter = new Object3DSerializationAdapter();
			SerializationAdapter.Object.Name = Name;

			ProcessChildren();

			var serializerSettings = new JsonSerializerSettings
			{
				Formatting = format ? Formatting.Indented : Formatting.None,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				ContractResolver = new CamelCaseCustomResolver()
			};

			return JsonConvert.SerializeObject(SerializationAdapter, serializerSettings);
		}

		internal void ProcessChildren(Object3D obj = null)
		{
			List<Object3D> children;

			if (obj == null)
			{
				children = Children;
			}
			else
			{
				children = obj.Children;

				if (obj is Group)
				{
					if (!(obj.Parent is Group) && obj.Parent.Parent == null)
					{
						SerializationAdapter.Object.Children.Add(obj);
					}
				}
				else
				{
					if (obj.Parent.Parent == null)
					{
						SerializationAdapter.Object.Children.Add(obj);
					}
				}
			}

			foreach (var child in children)
			{
				if (child is Group group) //childObj.GetType() == typeof(Group)
				{
					ProcessChildren(group);
				}
				else
				{
					if (child is IGeometryContainer geometryContainer)
					{
						var geometry = geometryContainer.Geometry;

						//geometry.GetType().Name

						var id = SerializationAdapter.Geometries.AddIfNew(geometry);
						if (geometry is ITextGeometry textGeometry)
						{
							var font = SerializationAdapter.Fonts.AddIfNew(textGeometry.Parameters.Font.FontData);
							textGeometry.Parameters.Font.Data = font;
						}
						geometry.Uuid = id;
					}

					if (child is Object3D obj3D && obj3D.Children.Count > 0)
					{
						//obj3D.ProcessChildren();
						ProcessChildren(obj3D);
					}

					if (child is Mesh mesh)
					{
						if (mesh.Material is MeshStandardMaterial material)
						{
							foreach (var kvp in material.GetTextures())
							{
								if (kvp.Value != null)
								{
									var imageId = SerializationAdapter.Images.AddIfNew(kvp.Value.Image);
									kvp.Value.Image.Uuid = imageId;
									var textureId = SerializationAdapter.Textures.AddIfNew(kvp.Value);
									kvp.Value.Uuid = textureId;
								}
							}

							material.Uuid = SerializationAdapter.Materials.AddIfNew(material);
						}

						if (obj == null)
						{
							SerializationAdapter.Object.Children.Add(mesh);
						}
					}
					else if (child is Line line)
					{
						SerializationAdapter.Materials.Add(line.Material as LineBasicMaterial);

						if (obj == null)
						{
							SerializationAdapter.Object.Children.Add(line);
						}
					}
					else if (child is LineSegments lineSegments)
					{
						SerializationAdapter.Materials.Add(lineSegments.Material as LineBasicMaterial);

						if (obj == null)
						{
							SerializationAdapter.Object.Children.Add(lineSegments);
						}
					}
					else if (child is Points points)
					{

						SerializationAdapter.Materials.Add(points.Material as PointsMaterial);

						if (obj == null)
						{
							SerializationAdapter.Object.Children.Add(points);
						}
					}
					else
					{
						if (obj == null)
						{
							SerializationAdapter.Object.Children.Add(child);
						}
						//Debug.WriteLine((child as Element).Type, "ThreeLib");

						//switch ((child as Element).Type)
						//{
						//	case "PointLight":
						//	case "AmbientLight":
						//	case "SpotLight":
						//	case "DirectionalLight":
						//	case "HemisphereLight":
						//	case "PerspectiveCamera":
						//	case "OrthographicCamera":								
						//		break;
						//	default:
						//		Debug.WriteLine((child as Element).Type, "ThreeLib");
						//		break;
						//}
					}
				}


			}
		}

		#endregion
	}

	internal class Object3DSerializationAdapter : ObjectSerializationAdapter
	{
		[JsonProperty("object", Order = 5)]
		internal Object3D Object { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal Object3DSerializationAdapter()
		{
			Object = new Object3D();
			Metadata.Generator = "ThreeLib-Object3D.toJSON";
		}
	}
}
