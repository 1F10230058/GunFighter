using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // ボタンから呼び出すためのpublicな関数
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}