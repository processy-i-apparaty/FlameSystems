namespace FlameSystems.Controls.Pickers.Models
{
    public static class GiveIdModel
    {
        private static int _id;
        public static int Get => _id++;
    }
}