namespace Companion.Modules
{
	/// <summary>
	/// Process user input. User input is delimited in the following way
	/// moduleName commandName parameter1 parameter2 etc
	/// </summary>
	public class UserRequest
	{
		public string ModuleName { get; set; } = string.Empty;

		public string CommandName { get; set; } = string.Empty;

		public string[] CommandParameters { get; set; } = new string[0];

		public UserRequest() { }

		public UserRequest(string userInput)
		{
			var userInputArray = userInput.Split(' ', StringSplitOptions.TrimEntries);
			ModuleName = userInputArray.ElementAtOrDefault(0) ?? string.Empty;
			CommandName = userInputArray.ElementAtOrDefault(1) ?? string.Empty;
			CommandParameters = userInputArray.Length > 2 ? userInputArray.Skip(2).ToArray() : new string[0];

		}
	}
}
