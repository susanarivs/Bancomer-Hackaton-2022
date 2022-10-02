using System;
using System.Threading.Tasks;

namespace WhiteLabel.Core
{
	public static class TaskExtensions
	{
		public static event EventHandler<ForgottenExceptionEventArgs> ForgottenExceptionOccurred;

		public static void Forget(this Task task)
		{
			task.ContinueWith(delegate(Task t)
			{
				if (TaskExtensions.ForgottenExceptionOccurred != null)
				{
					TaskExtensions.ForgottenExceptionOccurred(null, new ForgottenExceptionEventArgs(t));
				}
			}, TaskContinuationOptions.OnlyOnFaulted);
		}
	}
}
