using System.Text;

namespace ConsoleApp1
{
    public static class Tags
    {
        public enum indexs
        {
            NoData = 0,
            PlyFile = 1,
            JSONData = 2,
            AvatarPly = 3,
            MedicalPly = 4,
            RecivedData = 5,
            ChangedState = 6
        };

        public static string[] tageNames =
        {
            "nodata",
            "plyfile",
            "jsondata",
            "avatarplyfile",
            "medicalplyfile",
            "reciveddata",
            "changedstate"
        };

        public static string Encapsulate(string tagName, string data)
        {
            StringBuilder sb = new StringBuilder();
            MakeStartTag(tagName, ref sb);
            sb.Append(data);
            MakeEndTag(tagName, ref sb);
            return sb.ToString();
        }

        public static string MakeStartTag(string tagName)
        {
            return '<' + tagName + '>';
        }

        public static void MakeStartTag(string tagName, ref StringBuilder sb)
        {
            sb.Append('<');
            sb.Append(tagName);
            sb.Append('>');
        }

        public static string MakeEndTag(string tagName)
        {
            return "</" + tagName + '>';
        }

        public static void MakeEndTag(string tagName, ref StringBuilder sb)
        {
            sb.Append("</");
            sb.Append(tagName);
            sb.Append('>');
        }
    }
}
