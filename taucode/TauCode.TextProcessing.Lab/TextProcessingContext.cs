using System;
using System.Collections.Generic;

namespace TauCode.TextProcessing.Lab
{
    public class TextProcessingContext
    {
        #region Nested

        private class Generation
        {
            private readonly TextProcessingContext _holder;

            public Generation(TextProcessingContext holder)
            {
                _holder = holder;


                this.StartingIndex = holder.GetAbsoluteIndex();
                this.LocalIndex = 0;
                this.CurrentLine = holder.GetCurrentLine();
                this.CurrentColumn = holder.GetCurrentColumn();
            }

            public int StartingIndex { get; private set; }
            public int LocalIndex { get; private set; }
            public int CurrentLine { get; private set; }
            public int CurrentColumn { get; private set; }

            public int GetAbsoluteIndex() => this.StartingIndex + this.LocalIndex;
        }


        #endregion

        #region Fields

        private readonly Stack<Generation> _generations;

        #endregion

        #region Constructor

        public TextProcessingContext(string text)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            _generations = new Stack<Generation>();
            var rootGeneration = new Generation(this);
            _generations.Push(rootGeneration);
        }

        #endregion

        #region Private

        private Generation GetLastGeneration()
        {
            if (_generations.Count == 0)
            {
                return null;
            }

            return _generations.Peek();
        }

        #endregion

        #region Public

        public string Text { get; }

        public void RequestGeneration()
        {
            var generation = new Generation(this);
            _generations.Push(generation);
        }

        public void ReleaseGeneration()
        {
            throw new NotImplementedException();
        }

        public int Depth => _generations.Count;

        #endregion




        public int GetCurrentLine() => this.GetLastGeneration()?.CurrentLine ?? 0;

        private int GetAbsoluteIndex() => this.GetLastGeneration()?.GetAbsoluteIndex() ?? 0;

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
            var lastGeneration = _generations.Peek();
            var localIndex = lastGeneration.LocalIndex;

            return localIndex;
        }

        public bool IsEnd()
        {
            // todo checks
            var lastGeneration = _generations.Peek();
            var absoluteIndex = lastGeneration.StartingIndex + lastGeneration.LocalIndex;
            if (absoluteIndex > this.Text.Length)
            {
                throw new NotImplementedException();
            }

            return absoluteIndex == this.Text.Length;
        }

        public void Advance(int indexShift, int lineShift, int currentColumn)
        {
            throw new NotImplementedException();
        }

        public char GetCurrentChar()
        {
            // todo checks
            var absoluteIndex = this.GetAbsoluteIndex();
            return this.Text[absoluteIndex];
        }

        public void AdvanceByChar()
        {
            this.Advance(1, 0, this.GetCurrentColumn() + 1);
        }

        public char? GetPreviousChar()
        {
            // todo: checks
            var absoluteIndex = this.GetAbsoluteIndex();
            if (absoluteIndex == 0)
            {
                return null;
            }

            return this.Text[absoluteIndex];
        }
    }
}
