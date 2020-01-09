using System;
using System.Collections.Generic;

namespace TauCode.TextProcessing.Lab
{
    public class TextProcessingContext
    {
        private class Generation
        {
            public int StartingIndex { get; set; }
            public int LocalIndex { get; set; }
        }

        private readonly Stack<Generation> _generations;

        public TextProcessingContext(string text)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            _generations = new Stack<Generation>();
        }

        public void RequestGeneration()
        {
            throw new NotImplementedException();
        }

        public void ReleaseGeneration()
        {
            throw new NotImplementedException();
        }

        public string Text { get; }
        public int Depth => _generations.Count;

        public int GetCurrentColumn()
        {
            throw new NotImplementedException();
        }

        public int GetStartingIndex()
        {
            throw new NotImplementedException();
        }

        public int GetLocalIndex()
        {
            throw new NotImplementedException();
        }

        public bool IsEnd()
        {
            throw new NotImplementedException();
        }

        public void Advance(int indexShift, int lineShift, int currentColumn)
        {
            throw new NotImplementedException();
        }

        public char GetCurrentChar()
        {
            throw new NotImplementedException();
        }

        public void AdvanceByChar()
        {
            throw new NotImplementedException();
        }
    }
}
