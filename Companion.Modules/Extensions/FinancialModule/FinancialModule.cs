using Companion.Modules.Extensions.FinancialModule.Providers;

namespace Companion.Modules.Extensions.FinancialModule
{
    /// <summary>
    /// This class introduces the help command, and will be inherited by more advanced modules.
    /// </summary>
    public class FinancialModule: WelcomeModule
    {
        public FinancialModule(IEXProvider iexProvider)
        {
            _availableCommands.AddCommand(new Command("-quote", "Gets the current price of a stock ticker. Example -quote aapl ", (commandText) => iexProvider.GetTickerPrice(commandText)));
        }

        public override bool ExecuteCommand(string[] commandText)
        {
            var wasCommandExecuted = base.ExecuteCommand(commandText);

            return wasCommandExecuted;
        }

    }
}