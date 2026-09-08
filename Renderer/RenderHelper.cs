using BudgetCLI.Data.Models;

namespace BudgetCLI.Renderer
{
    public static class RenderHelper
    {
        public static ConsoleColor DefaultResponseTextColor = ConsoleColor.Blue;
        public static ConsoleColor DefaultInputTextColor = ConsoleColor.White;
        public static Dictionary<RecurringTypeEnum, string> RecurringTypeToRenderableString = new()
        {
            {RecurringTypeEnum.NOT_RECURRING, "Not Recurring"},
            {RecurringTypeEnum.WEEKLY, "Weekly"},
            {RecurringTypeEnum.BIWEEKLY, "Biweekly"},
            {RecurringTypeEnum.MONTHLY, "Monthly"},
            {RecurringTypeEnum.FOUR_WEEKS, "Every 4 Weeks"}
        };
        public static string MakeColumnString(string content, int columnWidth, TextAlignment textAlignment = TextAlignment.LEFT)
        {
            int numBlanks = columnWidth - content.Length;
            if (numBlanks < 0)
            {
                return content[0..columnWidth];
            }
            switch (textAlignment)
            {
                case TextAlignment.RIGHT:
                    return new string(' ', numBlanks) + content;
                case TextAlignment.CENTER:
                    int leftSpaces = numBlanks / 2;
                    return new string(' ', leftSpaces) + content + new string(' ', (numBlanks - leftSpaces));
                default:
                    return content + new string(' ', numBlanks);
            }
        }

        public static string GetIdOrQuestionMark(int? id)
        {
            if (id == null || id == -1)
            {
                return "?";
            }
            else
            {
                return ((int)id).ToString();
            }
        }

        public static void SetColorsToDefaultResponse()
        {
            Console.ForegroundColor = DefaultResponseTextColor;
        }

        public static void SetColorsToDefaultInput()
        {
            Console.ForegroundColor = DefaultInputTextColor;
        }

    }
}