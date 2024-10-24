namespace AutoKitTest.Lib.Manifest
{
    /// <summary>
    /// コマンドタイプ
    /// </summary>
    internal enum CommandType
    {
        None,           //  何もしない
        ImageCheck,     //  スクリーンをテンプレートマッチで画像チェック
        AppOpen,        //  アプリケーションを起動
        AppClose,       //  アプリケーションを終了(プロセス終了)
        Click,          //  特定の場所をクリック
        Run,            //  指定のスクリプト文を実行
        Wait,           //  指定時間待機
        ScreenShot,     //  スクリーンショットを撮影
        FolderOpen,     //  フォルダを開く
        FolderClose,    //  フォルダを閉じる
    }
}
