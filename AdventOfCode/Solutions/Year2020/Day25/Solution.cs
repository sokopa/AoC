using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day25 : ASolution
    {

        private long _cardPublicKey;
        private long _doorPublicKey;

        public Day25() : base(25, 2020, "")
        {
//             DebugInput = @"5764801
// 17807724".Replace("\r\n", "\n");

            var lines = Input.SplitByNewline();
            _cardPublicKey = long.Parse(lines[0]);
            _doorPublicKey = long.Parse(lines[1]);
        }

        private Dictionary<long, long> memo7 = new Dictionary<long, long>();

        private long TransformSubject(long loopSize, long subject = 7)
        {
            if (loopSize == 0)
            {
                memo7[loopSize] = 1;
                return subject;
            }
            else
            {
                var num = memo7[loopSize - 1];
                num *= subject;
                num %= 20201227;
                memo7[loopSize] = num;
                return num;
            }
        }

        

        protected override string SolvePartOne()
        {
            long? cardPrivate = null;
            long? doorPrivate = null;
            long i = 0;
            do
            {
                var loopNum = TransformSubject(i);
                if (loopNum == _cardPublicKey)
                {
                    cardPrivate = i;
                }

                if (loopNum == _doorPublicKey)
                {
                    doorPrivate = i;
                }

                i++;

            } while (cardPrivate == null || doorPrivate == null); 

            var encd = BigInteger.ModPow(new BigInteger(_doorPublicKey), new BigInteger(cardPrivate.Value), new BigInteger(20201227));
            var encc = BigInteger.ModPow(new BigInteger(_cardPublicKey), new BigInteger(doorPrivate.Value), new BigInteger(20201227));
            
            if (encd == encc)
            {
                return encd.ToString();
            }
            else
            {
                return "Not found";
            }
        }
        
        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}
