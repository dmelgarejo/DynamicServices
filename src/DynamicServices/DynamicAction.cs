namespace DynamicServices
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class DynamicAction
	{
		public DynamicAction(MethodInfo methodInfo)
		{
			_Method = methodInfo;
		}

		protected MethodInfo _Method { get; set; }

		public virtual Type DeclaringType
		{
			get { return _Method.DeclaringType; }
		}

		public virtual bool IsCommand()
		{
			return _Method.ReturnType == typeof (void);
		}

		public virtual bool IsQuery()
		{
			return !IsCommand();
		}

		public virtual object Invoke(object instance, IDictionary<string, object> parameters)
		{
			return _Method.Invoke(instance, parameters.Select(p => p.Value).OfType<object>().ToArray());
		}

		public virtual IList<DynamicParameter> GetParameters()
		{
			var parameters = from p in _Method.GetParameters()
			                 select new DynamicParameter
			                        {
			                        	Name = p.Name,
			                        	Type = p.ParameterType
			                        };
			return parameters.ToList();
		}
	}
}