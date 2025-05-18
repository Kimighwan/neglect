using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("testInt", "A", "B", "C")]
	public class ES3UserType_SaveTest : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SaveTest() : base(typeof(SaveTest)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (SaveTest)obj;
			
			writer.WritePrivateField("testInt", instance);
			writer.WritePrivateField("A", instance);
			writer.WritePrivateField("B", instance);
			writer.WritePrivateField("C", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (SaveTest)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "testInt":
					instance = (SaveTest)reader.SetPrivateField("testInt", reader.Read<System.Int32>(), instance);
					break;
					case "A":
					instance = (SaveTest)reader.SetPrivateField("A", reader.Read<System.Int32>(), instance);
					break;
					case "B":
					instance = (SaveTest)reader.SetPrivateField("B", reader.Read<System.Single>(), instance);
					break;
					case "C":
					instance = (SaveTest)reader.SetPrivateField("C", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SaveTestArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SaveTestArray() : base(typeof(SaveTest[]), ES3UserType_SaveTest.Instance)
		{
			Instance = this;
		}
	}
}