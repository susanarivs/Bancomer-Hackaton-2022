using System.Threading.Tasks;

namespace WhiteLabel.Core
{
	public interface ITriggerAction
	{
		Task Execute();
	}
}
