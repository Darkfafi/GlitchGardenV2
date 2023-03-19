using RaCollection;

namespace Game
{
	public struct MechanicResponse
	{
		public bool IsSuccess;
		public string Message;

		public IRaLocator Locator
		{
			get; private set;
		}

		public static MechanicResponse CreateResponse(bool success, string message, params (string, object)[] data)
		{
			RaLocator locator = new RaLocator(null, null);

			if(data != null)
			{
				for(int i = 0; i < data.Length; i++)
				{
					var entry = data[i];
					locator.Register(entry.Item1, entry.Item2);
				}
			}

			return new MechanicResponse()
			{
				IsSuccess = success,
				Message = message,
				Locator = locator,
			};
		}

		public static MechanicResponse CreateSuccessResponse(params (string, object)[] data)
		{
			return CreateResponse(true, string.Empty, data);
		}

		public static MechanicResponse CreateFailedResponse(string message, params (string, object)[] data)
		{
			return CreateResponse(false, message, data);
		}
	}
}