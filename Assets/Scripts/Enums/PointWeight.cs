public enum PointWeight
{
    /// <summary>
    /// すべて1
    /// </summary>
    Flat,
    /// <summary>
    /// 中心がポイント高め、端に行くほどポイント低め
    /// </summary>
    Center,
    /// <summary>
    /// 端がポイント高め、中心に行くほどポイント低め
    /// </summary>
    Edge,
    /// <summary>
    /// 完全ランダム
    /// </summary>
    Random
}