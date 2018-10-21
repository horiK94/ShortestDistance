using System;
using System.Diagnostics;

namespace ShortestDistance
{
    class Program
    {
        enum State
        {
            NONE = 0,
            START = 1,
            GOAL = 2,
            OBSTACLE = 3,
        }

        int width = 0, height = 0;
        State[,] stage = null;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.SetWidthAndHeight();
            p.InitStage();

            p.SetRandomPos(State.START);
            p.SetRandomPos(State.GOAL);

            Console.Write("Input obstacle num: ");
            int obstcleNum = int.Parse(Console.ReadLine());
            p.SetRandomPos(obstcleNum, State.OBSTACLE);

            p.ShowStage();
        }

        /// <summary>
        /// ステージの大きさを設定する
        /// </summary>
        void SetWidthAndHeight()
        {
            Console.Write("Input width: ");


            5this.width = int.Parse(Console.ReadLine());
            if (this.width <= 0)
            {
                Console.WriteLine("\nInvalid Value!");
            }

            Console.Write("Input height: ");
            this.height = int.Parse(Console.ReadLine());
            if (this.height <= 0)
            {
                Console.WriteLine("\nInvalid Value!");
            }
        }

        /// <summary>
        /// stageの初期化
        /// </summary>
        void InitStage()
        {
            if (width <= 0 || height <= 0)
            {
                Console.WriteLine("width か heightの値が0以下です");
                return;
            }

            stage = new State[width, height];
        }

        /// <summary>
        /// NONEとなっている場所にランダムでstateを変更する
        /// </summary>
        /// <param name="selectNum">Select number.</param>
        /// <param name="changeState">Change state.</param>
        void SetRandomPos(int selectNum, State changeState)
        {
            if (selectNum <= 0)
            {
                Console.WriteLine("selectnum は 0より大きい値を指定してください");
                return;
            }

            if (stage == null)
            {
                Console.WriteLine("stage is null");
                return;
            }

            //NONEになっている場所は何箇所あるか調査
            int canPutPosNum = 0;
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    if (stage[i, j] == State.NONE)
                    {
                        canPutPosNum++;
                    }
                }
            }

            if (canPutPosNum - selectNum < 0)
            {
                Console.WriteLine("置く数が置ける場所の数より多い");
                return;
            }

            //stateの変更を行う
            for (int i = 0; i < selectNum; i++)
            {
                SetRandomPos(changeState);
            }
        }

        void SetRandomPos(State changeState)
        {
            if (stage == null)
            {
                Console.WriteLine("stage is null");
                return;
            }

            //NONEになっている場所は何箇所あるか調査
            int canPutPosNum = 0;
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    if (this.stage[i, j] == State.NONE)
                    {
                        canPutPosNum++;
                    }
                }
            }

            if (canPutPosNum < 1)
            {
                Console.WriteLine("置く数が置ける場所の数より多い");
                return;
            }

            Random random = new Random();
            int r = random.Next(canPutPosNum);

            //先頭からr番目の位置のstate情報を更新する
            int count = 0;
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    if (stage[i, j] == State.NONE)
                    {
                        if (count == r)
                        {
                            stage[i, j] = changeState;
                            return;
                        }
                        count++;
                    }
                }
            }
        }

        void ShowStage()
        {
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    Console.Write(ConvertToSymbolFromState(this.stage[i, j]));
                }
                Console.WriteLine();
            }
        }

        char ConvertToSymbolFromState(State s)
        {
            switch (s)
            {
                case State.NONE:
                    return '_';
                case State.START:
                    return 'S';
                case State.GOAL:
                    return 'G';
                case State.OBSTACLE:
                    return '■';
                default:
                    Console.WriteLine("不正な値{0}が代入されました", s.ToString());
                    Debug.Assert(false);
                    return ' ';
            }
        }
    }
}