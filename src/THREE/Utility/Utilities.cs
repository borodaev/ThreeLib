using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace THREE.Utility
{
	/// <summary>
	/// 
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="floats"></param>
		/// <returns></returns>
		public static IEnumerable<object> OptimizeFloats(IEnumerable<float> floats)
		{
			return floats.Select(f =>
			{
				if (System.Math.Abs(f - System.Math.Floor(f)) <= float.Epsilon)
				{
					//Convert.ToInt16(f);
					return Convert.ToInt32(f);
				}
				else
				{
					return f;
				}
			}).Cast<object>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<object> Flatten(this IEnumerable<object> source)
		{
			return source.SelectMany(x => x is IEnumerable enumerable ? Flatten(enumerable.Cast<object>()) : new[] { x });
		}

		public static int CombineHashCodes(params object[] objects)
		{
			return CombineHashCodes(objects.Select(obj => ReferenceEquals(obj, null) ? 0 : obj.GetHashCode()));
		}

		public static int CombineHashCodes(params int[] hashCodes)
		{
			return CombineHashCodes(hashCodes);
		}

		public static int CombineHashCodes(IEnumerable<int> hashCodes)
		{
			//int hash1 = (5381 << 16) + 5381;
			//int hash2 = hash1;
			//int i = 0;
			//foreach (var hashCode in hashCodes)
			//{
			//	if (i % 2 == 0)
			//	{
			//		hash1 = ((hash1 << 5) + hash1 + (hash1 >> 27)) ^ hashCode;
			//	}
			//	else
			//	{
			//		hash2 = ((hash2 << 5) + hash2 + (hash2 >> 27)) ^ hashCode;
			//	}
			//	++i;
			//}
			//return hash1 + (hash2 * 1566083941); //unchecked?

			//var result = 0;
			//foreach (var hashCode in hashCodes)
			//{
			//	if (result == 0)
			//	{
			//		result = 17;
			//		//result = 5381;
			//	}

			//	unchecked
			//	{
			//		result = result * 31 + hashCode;
			//	}
			//	//result = ((result << 5) + result) ^ hashCode;
			//}
			//return result;

			//https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
			//7/13/17/23/31/37

			const int b = 378551;
			int a = 63689;
			var result = 0;

			foreach (var hashCode in hashCodes)
			{
				unchecked
				{
					result = result * a + hashCode;
					a = a * b;
				}
			}

			return result;
		}
	}
}
