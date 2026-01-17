using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

# if UNITY_EDITOR
namespace Yuffter.GameStateMachine
{
    /// <summary>
    /// ビルド設定にあるシーンからSceneName列挙型を生成するエディターツール
    /// </summary>
    public static class SceneEnumGenerator
    {
        private const string OUTPUT_PATH = "Assets/GameStateMachine/Generated/SceneEnum.cs";

        [MenuItem("Tools/Yuffter/Game State Machine/Generate SceneEnum")]
        public static void Generate()
        {
            var scenes = EditorBuildSettings.scenes; // ビルド設定にあるシーンを取得

            var sb = new StringBuilder(); // 列挙型生成用のStringBuilder
            sb.AppendLine("namespace Yuffter.GameStateMachine");
            sb.AppendLine("{");
            sb.AppendLine("    public enum SceneName");
            sb.AppendLine("    {");

            foreach (var scene in scenes)
            {
                if (!scene.enabled) continue;

                var name = Path.GetFileNameWithoutExtension(scene.path); // シーンファイルのパスからファイル名を拡張子を削除して取得
                sb.AppendLine($"        {name},");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            Directory.CreateDirectory(Path.GetDirectoryName(OUTPUT_PATH)); // 出力先ディレクトリが存在しない場合は作成
            File.WriteAllText(OUTPUT_PATH, sb.ToString(), Encoding.UTF8); // ファイルに書き込み

            AssetDatabase.Refresh(); // 新しいファイルを認識させる
            Debug.Log("SceneEnum.cs has been generated successfully.");
        }
    }
}
# endif