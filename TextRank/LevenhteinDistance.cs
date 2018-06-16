using System;

namespace TextRank
{
    sealed class LevenhteinDistance
    {
        private static int[,] _editDistanceArray;

        private static void BuildInitMatrix(int firstStringLength, int secondStringLength)
        {
            for (int i = 0; i <= firstStringLength; _editDistanceArray[i, 0] = i++)
            {
            }

            for (int j = 0; j <= secondStringLength; _editDistanceArray[0, j] = j++)
            {
            }
        }

        public int Calculate(string firstString, string secondString)
        {
            var firstStringLength = firstString.Length;
            var secondStringLength = secondString.Length;
            _editDistanceArray = new int[firstStringLength + 1, secondStringLength + 1];

            if (firstStringLength == 0)
            {
                return secondStringLength;
            }

            if (secondStringLength == 0)
            {
                return firstStringLength;
            }
           
           BuildInitMatrix(firstStringLength, secondStringLength);

            for (int i = 1; i <= firstStringLength; i++)
            {
                for (int j = 1; j <= secondStringLength; j++)
                {
                    int cost = secondString[j - 1] == firstString[i - 1] ? 0 : 1;

                    _editDistanceArray[i, j] = Math.Min(
                        Math.Min(_editDistanceArray[i - 1, j] + 1, _editDistanceArray[i, j - 1] + 1),
                        _editDistanceArray[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return _editDistanceArray[firstStringLength, secondStringLength];
        }
    }
}
