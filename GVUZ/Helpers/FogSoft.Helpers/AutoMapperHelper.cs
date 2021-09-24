using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace FogSoft.Helpers
{
	public class AutoMapperHelper
	{
		public static void InitializeMapper(IEnumerable<Assembly> assemblies)
		{
			var config = Mapper.Configuration;

			foreach (var type in assemblies.SelectMany(assembly => assembly.GetTypesWith<AutoMappingAttribute>()))
			{
				foreach (AutoMappingAttribute attr in type.GetCustomAttributes(typeof(AutoMappingAttribute), true))
				{
					if (attr.Source != null) config.CreateMap(attr.Source, type);
					if (attr.Destination != null) config.CreateMap(type, attr.Destination);
				}

				var mapping = Activator.CreateInstance(type) as IAutoMapping;
				if (mapping != null) mapping.CreateMap(config);
			}
		}
	}

	public interface IAutoMapping
	{
		void CreateMap(IConfiguration config);
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class AutoMappingAttribute : Attribute
	{
		public Type Source { get; set; }
		public Type Destination { get; set; }
	}
}
