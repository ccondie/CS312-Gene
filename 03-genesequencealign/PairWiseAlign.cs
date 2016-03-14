using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticsLab
{
    class PairWiseAlign
    {
        int MaxCharactersToAlign;
        Boolean runOnce = true;

        public PairWiseAlign()
        {
            // Default is to align only 5000 characters in each sequence.
            this.MaxCharactersToAlign = 5000;
        }

        public PairWiseAlign(int len)
        {
            // Alternatively, we can use an different length; typically used with the banded option checked.
            this.MaxCharactersToAlign = len;
        }

        /// <summary>
        /// this is the function you implement.
        /// </summary>
        /// <param name="sequenceA">the first sequence</param>
        /// <param name="sequenceB">the second sequence, may have length not equal to the length of the first seq.</param>
        /// <param name="banded">true if alignment should be band limited.</param>
        /// <returns>the alignment score and the alignment (in a Result object) for sequenceA and sequenceB.  The calling function places the result in the dispay appropriately.
        /// 
        public ResultTable.Result Align_And_Extract(GeneSequence sequenceA, GeneSequence sequenceB, bool banded)
        {
            ResultTable.Result result = new ResultTable.Result();
            int score;                                                       // place your computed alignment score here
            string[] alignment = new string[2];                              // place your two computed alignments here

            Console.WriteLine("Lenght of SeqeunceA: " + sequenceA.Sequence.Length + " " + sequenceA.Sequence.Substring(0,10));
            Console.WriteLine("Lenght of SeqeunceB: " + sequenceB.Sequence.Length + " " + sequenceB.Sequence.Substring(0, 10));
            Console.WriteLine();

            //A table to store the dynamic calculations ... uses the form of dynoTable[X, Y] or rather dynoTable[Col, Row] ... from the top left to the bottom right ... or quadrent 4
            int[,] dynoTable = new int[sequenceA.Sequence.Length, sequenceB.Sequence.Length];

            //A Table to store the previous locations for the best sequences. Entry prev[x,y] could only contain { [x-1, y] , [x, y-1], [x-1, y-1] }
            Coordinate[,] prev = new Coordinate[sequenceA.Sequence.Length, sequenceB.Sequence.Length];

            //Stores the inputed sequences into arrays for easy access of characters
            char[] seqA = sequenceA.Sequence.ToCharArray();
            char[] seqB = sequenceB.Sequence.ToCharArray();


            /*****************************************************************************************
            This is where the real code starts
            *****************************************************************************************/
            //Fill in the base cases into the first row and column of the dyno table (also potentially the prev table) ... sequenceA lies against the top of the chart, sequenceB along the left side
            //Iterate through each row from row 1 to row y
            //  Iterate through each column from col 1 to col x
            //      Calculate and store the value of the top location (also store the prev[x, y] as equal to x, y-1)
            //      Calculate the value of the corner location
            //          Compare with the value of dynoTable[x,y], it the new value is less update the prev and dynoTable values
            //      Calculate the value of the side location
            //          Compare with the value of dynoTable[x,y], it the new value is less update the prev and dynoTable values

            //Fill in the base cases into the first row and column of the dyno table (also potentially the prev table)
            for(int x = 0; x < seqA.Length; x++) {
                dynoTable[x, 0] = x * 5;
                prev[x, 0] = new Coordinate(x - 1, 0);
            }
            for(int y = 0; y < seqB.Length; y++) {
                dynoTable[0, y] = y * 5;
                prev[0, y] = new Coordinate(0, y - 1);
            }

            //Iterate through each row from row 1 to row y
            for(int y = 1; y < seqB.Length; y++) {
                //Iterate through each column from col 1 to col x
                for(int x = 1; x < seqA.Length; x++) {
                    int topVal;
                    int cornerVal;
                    int sideVal;

                    //Calculate and store the value of the top location (also store the prev[x, y] as equal to x, y-1)
                    topVal = dynoTable[x, y - 1] + 5;
                    dynoTable[x, y] = topVal;
                    prev[x, y] = new Coordinate(x, y - 1);

                    //Calculate the value of the corner location
                    if(seqA[x] == seqB[y])
                        cornerVal = dynoTable[x - 1, y - 1] - 3;
                    else
                        cornerVal = dynoTable[x - 1, y - 1] + 1;

                    //Compare with the value of dynoTable[x,y], it the new value is less update the prev and dynoTable values
                    if(cornerVal <= dynoTable[x, y]) {
                        dynoTable[x, y] = cornerVal;
                        prev[x, y] = new Coordinate(x - 1, y - 1);
                    }

                    //Calculate the value of the side location
                    sideVal = dynoTable[x - 1, y] + 5;

                    //Compare with the value of dynoTable[x,y], it the new value is less update the prev and dynoTable values
                    if (sideVal < dynoTable[x, y]) {
                        dynoTable[x, y] = sideVal;
                        prev[x, y] = new Coordinate(x - 1, y);
                    }
                }
            }

            if(runOnce) {

                for (int y = 0; y < dynoTable.GetLength(1); y++) {
                    for (int x = 0; x < dynoTable.GetLength(0); x++) {
                        Console.Write(dynoTable[x, y] + "\t");
                    }
                    Console.WriteLine();
                }

                runOnce = false;

            }

            






            // ********* these are placeholder assignments that you'll replace with your code  *******
            score = 0;                                                
            alignment[0] = "";
            alignment[1] = "";
            // ***************************************************************************************
            

            result.Update(score,alignment[0],alignment[1]);                  // bundling your results into the right object type 
            return(result);
        }


        public class Coordinate {
            int x;
            int y;

            public Coordinate(int x_p, int y_p) {
                x = x_p;
                y = y_p;
            }

            public int getX() { return x; }
            public int getY() { return y; }
        }
    }
}
