using System;
using System.Collections.Generic;
using System.Net;
using DotLiquid;
using Tekly.Common.Utils;
using Tekly.Tinker.Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace Tekly.Tinker.Routing
{
	public class ClassRoutes : Drop, ITinkerRoutes
	{
		public string DisplayName => m_descriptionAttribute?.Name;
		public string Description => m_descriptionAttribute?.Description;
		public bool Visible => m_descriptionAttribute != null;

		public List<RouteFunction> Functions => m_routeFunctions;

		private readonly object m_instance;
		private readonly TinkerServer m_tinkerServer;
		private readonly List<RouteFunction> m_routeFunctions = new List<RouteFunction>();
		private readonly DescriptionAttribute m_descriptionAttribute;

		public ClassRoutes(object instance, TinkerServer tinkerServer)
		{
			m_instance = instance;
			m_tinkerServer = tinkerServer;

			var methods = m_instance.GetType().GetMethods();
			var topLevelRoute = m_instance.GetType().GetAttribute<RouteAttribute>();

			Assert.IsNotNull(topLevelRoute, $"Missing Route Attribute on class [{instance.GetType().Name}");

			m_descriptionAttribute = instance.GetType().GetAttribute<DescriptionAttribute>();

			var root = topLevelRoute.Route;

			foreach (var method in methods) {
				var route = method.GetAttribute<RouteAttribute>();

				if (route == null) {
					continue;
				}

				try {
					var routeFunction = new RouteFunction(method, root, m_tinkerServer);
					m_routeFunctions.Add(routeFunction);
				} catch (Exception exception) {
					Debug.LogError($"Failed to create RouteFunction for method [{method.ReflectedType.Name}.{method.Name}]");
					Debug.LogException(exception);
				}
			}
		}

		public bool TryHandle(string route, HttpListenerRequest request, HttpListenerResponse response)
		{
			foreach (var routeFunction in m_routeFunctions) {
				if (!routeFunction.Matches(request)) {
					continue;
				}

				routeFunction.Invoke(m_instance, request, response);
				return true;
			}

			return false;
		}
	}
}