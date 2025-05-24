using System.Linq;
using MoreLinq;
using UnityEngine;
using UnityUtility.Collections;
using UnityUtility.Extensions;

public static class PointAssigner
{
    public static void AssignTo(SquareMatrix sources, PointWeight pointWeight, int min, int max)
    {
        var impl = PointAssignerFactory.Create(pointWeight, min, max);
        impl.AssignTo(sources);
    }

    public interface IPointAssignerImpl
    {
        void AssignTo(SquareMatrix sources);
    }
    public class PointAssignerFactory
    {
        public static IPointAssignerImpl Create(PointWeight pointWeight, int min, int max)
        {
            return pointWeight switch
            {
                PointWeight.Flat   => new FlatPointAssigner(min, max),
                PointWeight.Center => new CenterPointAssigner(min, max),
                PointWeight.Edge   => new EdgePointAssigner(min, max),
                PointWeight.Random => new RandomPointAssigner(min, max),
                _ => throw new System.NotImplementedException()
            };
        }
    }
    public abstract class PointAssignerImpl : IPointAssignerImpl
    {
        protected int Min { get; }
        protected int Max { get; }
        public PointAssignerImpl(int min, int max)
        {
            Min = min;
            Max = max;
        }
        public abstract void AssignTo(SquareMatrix sources);
    }

    public class FlatPointAssigner : PointAssignerImpl
    {
        public FlatPointAssigner(int min, int max) : base(min, max)
        {
        }

        public override void AssignTo(SquareMatrix sources)
        {
            foreach (var square in sources)
            {
                square.Point = 1;
            }
        }
    }
    public class CenterPointAssigner : PointAssignerImpl
    {
        public CenterPointAssigner(int min, int max) : base(min, max)
        {
        }

        public override void AssignTo(SquareMatrix sources)
        {
            // 中心の4マスから端のマスまでのマス数
            var distanceFromEdge_W = sources.ColumnSize / 2 - 1;
            var distanceFromEdge_H = sources.RowSize / 2 - 1;
            // 中心のマスをMaxとして、角のマスをMinとしたときに、1マスあたりのポイント減少量
            double deltaPerStep = (double)(Max - Min) / (distanceFromEdge_W + distanceFromEdge_H);
            // 中心のマスを取得
            var centerIndexes = sources.GetCenterIndexes();
            foreach (var (square, index) in sources.GetEnumeratorWithIndex())
            {
                // 4つの中心マスのうち、最も近いマスからのマンハッタン距離を取得
                var stepNumFromCenter = centerIndexes
                    .Select(centerIndex => (centerIndex.ToVector2Int() - index.ToVector2Int()).ManhattanDistance())
                    .Min();
                // ポイントはintで丸める
                square.Point = (int)(Max - stepNumFromCenter * deltaPerStep);
            }
        }
    }
    public class EdgePointAssigner : PointAssignerImpl
    {
        public EdgePointAssigner(int min, int max) : base(min, max)
        {
        }

        public override void AssignTo(SquareMatrix sources)
        {
            var edge = new Vector2Int(sources.RowSize / 2, sources.ColumnSize / 2);
            foreach (var (square, index) in sources.GetEnumeratorWithIndex())
            {
                var distance = Vector2Int.Distance(index.ToVector2Int(), edge);
                square.Point = (int)Mathf.Max(0, 1 - distance / (sources.RowSize / 2));
            }
        }
    }
    public class RandomPointAssigner : PointAssignerImpl
    {
        public RandomPointAssigner(int min, int max) : base(min, max)
        {
        }

        public override void AssignTo(SquareMatrix sources)
        {
            foreach (var square in sources)
            {
                square.Point = Random.Range(Min, Max);
            }
        }
    }

}