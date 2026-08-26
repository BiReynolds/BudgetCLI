using BudgetCLI.Core.Interfaces;
using BudgetCLI.Core.Objects;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;

namespace BudgetCLI.Renderer
{
    public class BasicRenderer : IRenderer
    {
        public BasicRenderer()
        {

        }
        public void Render(OutputTokenBase outputToken)
        {
            switch (outputToken.OutputTokenType)
            {
                case OutputTokenEnum.SIMPLE_TEXT:
                    RenderSimpleText((SimpleTextOutput)outputToken);
                    return;
                default:
                    throw new OutputTokenNotSupportedException(outputToken);
            }
        }
        
        public void RenderSimpleText(SimpleTextOutput simpleTextOutput)
        {
            Console.WriteLine(simpleTextOutput.Content);
        }
    }
}