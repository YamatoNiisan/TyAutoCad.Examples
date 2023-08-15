using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// MLeader 拡張メソッド
    /// </summary>
    public static class MLeaderExtensions
    {
        #region GetLeaderLines ##--NOTEST--##
        /// <summary>
        /// MLeader の すべての引出線(LeaderLine)を取得する
        /// </summary>
        public static IEnumerable<MLeaderLine> GetLeaderLines(this MLeader mleader)
        {
            // MLeader の Leader Index を取得
            var leaders = mleader.GetLeaderIndexes().Cast<int>().ToList();

            foreach (var i in leaders)
            {
                // 各 Leader の LeaderLine Index を取得
                var leaderLines = mleader.GetLeaderLineIndexes(i).Cast<int>().ToList();
                // LeaderLine の FirstVertex を取得
                foreach (var j in leaderLines)
                {
                    yield return new MLeaderLine(j, mleader.GetFirstVertex(j), mleader.GetLastVertex(j));
                }
            }
        }
        #endregion

        #region GetLeaderLineFirstVertexes ##--NOTEST--##
        /// <summary>
        /// MLeader の すべての引出線の先頭点をを取得する
        /// </summary>
        /// <param name="ml"></param>
        /// <returns> Point3dCollection </returns>
        public static Point3dCollection GetLeaderLineFirstVertexes(this MLeader ml)
        {
            // 戻り値のコレクションを準備
            var vertexes = new Point3dCollection();
            // MLeader の Leader Index を取得
            var leaders = ml.GetLeaderIndexes().Cast<int>().ToList();

            foreach (var i in leaders)
            {
                // 各 Leader の LeaderLine Index を取得
                var leaderLines = ml.GetLeaderLineIndexes(i).Cast<int>().ToList();
                // LeaderLine の FirstVertex を取得
                foreach (var j in leaderLines)
                {
                    vertexes.Add(ml.GetFirstVertex(j));
                }
            }
            return vertexes;
        }
        #endregion

        #region GetLeaderLineLastVertexes ##--NOTEST--##
        /// <summary>
        /// MLeader の すべての引出線のテール頂点を取得する
        /// </summary>
        /// <param name="ml"></param>
        /// <returns> Point3dCollection </returns>
        public static Point3dCollection GetLeaderLineLastVertexes(this MLeader ml)
        {
            // 戻り値のコレクションを準備
            var vertexes = new Point3dCollection();
            // MLeader の Leader Index を取得
            var leaders = ml.GetLeaderIndexes().Cast<int>().ToList();

            foreach (var i in leaders)
            {
                // 各 Leader の LeaderLine Index を取得
                var leaderLines = ml.GetLeaderLineIndexes(i).Cast<int>().ToList();
                // LeaderLine の FirstVertex を取得
                foreach (var j in leaderLines)
                {
                    vertexes.Add(ml.GetLastVertex(j));
                }
            }
            return vertexes;
        }
        #endregion
    }
}
