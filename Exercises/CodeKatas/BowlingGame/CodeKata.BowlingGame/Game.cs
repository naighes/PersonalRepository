using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingGame
{
    public class Game
    {
        public class Frame
        {
            internal Frame(Frame previousFrame, IList<Int32> throws)
            {
                _throws = throws;
                _index = previousFrame == null ? 0 : previousFrame.Index + 1;
                _firstThrowIndex = throws.Count;
            }

            private readonly IList<Int32> _throws;
            private readonly Int32 _firstThrowIndex;

            internal Int32 Index
            {
                get { return _index; }
            }
            private readonly Int32 _index;

            private Int32 GetKnockedOutPins(Int32 throwIndex)
            {
                return throwIndex < _throws.Count ? _throws[throwIndex] : 0;
            }

            private Boolean IsStrike
            {
                get { return GetKnockedOutPins(_firstThrowIndex) == TOTAL_PINS; }
            }

            private Boolean IsSpare
            {
                get { return GetKnockedOutPins(_firstThrowIndex) + GetKnockedOutPins(_firstThrowIndex + 1) == TOTAL_PINS; }
            }

            internal void CheckIfFrameIsComplete(Action frameComplete)
            {
                if (frameComplete == null)
                    return;

                if (IsStrike)
                    frameComplete();

                if (_firstThrowIndex + 1 < _throws.Count)
                    frameComplete();
            }

            internal Int32 Score
            {
                get
                {
                    var score = GetKnockedOutPins(_firstThrowIndex) + GetKnockedOutPins(_firstThrowIndex + 1);

                    if (IsStrike || IsSpare)
                        score += GetKnockedOutPins(_firstThrowIndex + 2);

                    return score;
                }
            }
        }

        public Game()
        {
            _throws = new List<Int32>();
            _frames = new List<Frame>();
            _frames.Add(new Frame(null, _throws));
        }

        private const Int32 TOTAL_PINS = 10;
        private readonly IList<Frame> _frames;

        public Int32 Score
        {
            get { return _frames.Sum(f => f.Score); }
        }

        internal Frame CurrentFrame
        {
            get { return _frames[_frames.Count - 1]; }
        }

        public void RegisterThrow(Int32 pins)
        {
            _throws.Add(pins);
            CurrentFrame.CheckIfFrameIsComplete(MoveNextFrame);
        }

        private void MoveNextFrame()
        {
            if (_frames.Count < TOTAL_PINS)
                _frames.Add(new Frame(CurrentFrame, _throws));
        }

        private readonly IList<Int32> _throws;

        public Int32 GetScoreUntilFrame(Frame frame)
        {
            return GetScoreUntilFrame(frame.Index);
        }

        public Int32 GetScoreUntilFrame(Int32 frameIndex)
        {
            return _frames.Where(f => f.Index <= frameIndex).Sum(f => f.Score);
        }
    }
}