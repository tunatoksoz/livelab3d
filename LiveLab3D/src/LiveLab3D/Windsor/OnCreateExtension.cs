namespace LiveLab3D.Windsor
{
	using Boo.Lang.Compiler.Ast;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;
	using Rhino.Commons.Binsor;
	using Rhino.Commons.Binsor.Macros;
	using Component = Rhino.Commons.Binsor.Component;

	public class OnCreateExtension : AbstractComponentExtension
	{
		#region Delegates

		public delegate void OnCreateActionDelegate(IKernel kernel, object item);

		#endregion

		private readonly OnCreateActionDelegate action;

		public OnCreateExtension(OnCreateActionDelegate onCreateActionDelegate)
		{
			this.action = onCreateActionDelegate;
		}

		public override void Apply(Component component, ComponentRegistration registration)
		{
			registration.OnCreate((kernel, item) =>
			                      this.action(kernel, item));
		}
	}


	public class OnCreateMacro : BaseBinsorExtensionMacro<OnCreateExtension>
	{
		public OnCreateMacro()
			: base("onCreate", false, "component")
		{
		}


		protected override bool ExpandExtension(ref MethodInvocationExpression extension, MacroStatement macro,
		                                        MacroStatement parent, ref Statement expansion)
		{
			extension.Arguments.Add(BuildInterceptHandler(macro));
			return true;
		}

		private static Expression BuildInterceptHandler(MacroStatement macro)
		{
			var e = new BlockExpression();

			e.Parameters.Add(new ParameterDeclaration("kernel", new SimpleTypeReference(typeof (IKernel).FullName)));
			e.Parameters.Add(new ParameterDeclaration("item", new SimpleTypeReference(typeof (object).FullName)));
			e.Body = macro.Body;
			return e;
		}
	}
}