using DatabaseDeployer.Core.Model;

namespace DatabaseDeployer.Core.Services
{
	
	public interface ILogMessageGenerator
	{
		string GetInitialMessage(TaskAttributes taskAttributes);
	}
}