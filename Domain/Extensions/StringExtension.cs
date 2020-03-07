namespace Domain.Extensions
{
    public static class StringExtension
    {
        public static string ShortName(this string value)
        {
            var entity = "Entity";
            var relationship = "Relationship";
            int offset = 0;
            if (value.EndsWith(entity))
            {
                offset = entity.Length;
            }
            else if (value.EndsWith(relationship))
            {
                offset = relationship.Length;

            }
            return value.Substring(0, value.Length - offset);
        }
    }
}
