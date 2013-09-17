namespace LiveLab3D.Windsor
{
	using Boo.Lang.Compiler.Ast;
	using Rhino.Commons.Binsor.Macros;

	public abstract class BaseEnvironmentMacro<TSetting, TInstaller> : BaseBinsorToplevelMacro<TSetting>
	{
		public override Statement Expand(MacroStatement macro)
		{
			var stmt = (ExpressionStatement) base.Expand(macro);
			Expression expr = stmt.Expression;
			ReferenceExpression initializeInstallerExpression =
				AstUtil.CreateReferenceExpression(typeof (TInstaller).FullName);
			var initializeInstallerInvoke = new MethodInvocationExpression(initializeInstallerExpression);
			var installMethod = new MemberReferenceExpression(initializeInstallerInvoke, "Register");
			var invokeInstallMethod = new MethodInvocationExpression(installMethod, expr);
			return new ExpressionStatement(invokeInstallMethod);
		}
	}
}