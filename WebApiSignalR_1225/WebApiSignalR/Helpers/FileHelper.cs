namespace WebApiSignalR.Helpers
{
    public class FileHelper
    {
        public static double Read()
        {
            return double.Parse(File.ReadAllText("data.txt"));
        }

        public static void Write(double data)
        {
            File.WriteAllText("data.txt",data.ToString());
        }

        public static double Read(string room)
        {
            return double.Parse(File.ReadAllText(room+".txt"));
        }

        public static void Write(string room,double data)
        {
            File.WriteAllText(room+".txt", data.ToString());
        }
    }
}
