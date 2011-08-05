using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syncfusion.Windows.Shared
{
    
    public class WordOccurrence
    {
        // Fields
        private int m_nCaretEnd = -1;
        private int m_nCaretStart = -1;
        private string m_sWord = "";

        // Properties
        public int EndPosition
        {
            get
            {
                return m_nCaretEnd;
            }
            set
            {
                m_nCaretEnd = value;
            }
        }

        public int StartPosition
        {
            get
            {
                return m_nCaretStart;
            }
            set
            {
                m_nCaretStart = value;
            }
        }

        public string Word
        {
            get
            {
                return m_sWord;
            }
            set
            {
                m_sWord = value;
            }
        }

        // Methods
        public WordOccurrence(string word, int caretStart, int caretEnd)
        {
            m_sWord = word;
            m_nCaretStart = caretStart;
            m_nCaretEnd = caretEnd;
        }

        public int GetEndPosition()
        {
            return m_nCaretEnd;
        }

        public int GetStartPosition()
        {
            return m_nCaretStart;
        }

        public string GetWord()
        {
            return m_sWord;
        }
    }

    /// <exclude/>
    public class BadWord :
        WordOccurrence
    {
        // Fields
        private int m_nReason;
        public static int REASON_DUPLICATE = 1;
        public static int REASON_SPELLING = 2;

        // Methods
        public BadWord(string word, int caretStart, int caretEnd)
            : base(word, caretStart, caretEnd)
        {
            m_nReason = REASON_SPELLING;
        }

        public BadWord(string word, int caretStart, int caretEnd, int reason)
            : base(word, caretStart, caretEnd)
        {
            m_nReason = reason;
        }

        // Properties
        public int Reason
        {
            get
            {
                return m_nReason;
            }
        }

        public int GetReason()
        {
            return m_nReason;
        }
    }
}
