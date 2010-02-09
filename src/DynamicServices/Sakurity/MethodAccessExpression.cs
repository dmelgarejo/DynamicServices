namespace DynamicServices.Sakurity
{
	using System;
	using System.Linq;
	using System.Reflection;

	public class MethodAccessExpression : ISecurityCheck
	{
		private readonly MethodInfo _Method;
		private readonly TypeAccessExpression _TypeAccess;
		private bool _Allow;
		private bool _Everyone;
		private string _Group;
		private int _Level = 1;
		private Guid _UserId = Guid.Empty;

		public MethodAccessExpression(string method, TypeAccessExpression typeAccess)
		{
			_TypeAccess = typeAccess;
			_Method = GetMethod(method);
			typeAccess.Registry.AddAction(ConfigyaOffia);
		}

		private void ConfigyaOffia(ISakurityOffica offica)
		{
			offica.AddCheck(this);
		}

		private MethodInfo GetMethod(string method)
		{
			method = method.ToLowerInvariant();
			var methodInfo = _TypeAccess.Type.GetMethods()
				.Where(m => m.Name.ToLowerInvariant() == method)
				.FirstOrDefault();
			if (methodInfo == null)
			{
				throw new ApplicationException(string.Format("Cannot find method {0} on type {1}", method, _TypeAccess.Type));
			}
			return methodInfo;
		}

		public MethodAccessExpression Allow()
		{
			_Allow = true;
			return this;
		}

		public MethodAccessExpression Deny()
		{
			_Allow = false;
			return this;
		}

		public MethodAccessExpression ForGroup(string group)
		{
			_Group = group;
			return this;
		}

		public MethodAccessExpression ForUser(Guid id)
		{
			_UserId = id;
			return this;
		}

		public MethodAccessExpression Everyone()
		{
			_Everyone = true;
			return this;
		}

		public MethodAccessExpression Level(int level)
		{
			_Level = level;
			return this;
		}

		public int GetLevel()
		{
			// ToDo use default level somehow?
			return _Level;
		}

		public SakurityResult Check(MethodInfo methodInfo)
		{
			if (methodInfo != _Method)
			{
				return SakurityResult.NotApplicable;
			}
			if (_Everyone)
			{
				return SakurityResult.Allow;
			}
			//todo implement other checks
			return SakurityResult.Deny;
		}
	}
}