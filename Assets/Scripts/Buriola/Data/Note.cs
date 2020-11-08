namespace Buriola.Data
{
    [System.Serializable]
    public class Note
    {
        private int _start;
        private int _length;
        
        public Note(int start, int length)
        {
            _start = start;
            _length = length;
        }
        
        public int Start
        {
            get => _start;
            set => _start = value;
        }

        public int Length
        {
            get => _length;
            set => _length = value;
        }
    }
}
