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
        int[,] minStep = null;

        bool isExamine = true;

        static void Main(string[] args)
        {
            Program p = new Program();

            p.InitSetting();

            p.ShowStage();

            p.ShowHanding();
        }

        void InitSetting()
        {
            SetWidthAndHeight();

            InitStage();

            SetRandomPos(State.START);
            SetRandomPos(State.GOAL);

            SetObstacleNum();
        }

        /// <summary>
        /// ステージの大きさを設定する
        /// </summary>
        void SetWidthAndHeight()
        {
            Console.Write("Input width: ");
           
            this.width = int.Parse(Console.ReadLine());
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

        void SetObstacleNum()
        {
            Console.Write("Input obstacle num: ");
            int obstcleNum = int.Parse(Console.ReadLine());
            SetRandomPos(obstcleNum, State.OBSTACLE);
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

        void InitMinStep()
        {
            if (width <= 0 || height <= 0)
            {
                Console.WriteLine("width か heightの値が0以下です");
                return;
            }

            minStep = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    minStep[i, j] = -1;     //-1で初期化
                }
            }
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
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(ConvertToSymbolFromState(this.stage[j, i]));
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

        void SetStepAtGoalPos()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (stage[i, j] == State.GOAL)
                    {
                        //GOAL地点を0步目とする
                        minStep[i, j] = 0;
                    }
                }
            }
        }

        void SetStepForStage()
        {
            //調査開始
            isExamine = true;

            InitMinStep();
            SetStepAtGoalPos();

            int step = 1;

            while(isExamine)
            {
                SetStep(step);
                step++;
            }
        }

        void SetStep(int setStepNum)
        {
            bool isMove = false;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(minStep[i, j] == setStepNum - 1)
                    {
                        //最小距離としてstepNum - 1が登録されている時は、移動を開始する
                        isMove = CanMove(i, j) ? true : isMove;
                        Move(i, j, setStepNum);     //置けるかどうかを調べてから置く必要がある.順番注意
                    }
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(stage[i, j] == State.START && minStep[i, j] != -1)
                    {
                        //スタート位置に最小距離が登録されたなら、調査を終える
                        isExamine = false;
                        return;
                    }
                }
            }

            //次の場所にどこも塗れなくても調査を終了
            if(!isMove)
            {
                isExamine = false;
            }
        }

        void Move(int x, int y, int setStepNum)
        {
            //左
            SetStep(x - 1, y, setStepNum);
            //右
            SetStep(x + 1, y, setStepNum);
            //上
            SetStep(x, y - 1, setStepNum);
            //下
            SetStep(x, y + 1, setStepNum);
        }

        bool CanMove(int x, int y)
        {
            //左
            if(CanSetStep(x - 1, y))
            {
                return true;
            }
            //右
            if(CanSetStep(x + 1, y))
            {
                return true;
            }
            //上
            if(CanSetStep(x, y - 1))
            {
                return true;
            }
            //下
            if(CanSetStep(x, y + 1))
            {
                return true;
            }

            return false;
        }

        void  SetStep(int x, int y, int setStepNum)
        {
            if(CanSetStep(x, y))
            {
                minStep[x, y] = setStepNum;
            }
        }

        bool CanSetStep(int x, int y)
        {
            //添字チェック
            if (!CorrectIndex(x, y))
            {
                //添字が配列の範囲外の場合は移動終了
                return false;
            }

            if(stage[x, y] == State.OBSTACLE)
            {
                //移動先が障害物の場合も移動終了
                return false;
            }

            if (minStep[x, y] == -1)
            {
                //未調査の場所の場合は最小移動距離を登録できる
                return true;
            }
            return false;
        }

        bool CorrectIndex(int x, int y)
        {
            //配列の範囲チェック
            if(x < 0 || x >= width)
            {
                return false;
            }
            if (y < 0 || y >= height) 
            {
                return false;
            }
            return true;
        }

        void ShowHanding()
        {
            //最短経路の調査
            SetStepForStage();

            if (!CanMoveToStartFromGoal())
            {
                Console.WriteLine("スタートからゴールまで行ける道が存在しません");
                return;
            }

            int x = 0, y = 0;       //現在地
            int nowStep = -1;
            //スタート地点の設定
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (stage[i, j] == State.START)
                    {
                        x = i;
                        y = j;

                        nowStep = minStep[i, j];
                    }
                }
            }

            Console.Write("最短移動経路: ");
            //スタート地点から逆順でゴールまで向かう
            while(nowStep > 0)
            {
                if (CorrectIndex(x - 1, y) && minStep[x - 1, y] == nowStep - 1)
                {
                    //左が最短
                    Console.Write("←");
                    x--;
                }
                else if(CorrectIndex(x + 1, y) && minStep[x + 1, y] == nowStep - 1)
                {
                    //右が最短
                    Console.Write("→");
                    x++;
                }
                else if(CorrectIndex(x, y - 1) && minStep[x, y - 1] == nowStep - 1)
                {
                    //上が最短
                    Console.Write("↑");
                    y--;
                }
                else
                {
                    //下が最短
                    Console.Write("↓");
                    y++;
                }
                nowStep--;
            }
        }

        bool CanMoveToStartFromGoal()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(stage[i, j] == State.START)
                    {
                        return minStep[i, j] != -1;
                    }
                }
            }
            //ここまで来ることは無い
            Debug.Assert(false);
            return false;
        }
    }
}