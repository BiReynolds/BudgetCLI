namespace BudgetCLI.Renderer
{
    public static class RenderHelper
    {
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

        public static string GetIdOrQuestionMark(int id)
        {
            if (id == -1)
            {
                return "?";
            }
            else
            {
                return id.ToString();
            }
        }
    }
}