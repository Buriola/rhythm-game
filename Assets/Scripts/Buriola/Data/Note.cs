namespace Buriola.Data
{
    [System.Serializable]
    public class Note
    {
        public Note(int start, int length)
        {
            Start = start;
            Length = length;
        }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
